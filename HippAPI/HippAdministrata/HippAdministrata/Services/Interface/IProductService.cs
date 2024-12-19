using HippAdministrata.Models.Domains;

namespace HippAdministrata.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<bool> CreateAsync(Product product);
        Task<bool> DeleteAsync(int id);
    }
}
