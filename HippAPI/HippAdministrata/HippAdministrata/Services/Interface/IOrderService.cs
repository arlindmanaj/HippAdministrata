﻿using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;
using HippAdministrata.Models.Enums;

namespace HippAdministrata.Services.Interface
{
    public interface IOrderService
    {
        Task<Order> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetByClientIdAsync(int clientId);
        Task SimulateShippingAsync(int driverId, int orderId);
        Task<IEnumerable<Order>> GetBySalesPersonIdAsync(int salesPersonId);
        Task<Order> AssignOrderAsync(int orderId, OrderAssignmentDto assignmentDto);
        Task<List<Order>> CreateMultipleOrdersAsync(int clientId, CreateOrderDto createOrderDto);
        Task<Order> UpdateOrderAssignmentAsync(int orderId, OrderAssignmentDto assignmentDto);
        Task<List<OrderDto>> GetOrdersBySalesPersonIdAsync(int salesPersonId);
        Task<Order> UpdateOrderAsync(int orderId, UpdateOrderDto updateOrderDto);

        Task<bool> DeleteAsync(int id);
        Task<List<OrderRequest>> GetPendingRequestsAsync();  // Get pending order requests
        //Task<bool> ApproveRequestAsync(int requestId); // Approve an order request
        Task<bool> RejectRequestAsync(int requestId);  // Reject an order request
    }
}
