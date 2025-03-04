using System;

namespace HippAdministrata.Models.Domains
{
    public class Notification
    {
        public int Id { get; set; } // Primary Key
        public int? UserId { get; set; } // Foreign Key to User
        public int? RoleId { get; set; }
        public User? User { get; set; }

        public string Message { get; set; } // Notification Content
        public string Type { get; set; } // e.g., "Order Update", "New Assignment"

        public bool IsRead { get; set; } = false; // Track Read/Unread Status
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp
    }
}
