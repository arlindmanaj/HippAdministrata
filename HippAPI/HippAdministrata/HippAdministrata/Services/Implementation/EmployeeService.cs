using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interface;

namespace HippAdministrata.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee> GetByIdAsync(int id)=> await _employeeRepository.GetByIdAsync(id);
        public async Task <IEnumerable<Employee>> GetAllAsync()=> await _employeeRepository.GetAllAsync();

        public async Task <bool> UpdateAsync(int id, Employee updatedEmployee)=> await _employeeRepository.UpdateAsync(id, updatedEmployee);    
        public async Task <bool> DeleteAsync(int id) => await _employeeRepository.DeleteAsync(id);
    
    }
}
