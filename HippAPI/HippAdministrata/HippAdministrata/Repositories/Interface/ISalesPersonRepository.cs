using HippAdministrata.Models.Domains;

namespace HippAdministrata.Repositories.Interface
{
    public interface ISalesPersonRepository
    {
        Task<SalesPerson> GetByIdAsync(int id);
        Task<IEnumerable<SalesPerson>> GetAllAsync();
        Task<bool> UpdateAsync(int id, SalesPerson updatedSalesPerson);
        Task<bool> DeleteAsync(int id);
    }
}
