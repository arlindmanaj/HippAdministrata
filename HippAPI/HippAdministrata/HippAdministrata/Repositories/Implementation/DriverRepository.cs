using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
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

        public async Task<Driver> GetByIdAsync(int id) => await _context.Set<Driver>().FindAsync(id);

        public async Task<IEnumerable<Driver>> GetAllAsync() => await _context.Set<Driver>().ToListAsync();

        public async Task<IEnumerable<Driver>> GetByCarModelAsync(string carModel)
        {
            return await _context.Set<Driver>().Where(d => d.CarModel == carModel).ToListAsync();
        }

        public async Task<IEnumerable<Driver>> GetByLicensePlateAsync(string licensePlate)
        {
            return await _context.Set<Driver>().Where(d => d.LicensePlate == licensePlate).ToListAsync();
        }

        public async Task<bool> CreateAsync(Driver driver)
        {
            await _context.Set<Driver>().AddAsync(driver);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Driver driver)
        {
            _context.Set<Driver>().Update(driver);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var driver = await GetByIdAsync(id);
            if (driver != null)
            {
                _context.Set<Driver>().Remove(driver);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
