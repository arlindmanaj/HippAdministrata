using HippAdministrata.Models.Domains;

namespace HippAdministrata.Repositories.Interface
{
    public interface IWarehouseRepository
    {
        Task<Warehouse> GetByIdAsync(int id);
        Task<IEnumerable<Warehouse>> GetAllAsync();
        Task<IEnumerable<Order>> GetOrdersByWarehouseAsync(int warehouseId);
        Task<bool> CreateAsync(Warehouse warehouse);
        Task<bool> UpdateAsync(Warehouse warehouse);
        Task<bool> DeleteAsync(int id);
    }
}
