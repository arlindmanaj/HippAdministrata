using HippAdministrata.Models.Domains;

namespace HippAdministrata.Repositories.Interface
{
    public interface IWarehouseRepository
    {
        Task<Warehouse> CreateAsync(Warehouse warehouse);
        Task<Warehouse?> GetByIdAsync(int id);
        Task<IEnumerable<Warehouse>> GetAllAsync();
        Task<Warehouse> UpdateAsync(Warehouse warehouse);
        Task DeleteAsync(int id);
    }

}
