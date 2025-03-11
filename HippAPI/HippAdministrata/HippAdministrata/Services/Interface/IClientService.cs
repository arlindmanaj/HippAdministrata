namespace HippAdministrata.Services.Interface
{
    public interface IClientService
    {
        Task<int?> GetClientIdByUserIdAsync(int userId);
    }

}
