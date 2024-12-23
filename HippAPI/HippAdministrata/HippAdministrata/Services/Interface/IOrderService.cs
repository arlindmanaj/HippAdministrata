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
        Task<IEnumerable<Order>> GetBySalesPersonIdAsync(int salesPersonId);
        Task<Order> AssignOrderAsync(int orderId, OrderAssignmentDto assignmentDto);
        Task<Order> CreateOrderAsync(int clientId, OrderDto orderDto);
        Task<bool> UpdateAsync(Order order);
        Task<bool> DeleteAsync(int id);
        
    }
}
