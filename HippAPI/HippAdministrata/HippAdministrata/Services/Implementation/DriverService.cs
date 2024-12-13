using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interface;

namespace HippAdministrata.Services.Implementation
{
    public class DriverService
    {
        private readonly IDriverRepository _driverRepository;

        public DriverService(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<Driver> GetByIdAsync(int id) => await _driverRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Driver>> GetAllAsync() => await _driverRepository.GetAllAsync();

        public async Task<IEnumerable<Driver>> GetByCarModelAsync(string carModel) => await _driverRepository.GetByCarModelAsync(carModel);

        public async Task<IEnumerable<Driver>> GetByLicensePlateAsync(string licensePlate) => await _driverRepository.GetByLicensePlateAsync(licensePlate);

        public async Task<bool> CreateAsync(Driver driver) => await _driverRepository.CreateAsync(driver);

        public async Task<bool> UpdateAsync(Driver driver) => await _driverRepository.UpdateAsync(driver);

        public async Task<bool> DeleteAsync(int id) => await _driverRepository.DeleteAsync(id);
    }
}
