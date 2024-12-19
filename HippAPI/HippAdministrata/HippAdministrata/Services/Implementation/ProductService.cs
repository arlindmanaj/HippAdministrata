using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interface;
using HippAdministrata.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<bool> CreateAsync(Product product)
        {
            // Add validation logic if needed (e.g., prevent duplicate names)
            return await _productRepository.CreateAsync(product);
        }


        public async Task<bool> DeleteAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }
    }

}
