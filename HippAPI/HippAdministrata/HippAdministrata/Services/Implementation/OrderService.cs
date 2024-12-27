using HippAdministrata.Models.Domains;
using HippAdministrata.Models.Enums;
using HippAdministrata.Repositories.Implementation;
using HippAdministrata.Repositories.Interface;
using HippAdministrata.Services.Interface;
using HippAdministrata.Models.DTOs;
using HippAdministrata.Models.JunctionTables;
using HippAdministrata.Data;

namespace HippAdministrata.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IProductRepository _productRepository;

        public OrderService(IProductRepository productRepository, IOrderRepository orderRepository, ApplicationDbContext applicationDbContext)
        {
            _orderRepository = orderRepository;
            _applicationDbContext = applicationDbContext;
            _productRepository = productRepository;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Order>> GetByClientIdAsync(int clientId)
        {
            return await _orderRepository.GetByClientIdAsync(clientId);
        }

        public async Task<IEnumerable<Order>> GetBySalesPersonIdAsync(int salesPersonId)
        {
            return await _orderRepository.GetBySalesPersonIdAsync(salesPersonId);
        }

        public async Task<List<Order>> CreateMultipleOrdersAsync(int clientId, OrderDto orderDto)
        {
            var salesperson = _applicationDbContext.SalesPersons.FirstOrDefault();
            if (salesperson == null)
                throw new Exception("No default salesperson available for the client.");

            var createdOrders = new List<Order>();

            foreach (var productDto in orderDto.Products)
            {
                // Fetch the product
                var product = await _productRepository.GetByIdAsync(productDto.ProductId);
                if (product == null)
                    throw new Exception($"Product with ID {productDto.ProductId} not found.");

                if (productDto.Quantity > product.UnlabeledQuantity)
                    throw new Exception($"Requested quantity for product {product.Name} exceeds available stock.");

                // Deduct stock
                product.UnlabeledQuantity -= productDto.Quantity;
                await _productRepository.UpdateProductAsync(product);

                // Create the order
                var order = new Order
                {
                    ClientId = clientId,
                    ProductId = productDto.ProductId,
                    SalesPersonId = salesperson.Id,
                    DeliveryDestination = orderDto.DeliveryDestination,
                    Quantity = productDto.Quantity,
                    UnlabeledQuantity = productDto.Quantity,
                    LabeledQuantity = 0,
                    ProductPrice = product.Price,
                    OrderStatus = OrderStatus.Created,
                    CreatedAt = DateTime.UtcNow
                };

                createdOrders.Add(await _orderRepository.AddAsync(order));
            }

            return createdOrders;
        }

        public async Task SimulateShippingAsync(int driverId, int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new Exception("Order not found");

            if (order.DriverId != driverId)
                throw new Exception("You are not assigned to this order");

            if (order.OrderStatus != OrderStatus.ReadyForShipping)
                throw new Exception("Order is not ready for shipping");

            order.OrderStatus = OrderStatus.InTransit;
            order.LastUpdated = DateTime.UtcNow;
            await _orderRepository.UpdateOrderAsync(order);

            // Simulate updates
            for (int i = 1; i <= 5; i++)
            {
                await Task.Delay(2000); // Simulate time passing

                // Update order status
                order.LastUpdated = DateTime.UtcNow;
                if (i == 5)
                {
                    order.OrderStatus = OrderStatus.Shipped;
                }
                else
                {
                    order.OrderStatus = OrderStatus.InTransit; 
                }
                await _orderRepository.UpdateOrderAsync(order);
            }
        }

        public async Task<Order> UpdateOrderAssignmentAsync(int orderId, OrderAssignmentDto assignmentDto)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new Exception("Order not found.");

            // Allow updating assignments regardless of status
            if (assignmentDto.EmployeeId.HasValue)
                order.EmployeeId = assignmentDto.EmployeeId.Value;

            if (assignmentDto.DriverId.HasValue)
                order.DriverId = assignmentDto.DriverId.Value;

            if (assignmentDto.WarehouseId.HasValue)
                order.WarehouseId = assignmentDto.WarehouseId.Value;

            order.LastUpdated = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order);
            return order;
        }

        public async Task<List<OrderDto>> GetOrdersBySalesPersonIdAsync(int salesPersonId)
        {
            return await _orderRepository.GetOrdersBySalesPersonIdAsync(salesPersonId);
        }




        public async Task<Order> AssignOrderAsync(int orderId, OrderAssignmentDto assignmentDto)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || order.OrderStatus != OrderStatus.Created)
                throw new Exception("Order not found or cannot be assigned.");

            // Update assignments
            order.EmployeeId = assignmentDto.EmployeeId;
            order.DriverId = assignmentDto.DriverId;
            order.WarehouseId = assignmentDto.WarehouseId;
            order.OrderStatus = OrderStatus.InProgress;
            order.LastUpdated = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order);
            return order;
        }

       

        public async Task<bool> DeleteAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }

     
    }
}
