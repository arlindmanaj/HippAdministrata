using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;
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

            public async Task<Product?> GetByIdAsync(int id)
            {
                return await _productRepository.GetByIdAsync(id);
            }

            public async Task<IEnumerable<Product>> GetAllAsync()
            {
                return await _productRepository.GetAllAsync();
            }

            public async Task<Product> AddAsync(ProductDto productDto)
            {
                var product = new Product
                {
                    Name = productDto.Name,
                    UnlabeledQuantity = productDto.UnlabeledQuantity,
                    LabeledQuantity = productDto.LabeledQuantity
                };

                return await _productRepository.AddAsync(product);
            }

            public async Task DeleteAsync(int id)
            {
                await _productRepository.DeleteAsync(id);
            }

            public async Task UpdateAsync(int id, ProductDto productDto)
            {
                var existingProduct = await _productRepository.GetByIdAsync(id);
                if (existingProduct == null) throw new Exception("Product not found");

                existingProduct.Name = productDto.Name;
                existingProduct.UnlabeledQuantity = productDto.UnlabeledQuantity;
                existingProduct.LabeledQuantity = productDto.LabeledQuantity;
                existingProduct.UpdatedAt = DateTime.UtcNow;

                await _productRepository.UpdateAsync(existingProduct);
            }
        }

    }
