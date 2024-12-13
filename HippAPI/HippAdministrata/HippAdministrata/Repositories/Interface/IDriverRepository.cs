using HippAdministrata.Models.Domains;

namespace HippAdministrata.Repositories.Interface
{
    public interface IDriverRepository
    {
        Task<Driver> GetByIdAsync(int id);
        Task<IEnumerable<Driver>> GetAllAsync();
        Task<IEnumerable<Driver>> GetByCarModelAsync(string carModel);
        Task<IEnumerable<Driver>> GetByLicensePlateAsync(string licensePlate);
        Task<bool> CreateAsync(Driver driver);
        Task<bool> UpdateAsync(Driver driver);
        Task<bool> DeleteAsync(int id);
    }
}
