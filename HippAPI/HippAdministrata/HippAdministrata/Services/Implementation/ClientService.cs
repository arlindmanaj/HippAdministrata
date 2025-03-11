using HippAdministrata.Repositories.Interface;
using HippAdministrata.Services.Interface;

namespace HippAdministrata.Services.Implementation
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<int?> GetClientIdByUserIdAsync(int userId)
        {
            var client = await _clientRepository.GetClientByUserIdAsync(userId);
            return client?.Id;
        }
    }

}
