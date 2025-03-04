using System.Collections.Generic;
using System.Threading.Tasks;
using HippAdministrata.Models.Domains;

namespace HippAdministrata.Services.Interfaces
{
    public interface INotificationService
    {
        Task AddNotificationAsync(int userId, string message, string type);
        //Task<List<Notification>> GetUserNotificationsAsync(int userId);
        Task<List<Notification>> GetRoleNotificationsAsync(int roleId);
        Task AddNotificationForRoleAsync(int roleId, string message, string type);


    }
}
