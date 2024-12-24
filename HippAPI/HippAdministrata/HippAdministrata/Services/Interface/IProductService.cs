using HippAdministrata.Models.DTOs;
using HippAdministrata.Models.Domains;

namespace HippAdministrata.Services.Interface
{
    public interface IProductService
    {
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> AddAsync(ProductDto productDto);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, ProductDto productDto);
    }

}
