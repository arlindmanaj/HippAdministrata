using HippAdministrata.Models.Domains;
using HippAdministrata.Models.Enums;

namespace HippAdministrata.Services.Interface
{
    public interface IWarehouseService
    {
        Task<Warehouse> CreateWarehouseAsync(string name, Location location);
        Task<Warehouse?> GetWarehouseByIdAsync(int id);
        Task<IEnumerable<Warehouse>> GetAllWarehousesAsync();
        Task<Warehouse> UpdateWarehouseAsync(int id, string name, Location location);
        Task DeleteWarehouseAsync(int id);
    }

}
