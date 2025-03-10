﻿using HippAdministrata.Models.Domains;
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

namespace HippAdministrata.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IProductRepository _productRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationService _notificationService;


        public OrderService(IProductRepository productRepository, IOrderRepository orderRepository, ApplicationDbContext applicationDbContext, INotificationService notificationService)
        {
            _orderRepository = orderRepository;
            _applicationDbContext = applicationDbContext;
            _productRepository = productRepository;
            _notificationService = notificationService;
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
            var rolesToNotify = new List<int> { 1, 3 };

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

       

        public async Task<bool> DeleteAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }

     
    }
}
