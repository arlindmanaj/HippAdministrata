using HippAdministrata.Models.Domains;
using HippAdministrata.Models.Enums;
using HippAdministrata.Repositories.Implementation;
using HippAdministrata.Repositories.Interface;
using HippAdministrata.Services.Interface;
using HippAdministrata.Models.DTOs;
using HippAdministrata.Models.JunctionTables;
using HippAdministrata.Data;
using HippAdministrata.Hubs;
using Microsoft.AspNetCore.SignalR;
using HippAdministrata.Services.Implementation;
using HippAdministrata.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
    

namespace HippAdministrata.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IProductRepository _productRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationService _notificationService;
        private readonly IClientRepository _clientRepository;


        public OrderService(IProductRepository productRepository, IOrderRepository orderRepository, ApplicationDbContext applicationDbContext, INotificationService notificationService, IClientRepository clientRepository)
        {
            _orderRepository = orderRepository;
            _applicationDbContext = applicationDbContext;
            _productRepository = productRepository;
            _notificationService = notificationService;
            _clientRepository = clientRepository;
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


        public async Task<List<Order>> CreateMultipleOrdersAsync(int clientId, CreateOrderDto createOrderDto)
        {
            var salesperson = _applicationDbContext.SalesPersons.FirstOrDefault();
            if (salesperson == null)
                throw new Exception("No default salesperson available for the client.");

            var createdOrders = new List<Order>();

            foreach (var productDto in createOrderDto.Products)
            {
                var product = await _productRepository.GetByIdAsync(productDto.ProductId);
                if (product == null)
                    throw new Exception($"Product with ID {productDto.ProductId} not found.");

                if (productDto.Quantity > product.UnlabeledQuantity)
                    throw new Exception($"Requested quantity for product {product.Name} exceeds available stock.");

                product.UnlabeledQuantity -= productDto.Quantity;
                await _productRepository.UpdateProductAsync(product);

                var order = new Order
                {
                    ClientId = clientId,
                    ProductId = productDto.ProductId,
                    SalesPersonId = salesperson.Id,
                    DeliveryDestination = createOrderDto.DeliveryDestination,
                    Quantity = productDto.Quantity,
                    UnlabeledQuantity = productDto.Quantity,
                    LabeledQuantity = 0,
                    ProductPrice = product.Price,
                    OrderStatusId = (int?)OrderStatuses.Created,
                    CreatedAt = DateTime.UtcNow
                };

                createdOrders.Add(await _orderRepository.AddAsync(order));
            }
            // Send notification to Admins (RoleId = 1) and Managers (RoleId = 3)
            var rolesToNotify = new List<int> { 1, 3};

            foreach (var roleId in rolesToNotify)
            {
                await _notificationService.AddNotificationForRoleAsync(
                    roleId,
                    $" {createdOrders.Count} new order(s) have been placed by client ID {clientId} !",
                    "New Order"
                );
            }

            return createdOrders;
        }
        //public async Task<List<Order>> CreateMultipleOrdersAsync(int clientId, CreateOrderDto createOrderDto)
        //{
        //    var salesperson = _applicationDbContext.SalesPersons.FirstOrDefault();
        //    if (salesperson == null)
        //        throw new Exception("No default salesperson available for the client.");

        //    var createdOrders = new List<Order>();

        //    foreach (var productDto in createOrderDto.Products)
        //    {
        //        var product = await _productRepository.GetByIdAsync(productDto.ProductId);
        //        if (product == null)
        //            throw new Exception($"Product with ID {productDto.ProductId} not found.");

        //        if (productDto.Quantity > product.UnlabeledQuantity)
        //            throw new Exception($"Requested quantity for product {product.Name} exceeds available stock.");

        //        product.UnlabeledQuantity -= productDto.Quantity;
        //        await _productRepository.UpdateProductAsync(product);

        //        var order = new Order
        //        {
        //            ClientId = clientId,
        //            ProductId = productDto.ProductId,
        //            SalesPersonId = salesperson.Id,
        //            DeliveryDestination = createOrderDto.DeliveryDestination,
        //            Quantity = productDto.Quantity,
        //            UnlabeledQuantity = productDto.Quantity,
        //            LabeledQuantity = 0,
        //            ProductPrice = product.Price,
        //            OrderStatusId = (int?)OrderStatuses.Created, // Mark it as created
        //            CreatedAt = DateTime.UtcNow
        //        };

        //        var createdOrder = await _orderRepository.AddAsync(order);
        //        createdOrders.Add(createdOrder);

        //        // Create an entry in OrderRequests with CreatedAt instead of RequestedAt
        //        var orderRequest = new OrderRequest
        //        {
        //            OrderId = createdOrder.Id,
        //            ClientId = clientId,
        //            RequestType = "Create", // For example, "Create" could be a valid request type
        //            Status = "Pending", // You can adjust the status here as needed
        //            CreatedAt = DateTime.UtcNow // Use CreatedAt instead of RequestedAt
        //        };

        //        _applicationDbContext.OrderRequests.Add(orderRequest);
        //    }

        //    // Save changes to the database
        //    await _applicationDbContext.SaveChangesAsync();

        //    // Send notification to Admins (RoleId = 1) and Managers (RoleId = 3)
        //    var rolesToNotify = new List<int> { 1, 3 };

        //    foreach (var roleId in rolesToNotify)
        //    {
        //        await _notificationService.AddNotificationForRoleAsync(
        //            roleId,
        //            $"{createdOrders.Count} new order(s) have been placed by client ID {clientId}!",
        //            "New Order"
        //        );
        //    }

        //    return createdOrders;
        //}





        public async Task SimulateShippingAsync(int driverId, int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new Exception("Order not found");

            if (order.DriverId != driverId)
                throw new Exception("You are not assigned to this order");

            if (order.OrderStatusId != (int?)OrderStatuses.ReadyForShipping)
                throw new Exception("Order is not ready for shipping");

            order.OrderStatusId = (int?)OrderStatuses.InTransit;
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
                    order.OrderStatusId = (int?)OrderStatuses.Shipped;
                }
                else
                {
                    order.OrderStatusId = (int?)OrderStatuses.InTransit; 
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
            order.OrderStatusId = (int)OrderStatuses.InProgress;

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
            if (order == null || order.OrderStatusId != (int?)OrderStatuses.Created)
                throw new Exception("Order not found or cannot be assigned.");

            // Update assignments
            order.EmployeeId = assignmentDto.EmployeeId;
            order.DriverId = assignmentDto.DriverId;
            order.WarehouseId = assignmentDto.WarehouseId;
            order.OrderStatusId = (int?)OrderStatuses.InProgress;
            order.LastUpdated = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order);
            return order;
        }



        //public async Task<bool> DeleteAsync(int id)
        //{
        //    return await _orderRepository.DeleteAsync(id);
        //}
        public async Task<bool> DeleteAsync(int id)
        {
           // // Fetch related order requests first
           // var orderRequests = await _applicationDbContext.OrderRequests
           //     .Where(or => or.OrderId == id)
           //     .ToListAsync(); // Force query execution

           // //if (orderRequests.Any())
           // //{
           // //    _applicationDbContext.OrderRequests.RemoveRange(orderRequests);
           // //    await _applicationDbContext.SaveChangesAsync(); // Commit order request deletion first
           // //}

           // //Now delete the order itself
           //var order = await _orderRepository.GetByIdAsync(id);
           // if (order == null)
           //     throw new Exception("Order not found");

            return await _orderRepository.DeleteAsync(id); // Pass order ID instead of object
        }
        //public async Task<bool> DeleteAsync(int id)
        //{
        //    var order = await _applicationDbContext.Orders
        //        .AsNoTracking() // Prevents tracking issues
        //        .FirstOrDefaultAsync(o => o.Id == id);

        //    if (order == null)
        //    {
        //        throw new Exception($"Order {id} not found in the database before deletion.");
        //    }

        //    Console.WriteLine($"Deleting Order {id}: ProductId = {order.ProductId}, WarehouseId = {order.WarehouseId}");

        //    _applicationDbContext.Orders.Remove(order);
        //    await _applicationDbContext.SaveChangesAsync();

        //    return true;
        //}





        // Notification Request for Order
        // Get pending requests (update/delete)
        public async Task<List<OrderRequest>> GetPendingRequestsAsync()
        {
            return await _applicationDbContext.OrderRequests
                .Where(o => o.Status == "Pending")  // Filter to get only pending requests
                .ToListAsync();
        }


        // Approve an order request (update/delete)
        //public async Task<bool> ApproveRequestAsync(int requestId)
        //{
        //    var request = await _applicationDbContext.OrderRequests.FindAsync(requestId);
        //    if (request == null || request.Status != "Pending")
        //        return false;

        //    // Mark the request as approved
        //    request.Status = "Approved";
        //    request.ReviewedAt = DateTime.UtcNow;

        //    // Approve the action (update or delete order)
        //    var order = await _applicationDbContext.Orders.FindAsync(request.OrderId);
        //    if (order == null)
        //        return false;

        //    if (request.RequestType == "Delete")
        //    {
        //        _applicationDbContext.Orders.Remove(order);
        //    }
        //    else if (request.RequestType == "Update")
        //    {
        //        // Handle update logic here if needed (e.g., update some order properties)
        //        // Example: order.Status = "Updated"; 
        //    }

        //    await _applicationDbContext.SaveChangesAsync();
        //    return true;
        //}

        // Reject an order request
        public async Task<bool> RejectRequestAsync(int requestId)
        {
            var request = await _applicationDbContext.OrderRequests.FindAsync(requestId);
            if (request == null || request.Status != "Pending")
                return false;

            // Mark the request as rejected
            request.Status = "Rejected";
            request.ReviewedAt = DateTime.UtcNow;

            await _applicationDbContext.SaveChangesAsync();
            return true;
        }
        // OrderService.cs

        // OrderService.cs

        public async Task<Order> UpdateOrderAsync(int orderId, UpdateOrderDto updateOrderDto)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
            {
                throw new ArgumentException("Order not found");
            }

            // Update the order properties based on the DTO
            order.DeliveryDestination = updateOrderDto.DeliveryDestination;
            order.Quantity = updateOrderDto.Quantity; // This will overwrite with the new quantity
            order.ProductId = updateOrderDto.ProductId; // This will change the product ID
            order.LastUpdated = DateTime.UtcNow; // Set LastUpdated timestamp
            order.UnlabeledQuantity = updateOrderDto.Quantity; // Update the unlabeled quantity    
            // Call the repository's UpdateAsync method to persist the changes
            bool updateSuccess = await _orderRepository.UpdateAsync(order);

            if (updateSuccess)
            {
                return order; // Return the updated order if the update was successful
            }
            else
            {
                throw new InvalidOperationException("Failed to update order");
            }
        }



    }
}
