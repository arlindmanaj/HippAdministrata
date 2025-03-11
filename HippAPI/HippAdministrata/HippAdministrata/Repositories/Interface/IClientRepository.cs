using HippAdministrata.Models.Domains;

namespace HippAdministrata.Repositories.Interface
{
    public interface IClientRepository
    {
        Task<Client?> GetClientByUserIdAsync(int userId);
    }

}
