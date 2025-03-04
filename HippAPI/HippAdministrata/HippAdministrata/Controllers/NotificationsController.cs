using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using HippAdministrata.Hubs; // Make sure this matches your namespace
using HippAdministrata.Repositories.Interfaces;
using HippAdministrata.Models.Domains;
using Microsoft.EntityFrameworkCore;
using HippAdministrata.Data;
[Route("api/notifications")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly INotificationRepository _notificationRepository;
    private readonly ApplicationDbContext _context;

    public NotificationsController(IHubContext<NotificationHub> hubContext, INotificationRepository notificationRepository, ApplicationDbContext context)
    {
        _hubContext = hubContext;
        _notificationRepository = notificationRepository;
        _context = context;
    }

    //[HttpPost("send")]
    //public async Task<IActionResult> SendNotification([FromBody] NotificationDto notification)
    //{
    //    if (notification == null || string.IsNullOrEmpty(notification.Message))
    //    {
    //        return BadRequest("Message is required.");
    //    }

    //    // Send notification to all connected clients
    //    await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification.Message);

    //    return Ok(new { Message = "Notification sent successfully!" });
    //}
    [HttpPost("send")]
    public async Task<IActionResult> SendNotification(NotificationDto notificationDto)
    {
        var notification = new Notification
        {
            Message = notificationDto.Message,
            Type = notificationDto.Type,
            RoleId = notificationDto.RoleId, // Assign RoleId
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        await _notificationRepository.AddNotificationAsync(notification);

        // Send real-time notification using SignalR
        await _hubContext.Clients.Group($"Role_{notificationDto.RoleId}")
            .SendAsync("ReceiveNotification", notificationDto.Message);

        return Ok("Notification sent successfully.");
    }


    //[HttpGet("get-user-notifications/{userId}")]
    //public async Task<IActionResult> GetUserNotifications(int userId)
    //{
    //    var notifications = await _notificationRepository.GetUserNotificationsAsync(userId);
    //    return Ok(notifications);
    //}
    [HttpGet("get-role-notifications/{roleId}")]
    public async Task<IActionResult> GetRoleNotifications(int roleId)
    {
        var notifications = await _notificationRepository.GetRoleNotificationsAsync(roleId);
        return Ok(notifications);
    }

    [HttpPost("mark-as-read/{roleId}")]
    public async Task<IActionResult> MarkAllAsRead(int roleId)
    {
        var notifications = _context.Notifications
            .Where(n => n.RoleId == roleId && !n.IsRead)
            .ToList();

        if (!notifications.Any())
            return Ok("No unread notifications.");

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }

        await _context.SaveChangesAsync();
        return Ok("Notifications marked as read.");
    }





}

// DTO for receiving notification data
public class NotificationDto
{
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // 🔹 Add this field
    public int? UserId { get; set; } // If sending to a specific user
    public int? RoleId { get; set; } // 🔹 Add this field to send to a role
}
