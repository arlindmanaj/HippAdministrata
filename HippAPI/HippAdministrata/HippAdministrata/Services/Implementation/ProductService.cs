using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> GetByIdAsync(int id) => await _productRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Product>> GetAllAsync() => await _productRepository.GetAllAsync();

        public async Task<IEnumerable<Product>> GetUnlabeledProductsAsync() => await _productRepository.GetUnlabeledProductsAsync();

        public async Task<bool> CreateAsync(Product product) => await _productRepository.CreateAsync(product);

        public async Task<bool> UpdateAsync(Product product)
        {
            try
            {
                return await _productRepository.UpdateAsync(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Optional: Add any additional business logic here (e.g., logging or retrying)
                return false;
            }
        }


        public async Task<bool> DeleteAsync(int id) => await _productRepository.DeleteAsync(id);

        public async Task<bool> UpdateQuantitiesAsync(int id, decimal labeled, decimal unlabeled)
        {
            return await _productRepository.UpdateQuantitiesAsync(id, labeled, unlabeled);
        }
    }
}
