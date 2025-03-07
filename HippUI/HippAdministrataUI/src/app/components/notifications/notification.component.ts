import { Component, HostListener, OnInit } from '@angular/core';
import { NotificationService } from '../../../services/notification.service';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth-service.service';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css'],
  standalone: true, // Mark it as standalone
  imports: [CommonModule], // Import CommonModule
})

export class NotificationComponent implements OnInit {
  notifications: any[] = [];
  hasUnread: boolean = false;
  isOpen = false;
  userId!: number;
  unreadCount: number = 0;
  roleId: number = Number(localStorage.getItem('roleId'));
  showNotifications = false;
  constructor(private notificationService: NotificationService,private authService: AuthService,private cdRef: ChangeDetectorRef) {
    
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
  @HostListener('document:click', ['$event'])
  onClickOutside(event: Event) {
    const target = event.target as HTMLElement;
    if (!target.closest('.notification-wrapper')) {
      this.showNotifications = false;
    }
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
  
        // Check if there are unread notifications
        this.hasUnread = this.notifications.some(n => !n.isRead);  // Check if any notification is unread
        this.unreadCount = this.notifications.filter(n => !n.isRead).length;  // Count unread notifications
        
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
    this.showNotifications = !this.showNotifications;
    console.log("🔔 Bell Clicked! Show Notifications:", this.showNotifications);

    if (this.showNotifications) {
      console.log("📤 Sending request to mark all as read...");

      this.notificationService.markAllAsRead(this.roleId).subscribe({
        next: (response) => {
          console.log("✅ API Response:", response);
          if (response.success) {
            this.unreadCount = 0;
            this.hasUnread = false;  // ✅ Remove blue dot when notifications are read
            this.notifications.forEach(n => n.isRead = true);
          }
        },
        error: (error) => {
          console.error("❌ Error marking as read:", error);
        }
      });
    }
  }

}
  

