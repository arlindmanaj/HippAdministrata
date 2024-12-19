using HippAdministrata.Models.Domains;
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
        Task<bool> CreateAsync(Order order);
        Task<bool> UpdateAsync(Order order);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int id, OrderStatus status);
        Task<bool> AssignEmployeeToOrder(int orderId, int employeeId);
        Task<bool> AssignDriverToOrder(int orderId, int driverId);

        Task<bool> ProcessOrderRequestAsync(int requestId, OrderProcessRequestDto request);
    }
}
