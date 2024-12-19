using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Models.JunctionTables;
using HippAdministrata.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Repositories.Implementation
{
    public class DriverRepository : IDriverRepository
    {
        private readonly ApplicationDbContext _context;

        public DriverRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Driver> GetByIdAsync(int id)
        {
            return await _context.Drivers
                .Include(d => d.User)
                .Include(d => d.Order)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Driver>> GetAllAsync()
        {
            return await _context.Drivers
                .Include(d => d.User) // Include User
                .ToListAsync();
        }

        public async Task<IEnumerable<Driver>> GetByCarModelAsync(string carModel)
        {
            return await _context.Drivers
                .Where(d => d.CarModel == carModel)
                .ToListAsync();
        }

        public async Task<IEnumerable<Driver>> GetByLicensePlateAsync(string licensePlate)
        {
            return await _context.Drivers
                .Where(d => d.LicensePlate == licensePlate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CarDrivers>> GetCarDriversByDriverIdAsync(int driverId, int carId)
        {
            return await _context.Set<CarDrivers>()
                .Where(cd => cd.DriverId == driverId)
                .Include(cd => cd.Driver)
                .ToListAsync();
        }

        public async Task<bool> CreateAsync(Driver driver)
        {
            await _context.Drivers.AddAsync(driver);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Driver driver)
        {
            _context.Drivers.Update(driver);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var driver = await GetByIdAsync(id);
            if (driver != null)
            {
                _context.Drivers.Remove(driver);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }

}
