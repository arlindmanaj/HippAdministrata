using System.Collections.Generic;
using System.Threading.Tasks;
using HippAdministrata.Models.Domains;

namespace HippAdministrata.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task AddNotificationAsync(Notification notification);
        //Task<List<Notification>> GetUserNotificationsAsync(int userId);
        //Task MarkNotificationsAsReadAsync(int userId);
        Task<List<Notification>> GetRoleNotificationsAsync(int roleId);

    }
}
