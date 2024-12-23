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
            return await _context.Set<Product>().FindAsync(id);
        }

       
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Set<Product>().ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetUnlabeledProductsAsync()
        {
            return await _context.Set<Product>().Where(p => p.UnlabeledQuantity > 0).ToListAsync();
        }

        public async Task<bool> CreateAsync(Product product)
        {
            await _context.Set<Product>().AddAsync(product);
            return await _context.SaveChangesAsync() > 0;
        }


        //public async Task<bool> UpdateAsync(Product product)
        //{
        //    _context.Set<Product>().Update(product);
        //    return await _context.SaveChangesAsync() > 0;
        //}
        public async Task<bool> UpdateAsync(Product product)
        {
            var existingProduct = await _context.Set<Product>().FindAsync(product.Id); // Solution 1
            if (existingProduct == null) return false;

            existingProduct.Name = product.Name;
           
            existingProduct.UnlabeledQuantity = product.UnlabeledQuantity;
            existingProduct.LabeledQuantity = product.LabeledQuantity;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                _context.Set<Product>().Remove(product);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> UpdateQuantitiesAsync(int id, decimal labeled, decimal unlabeled)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                product.LabeledQuantity = labeled;
                product.UnlabeledQuantity = unlabeled;
                _context.Set<Product>().Update(product);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
