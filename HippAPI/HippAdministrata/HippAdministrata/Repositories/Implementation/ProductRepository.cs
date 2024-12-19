using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Repositories.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<bool> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }

}
