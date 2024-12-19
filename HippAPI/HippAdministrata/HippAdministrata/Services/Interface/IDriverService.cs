using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;
namespace HippAdministrata.Services.Interface
{
    public interface IDriverService
    {
        Task<Driver> GetByIdAsync(int id);
        Task<IEnumerable<Driver>> GetAllAsync();
        Task<bool> UpdateAsync(Driver driver);
        Task<bool> DeleteAsync(int id);
      
        Task<bool> ShipOrderAsync(int driverId, int orderId);
    }
}
