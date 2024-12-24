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

        public OrderService(IOrderRepository orderRepository, ApplicationDbContext applicationDbContext)
        {
            _orderRepository = orderRepository;
            _applicationDbContext = applicationDbContext;
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


        public async Task<Order> CreateOrderAsync(int clientId, OrderDto orderDto)
        {
            // Automatically assign salesperson (example logic)
            var salesperson =  _applicationDbContext.SalesPersons.FirstOrDefault();
            if (salesperson == null)
                throw new Exception("No default salesperson available for the client.");

            // Create and save order
            var order = new Order
            {
                ClientId = clientId,
                ProductId = orderDto.ProductId,
                SalesPersonId = salesperson.Id,
                DeliveryDestination = orderDto.DeliveryDestination,
                Quantity = orderDto.Quantity,
                UnlabeledQuantity = orderDto.Quantity, // Initialize as total quantity
                LabeledQuantity = 0,
                OrderStatus = OrderStatus.Created,
                CreatedAt = DateTime.UtcNow
            };

            return await _orderRepository.AddAsync(order);
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

        public async Task<bool> UpdateAsync(Order order)
        {
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }

     
    }
}
