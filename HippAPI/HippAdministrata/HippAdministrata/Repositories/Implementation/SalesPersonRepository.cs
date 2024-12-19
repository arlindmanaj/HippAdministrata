using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Repositories.Implementation
{
    public class SalesPersonRepository : ISalesPersonRepository
    {
        private readonly ApplicationDbContext _context;

        public SalesPersonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SalesPerson> GetByIdAsync(int id)
        {
            return await _context.Set<SalesPerson>()
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<SalesPerson>> GetAllAsync()
        {
            return await _context.Set<SalesPerson>()
                .Include(e => e.User)
                .ToListAsync();
        }


        public async Task<bool> UpdateAsync(int id, SalesPerson updatedSalesPerson)
        {
            var existingSalesPerson = await GetByIdAsync(id);
            if (existingSalesPerson == null) return false;

            existingSalesPerson.Username =  updatedSalesPerson.Username;
            existingSalesPerson.Password = updatedSalesPerson.Password;
            existingSalesPerson.UserId = updatedSalesPerson.UserId;

            _context.Set<SalesPerson>().Update(existingSalesPerson);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var salesPerson = await GetByIdAsync(id);
            if (salesPerson == null) return false;

            _context.Set<SalesPerson>().Remove(salesPerson);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
