using HippAdministrata.Models.Domains;

namespace HippAdministrata.Repositories.Interface
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByIdAsync(int id);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<bool> UpdateAsync(int id, Employee updatedEmployee);
        Task<bool> DeleteAsync(int id);
    }
}
