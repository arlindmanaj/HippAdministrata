using HippAdministrata.Models.Domains;

namespace HippAdministrata.Repositories.Interface
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetByClientIdAsync(int clientId);
        Task<IEnumerable<Order>> GetBySalesPersonIdAsync(int salesPersonId);
        Task<bool> CreateAsync(Order order);
        Task<bool> UpdateAsync(Order order);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int id, string status);
    }
}
