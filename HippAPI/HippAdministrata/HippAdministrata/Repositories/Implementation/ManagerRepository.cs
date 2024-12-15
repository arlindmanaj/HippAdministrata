using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Repositories.Implementation
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly ApplicationDbContext _context;

        public ManagerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Manager> GetByIdAsync(int id)
        {
            return await _context.Set<Manager>()
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Manager>> GetAllAsync()
        {
            return await _context.Set<Manager>()
                .Include(m => m.User)
                .ToListAsync();
        }


        public async Task<bool> UpdateAsync(int id, Manager updatedManager)
        {
            var existingManager = await GetByIdAsync(id);
            if (existingManager == null) return false;

            existingManager.Name = updatedManager.Name;
            existingManager.Password = updatedManager.Password;
            existingManager.UserId = updatedManager.UserId;

            _context.Set<Manager>().Update(existingManager);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var manager = await GetByIdAsync(id);
            if (manager == null) return false;

            _context.Set<Manager>().Remove(manager);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
