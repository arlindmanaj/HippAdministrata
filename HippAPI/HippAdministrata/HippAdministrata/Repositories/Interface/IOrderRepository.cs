using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Repositories.Interface
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetByClientIdAsync(int clientId);
        Task<IEnumerable<Order>> GetBySalesPersonIdAsync(int salesPersonId);
        Task<Order?> GetByIdWithProductAsync(int orderId);
        Task<bool> UpdateAsync(Order order);
        Task<List<OrderDto>> GetOrdersBySalesPersonIdAsync(int salesPersonId);
        Task<IEnumerable<Order>> GetOrdersByEmployeeIdAsync(int employeeId);
        Task UpdateOrderAsync(Order order);
        Task<Order> AddAsync(Order order);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int id, string status);
    }
}
