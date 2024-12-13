using HippAdministrata.Models.Domains;
namespace HippAdministrata.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(string id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetByMenaxherIdAsync(string menaxherId);
        Task<IEnumerable<Product>> GetUnlabeledProductsAsync();
        Task<bool> CreateAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateQuantitiesAsync(string id, decimal labeled, decimal unlabeled);
        Task<bool> UpdateImageAsync(string id, string imageUrl, string imagePublicId);
    }
}
