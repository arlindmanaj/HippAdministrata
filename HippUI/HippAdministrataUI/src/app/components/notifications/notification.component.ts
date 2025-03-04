import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../../services/notification.service';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth-service.service';
@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css'],
  standalone: true, // Mark it as standalone
  imports: [CommonModule], // Import CommonModule
})

export class NotificationComponent implements OnInit {
  notifications: any[] = [];
  hasUnread = false;
  isOpen = false;
  userId!: number;
  unreadCount: number = 0;
  roleId: number = Number(localStorage.getItem('roleId'));
  showNotifications = false;
  constructor(private notificationService: NotificationService,private authService: AuthService) {
    
  }

  ngOnInit(): void {
    this.getRoleIdFromLocalStorage();
    this.loadNotifications();
    
    // this.notificationService.notifications$.subscribe(notifications => {
    //   this.notifications = notifications;
    //   this.hasUnread = notifications.some(n => !n.isRead);
    // });
    // this.loadNotifications();
    this.notificationService.getNotificationUpdates().subscribe((newNotifications) => {
      console.log("🔄 Real-Time Notification Update:", newNotifications);
      this.notifications = newNotifications;
      this.hasUnread = newNotifications.some(n => !n.isRead);
    });
    
  }

  getRoleIdFromLocalStorage() {
    const storedRoleId = localStorage.getItem("roleId");
    if (storedRoleId) {
      this.roleId = parseInt(storedRoleId, 10);
    }
  }

  loadNotifications() {
    this.notificationService.getRoleNotifications(this.roleId).subscribe(
      (data) => {
        this.notifications = data;
      },
      (error) => {
        console.error('Error fetching notifications:', error);
      }
    );
  }

  markAsRead(notification: any) {
    notification.isRead = true;
    this.unreadCount--;
  }

  getUserId() {
    const payload = this.authService.decodeToken();
    console.log("Decoded Token Payload:", payload); // Debugging
  
    if (payload && payload.id) {  // ✅ Use 'id' instead of 'roleSpecificId'
      this.userId = payload.id;
      console.log("User ID Retrieved:", this.userId);
      this.loadNotifications();
    } else {
      console.error("❌ User ID not found in token! Ensure you are logged in.");
    }
  }
  listenForRealTimeNotifications() {
    this.notificationService.getNotificationUpdates().subscribe((newNotification) => {
      console.log("🔔 New Notification Received:", newNotification);
      this.notifications.unshift(newNotification); // ✅ Add to the list
      this.hasUnread = true; // ✅ Mark as unread
    });
  }
  
  notificationBellClicked() {
    const roleId = Number(localStorage.getItem("roleId"));
  
    this.notificationService.markAllAsRead(roleId).subscribe(() => {
      // Update notifications locally to reflect the change
      this.notifications.forEach(n => n.isRead = true);
  
      // Clear the unread count
      this.unreadCount = 0;
    });
  
    // Toggle the dropdown
    this.showNotifications = !this.showNotifications;
  }
  
  
}
