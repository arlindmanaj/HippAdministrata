using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace HippAdministrata.Repositories.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int productId)
        {
            // Fetch the product and related orders
            var product = await _context.Products
                                         // Assuming there's a navigation property `Orders` in Product
                                        .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                throw new Exception("Product not found");

            // Delete all orders associated with the product
            var relatedOrders = await _context.Orders.Where(o => o.ProductId == productId).ToListAsync();
            _context.Orders.RemoveRange(relatedOrders);

            // Delete the product
            _context.Products.Remove(product);

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }

}
