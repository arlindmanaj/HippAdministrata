using HippAdministrata.Models.Domains;

namespace HippAdministrata.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<bool> CreateAsync(Product product);
        Task<bool> DeleteAsync(int id);
    }
}
