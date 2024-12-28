using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;
namespace HippAdministrata.Services.Interface
{
    public interface IDriverService
    {
        Task<Driver> GetByIdAsync(int id);
        Task TransferProductBetweenWarehouses(int productId, int sourceWarehouseId, int destinationWarehouseId);
        Task<IEnumerable<Driver>> GetAllAsync();
        Task<bool> UpdateAsync(Driver driver);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<OrderDto>> GetAssignedOrdersAsync(int driverId);



    }
}
