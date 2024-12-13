using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Repositories.Implementation
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly ApplicationDbContext _context;

        public WarehouseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Warehouse> GetByIdAsync(int id) => await _context.Set<Warehouse>().FindAsync(id);

        public async Task<IEnumerable<Warehouse>> GetAllAsync() => await _context.Set<Warehouse>().ToListAsync();

        public async Task<IEnumerable<Order>> GetOrdersByWarehouseAsync(int warehouseId)
        {
            return await _context.Set<Order>().Where(o => o.WarehouseId == warehouseId).ToListAsync();
        }

        public async Task<bool> CreateAsync(Warehouse warehouse)
        {
            await _context.Set<Warehouse>().AddAsync(warehouse);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Warehouse warehouse)
        {
            _context.Set<Warehouse>().Update(warehouse);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var warehouse = await GetByIdAsync(id);
            if (warehouse != null)
            {
                _context.Set<Warehouse>().Remove(warehouse);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
