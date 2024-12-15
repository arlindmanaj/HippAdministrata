using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interface;

namespace HippAdministrata.Services
{
    public class ManagerService
    {
        private readonly IManagerRepository _managerRepository;

        public ManagerService(IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }

        public async Task<Manager> GetByIdAsync(int id) => await _managerRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Manager>> GetAllAsync() => await _managerRepository.GetAllAsync();

        public async Task<bool> UpdateAsync(int id, Manager updatedManager) => await _managerRepository.UpdateAsync(id, updatedManager);

        public async Task<bool> DeleteAsync(int id) => await _managerRepository.DeleteAsync(id);
    }
}
