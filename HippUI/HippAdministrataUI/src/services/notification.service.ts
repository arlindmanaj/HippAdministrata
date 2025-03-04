import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  public hubConnection!: signalR.HubConnection;
  public notificationsSubject = new BehaviorSubject<any[]>([]);
  notifications$ = this.notificationsSubject.asObservable();
  userId!: number;

  private apiUrl = 'https://localhost:7136/api/notifications';

  constructor( private http: HttpClient) { 
    this.startConnection();
    this.addNotificationListener();
  }

  // private startConnection() {
  //   this.hubConnection = new signalR.HubConnectionBuilder()
  //   .withUrl("https://localhost:7136/notificationHub", {
  //     skipNegotiation: true, // Required if using WebSockets only
  //     transport: signalR.HttpTransportType.WebSockets
  //   })
  //   .withAutomaticReconnect()
  //   .build();
  
  //   this.hubConnection
  // .start()
  // .then(() => console.log("✅ SignalR Connected"))
  // .catch(err => console.error("❌ SignalR Connection Failed", err));

  // this.hubConnection.on("ReceiveNotification", (message) => {
  //   console.log("🔔 New Notification: ", message);
  // });
  // }
  private startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7136/notificationHub", {
        withCredentials: false, 
        skipNegotiation: true, 
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()
      .build();
  
    this.hubConnection.start()
      .then(() => {
        console.log("✅ SignalR Connected");
  
        // Manually invoke the new 'AddToGroup' method
        const roleId = Number(localStorage.getItem("roleId")); 
        if (roleId === 3) {
          this.hubConnection.invoke("AddToGroup", "Managers")
            .then(() => console.log("✅ Joined Managers Group"))
            .catch(err => console.error("❌ Failed to join Managers Group", err));
        } else if (roleId === 1) {
          this.hubConnection.invoke("AddToGroup", "Admins")
            .then(() => console.log("✅ Joined Admins Group"))
            .catch(err => console.error("❌ Failed to join Admins Group", err));
        }
      })
      .catch(err => console.error("❌ SignalR Connection Failed", err));
  
      this.hubConnection.on("ReceiveNotification", (notification) => {
        console.log("🔔 New Notification:", notification);
      
        // Make sure the notification object is properly structured
        if (!notification || !notification.message) {
          console.error("❌ Invalid notification received:", notification);
          return;
        }
      
        // Get current notifications
        const currentNotifications = this.notificationsSubject.getValue();
      
        // Add the new notification and update UI
        this.notificationsSubject.next([...currentNotifications, notification]);
      });
      
      
  }
  getNotificationUpdates() {
    return this.notifications$;
  }
  

  // private addNotificationListener() {
  //   this.hubConnection.on("ReceiveNotification", (message) => {
  //     console.log("New Notification:", message);
  //     alert(message); // Example: Show alert, but replace with proper UI updates
  //   });
  // }
  // private addNotificationListener() {
  //   this.hubConnection.on("ReceiveNotification", (message) => {
  //     console.log("🔔 New Notification:", message);
  //     const newNotification = { message, isRead: false, createdAt: new Date() };
  //     const currentNotifications = this.notificationsSubject.value;
  //     this.notificationsSubject.next([newNotification, ...currentNotifications]);
  //   });
  // }



  
  private addNotificationListener() {
    this.hubConnection.on("ReceiveNotification", (message) => {
      console.log("🔔 New Notification:", message);
  
      // Show alert for real-time notification
      alert(message);
  
      // Fetch notifications from the backend again to update the wrapper
      const roleId = Number(localStorage.getItem("roleId"));
      this.getRoleNotifications(roleId).subscribe((notifications) => {
        this.notificationsSubject.next(notifications); // Update the UI
      });
    });
  }
  
  

  
  sendNotification(message: string) {
    this.hubConnection.invoke("SendNotification", message)
      .catch(err => console.error("Error sending notification:", err));
  }






  
  markAllAsRead(roleId: number): Observable<any> {
    return this.http.post(`https://localhost:7136/api/notifications/mark-as-read/${roleId}`, {});
  }
  
  // }
  getRoleNotifications(roleId: number): Observable<Notification[]> {
    return this.http.get<Notification[]>(`https://localhost:7136/api/notifications/get-role-notifications/${roleId}`);
  }
  
  





}
