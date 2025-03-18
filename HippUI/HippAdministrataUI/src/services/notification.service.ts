import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { Observable } from 'rxjs';
import { RealTimeNotificationComponent } from '../app/components/real-time-notification/real-time-notification.component';
@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  public hubConnection!: signalR.HubConnection;
  public notificationsSubject = new BehaviorSubject<any[]>([]);
  notifications$ = this.notificationsSubject.asObservable();
  userId!: number;
  private notificationComponent!: RealTimeNotificationComponent;

  private apiUrl = 'https://localhost:7136/api/notifications';

  constructor( private http: HttpClient) { 
    this.startConnection();
    this.addNotificationListener();
  }
  setNotificationComponent(component: RealTimeNotificationComponent) {
    this.notificationComponent = component;
  }

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
        else if (roleId === 6) {
          this.hubConnection.invoke("AddToGroup", "Clients")
            .then(() => console.log("✅ Joined Clients Group"))
            .catch(err => console.error("❌ Failed to join Clients Group", err));
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
  

    // Add a new subject for real-time notifications (separate from the wrapper)
  private realTimeNotificationSubject = new BehaviorSubject<any | null>(null);
  realTimeNotification$ = this.realTimeNotificationSubject.asObservable();

  private addNotificationListener() {
    this.hubConnection.on("ReceiveNotification", (message) => {
      console.log("🔔 New Notification:", message);

      // Send only the latest notification to the real-time notification observable
      this.realTimeNotificationSubject.next({ message, timestamp: new Date() });

      // Wrapper logic stays the same, untouched
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
