using HippAdministrata.Models.Domains;

namespace HippAdministrata.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetUnlabeledProductsAsync();
        Task<bool> CreateAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateQuantitiesAsync(int id, decimal labeled, decimal unlabeled);
    }
}
