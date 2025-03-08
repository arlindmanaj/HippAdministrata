import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../../services/notification.service';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-real-time-notification',
  templateUrl: './real-time-notification.component.html',
  styleUrls: ['./real-time-notification.component.css'],
  imports: [CommonModule]
})
// export class RealTimeNotificationComponent implements OnInit {
//   realTimeNotification: any = null;

//   constructor(private notificationService: NotificationService) {}

//   ngOnInit(): void {
//     this.notificationService.realTimeNotification$.subscribe(notification => {
//       if (notification) {
//         this.realTimeNotification = notification;

//         // Auto-hide after 5 seconds
//         setTimeout(() => {
//           this.realTimeNotification = null;
//         }, 5000);
//       }
//     });
//   }
// }
export class RealTimeNotificationComponent implements OnInit {
  realTimeNotifications: any[] = []; // Store multiple notifications

  constructor(private notificationService: NotificationService) {}

  ngOnInit(): void {
    this.notificationService.realTimeNotification$.subscribe(notification => {
      if (notification) {
        this.realTimeNotifications.unshift(notification); // Add new notification at the top of the stack
        this.playNotificationSound();
        // Auto-hide after 5 seconds
        setTimeout(() => {
          this.closeNotification(notification);
        }, 5000);
      }
    });
  }

  playNotificationSound() {
    const isMuted = localStorage.getItem('muteNotifications') === 'true';
    if (isMuted) return;
  
    const audio = new Audio('https://cdn.pixabay.com/audio/2024/11/27/audio_e6b2e5efcc.mp3');

    audio.play().catch(error => console.error('Error playing sound:', error));
  }
  
  // Close individual notification
  closeNotification(notification: any) {
    this.realTimeNotifications = this.realTimeNotifications.filter(n => n !== notification);
  }
}