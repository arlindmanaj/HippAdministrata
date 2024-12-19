using HippAdministrata.Models.Domains;

namespace HippAdministrata.Repositories.Interface
{
    public interface IClientRepository
    {
        Task<Client> GetByIdAsync(int id);
        Task<IEnumerable<Client>> GetAllAsync();
        Task<bool> CreateAsync(Client client);
        Task<bool> UpdateAsync(int id, Client updatedClient);
        Task<bool> DeleteAsync(int id);

    }
}
