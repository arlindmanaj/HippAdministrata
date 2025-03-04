using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HippAdministrata.Repositories.Implementation
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context; // Correct field declaration

        public NotificationRepository(ApplicationDbContext context) // Correct parameter name
        {
            _context = context; // Assign parameter to the private field
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Notification>> GetRoleNotificationsAsync(int roleId)
        {
            return await _context.Notifications
                .Where(n => n.RoleId == roleId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }






        //public async Task<List<Notification>> GetUserNotificationsAsync(int userId)
        //{
        //    return await _context.Notifications
        //        .Where(n => n.UserId == userId)
        //        .OrderByDescending(n => n.CreatedAt)
        //        .ToListAsync();
        //}
        //public async Task MarkNotificationsAsReadAsync(int userId)
        //{
        //    var notifications = await _context.Notifications
        //                                      .Where(n => n.UserId == userId && !n.IsRead)
        //                                      .ToListAsync();

        //    if (notifications.Any())
        //    {
        //        foreach (var notification in notifications)
        //        {
        //            notification.IsRead = true;
        //        }

        //        await _context.SaveChangesAsync();
        //    }
        //}

    }

}
