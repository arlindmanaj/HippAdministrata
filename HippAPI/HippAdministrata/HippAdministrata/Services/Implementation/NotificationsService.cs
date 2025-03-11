using System.Collections.Generic;
using System.Threading.Tasks;
using HippAdministrata.Hubs;
using HippAdministrata.Models.Domains;
using HippAdministrata.Repositories.Interfaces;
using HippAdministrata.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace HippAdministrata.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext; // Add this line

        public NotificationService(INotificationRepository notificationRepository, IHubContext<NotificationHub> hubContext)
        {
            _notificationRepository = notificationRepository;
            _hubContext = hubContext; // Assign it here
        }


        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task AddNotificationAsync(int userId, string message, string type)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                Type = type
            };

            await _notificationRepository.AddNotificationAsync(notification);
        }

        public async Task AddNotificationForRoleAsync(int roleId, string message, string type)
        {
            var notification = new Notification
            {
                RoleId = roleId,
                Message = message,
                Type = type,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await _notificationRepository.AddNotificationAsync(notification);

            var groups = new Dictionary<int, string>
            {
                { 1, "Admins" },
                { 3, "Managers" }
            };

            if (groups.ContainsKey(roleId))
            {
                await _hubContext.Clients.Group(groups[roleId]).SendAsync("ReceiveNotification", message);
                Console.WriteLine($"🔔 Notification Sent to {groups[roleId]}");
            }
        }

        public async Task<List<Notification>> GetRoleNotificationsAsync(int roleId)
        {
            return await _notificationRepository.GetRoleNotificationsAsync(roleId);
        }
    }
}
