using HippAdministrata.Models.Domains;

namespace HippAdministrata.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByUsernameAsync(string username);
        Task<List<Client>> GetAllClientsAsync();
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<List<Driver>> GetAllDriversAsync();
        Task<List<SalesPerson>> GetAllSalesPersonsAsync();

        Task AddUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}
