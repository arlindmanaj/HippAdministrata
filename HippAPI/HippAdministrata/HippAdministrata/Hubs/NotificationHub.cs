using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using HippAdministrata.Data;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HippAdministrata.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly ApplicationDbContext _dbContext;

        public NotificationHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendNotification(string message)
        {
            await Clients.Group("Admins").SendAsync("ReceiveNotification", message);
        }

        public override async Task OnConnectedAsync()
        {
            var user = Context.User;
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = Context.User?.FindFirst(ClaimTypes.Role)?.Value;

            Console.WriteLine($"🔍 User Connected: {userId}, Role: {role}");

            if (role == "Admin")
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
                Console.WriteLine($"✅ Admin {userId} added to 'Admins' group.");
            }
            else
            {
                Console.WriteLine($"🚫 User {userId} is NOT an Admin.");
            }

            await base.OnConnectedAsync();
        }
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            Console.WriteLine($"✅ {Context.ConnectionId} joined {groupName}");
        }



        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Admins");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
