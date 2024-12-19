using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Models.Enums;
using HippAdministrata.Models.JunctionTables;
using HippAdministrata.Repositories.Interface;
using HippAdministrata.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Services.Implementation
{

    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly ApplicationDbContext _context;

        public DriverService(IDriverRepository driverRepository, ApplicationDbContext context)
        {
            _driverRepository = driverRepository;
            _context = context;
        }

        public async Task<Driver> GetByIdAsync(int id)
        {
            return await _driverRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Driver>> GetAllAsync()
        {
            return await _driverRepository.GetAllAsync();
        }

        public async Task<bool> UpdateAsync(Driver driver)
        {
            return await _driverRepository.UpdateAsync(driver);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _driverRepository.DeleteAsync(id);
        }

       
        public async Task<bool> ShipOrderAsync(int driverId, int orderId)
        {
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null) return false;

            var order = driver.Order;
            if (order == null || order.Id != orderId) return false;

            // Simulated shipping logic
            order.DeliveryDestination = "In Transit";
            order.OrderStatus = OrderStatus.Shipped;

            return await _driverRepository.UpdateAsync(driver);
        }
    }
}
