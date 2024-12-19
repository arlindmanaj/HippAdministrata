using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interface;

namespace HippAdministrata.Services
{
    public class SalesPersonService
    {
        private readonly ISalesPersonRepository _salesPersonRepository;

        public SalesPersonService(ISalesPersonRepository salesPersonRepository)
        {
            _salesPersonRepository = salesPersonRepository;
        }

        public async Task<SalesPerson> GetByIdAsync(int id) => await _salesPersonRepository.GetByIdAsync(id);
        public async Task<IEnumerable<SalesPerson>> GetAllAsync() => await _salesPersonRepository.GetAllAsync();

        public async Task<bool> UpdateAsync(int id, SalesPerson updatedSalesPerson) => await _salesPersonRepository.UpdateAsync(id, updatedSalesPerson);
        public async Task<bool> DeleteAsync(int id) => await _salesPersonRepository.DeleteAsync(id);

    }
}
