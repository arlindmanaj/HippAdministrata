using HippAdministrata.Models.Domains;

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

        Task UpdateOrderAsync(Order order);
        Task<Order> AddAsync(Order order);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int id, string status);
    }
}
