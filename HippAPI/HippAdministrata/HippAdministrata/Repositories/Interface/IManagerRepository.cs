using HippAdministrata.Models.Domains;

namespace HippAdministrata.Repositories.Interface
{
    public interface IManagerRepository
    {
        Task<Manager> GetByIdAsync(int id);
        Task<IEnumerable<Manager>> GetAllAsync();
        Task<bool> UpdateAsync(int id, Manager updatedManager);
        Task<bool> DeleteAsync(int id);
    }
}
