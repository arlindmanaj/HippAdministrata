
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth-service.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { NotificationService } from '../../../services/notification.service';
import { NotificationComponent } from '../notifications/notification.component';
import { RealTimeNotificationComponent } from '../real-time-notification/real-time-notification.component';


@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule,NotificationComponent,RealTimeNotificationComponent], // Import FormsModule and CommonModule
})

export class AdminDashboardComponent implements OnInit {
  users: any[] = [];
  newUser = { name: '', password: '', email: '' };
  newUserRole = ''; // Role selected by the admin
  newDriverDetails = { licensePlate: '', carModel: '' };
  newClientDetails = { phone: '', address: '' };
  filteredUsers: any[] = [];
  selectedRole: string = '';
  uniqueRoles: string[] = [];
  sidebarCollapsed: boolean = false;

  errorMessage = '';
  successMessage = '';
  activeSection = 'registration'; // Default section
  isSettingsModalOpen = false;
  muteNotifications = false;

  constructor(private authService: AuthService, private router: Router,private notificationService: NotificationService) {}

  ngOnInit(): void {
    this.loadUsers();
    const savedMuteSetting = localStorage.getItem('muteNotifications');
    this.muteNotifications = savedMuteSetting === 'true';
  }

  // Switch sections
  viewSection(section: string): void {
    this.activeSection = section;
  }

  // Load all users
  loadUsers(): void {
    this.authService.getUsers().subscribe(
      (data) => {
        this.users = data;
        this.filteredUsers = [...this.users];
        this.extractUniqueRoles();
      },
      (error) => {
        console.error('Failed to load users:', error);
        this.errorMessage = 'Failed to load users. Please try again later.';
      }
    );
  }
  sendTestNotification() {
    this.notificationService.sendNotification("🚀 Admin Test Notification!");
  }
  extractUniqueRoles(): void {
    this.uniqueRoles = Array.from(new Set(this.users.map((user) => user.role)));
  }

  filterUsersByRole(): void {
    if (this.selectedRole) {
      this.filteredUsers = this.users.filter((user) => user.role === this.selectedRole);
    } else {
      this.filteredUsers = [...this.users]; // Reset to show all users
    }
  }

  // Add user
  addUser(): void {
    const { name, password, email } = this.newUser;

    if (!name || !password || !this.newUserRole) {
      this.errorMessage = 'Name, password, and role are required.';
      return;
    }

    if (['SalesPerson', 'Manager'].includes(this.newUserRole) && !email) {
      this.errorMessage = 'Email is required for this role.';
      return;
    }

    switch (this.newUserRole) {
      case 'Driver':
        const { licensePlate, carModel } = this.newDriverDetails;
        if (!licensePlate || !carModel) {
          this.errorMessage = 'License plate and car model are required for drivers.';
          return;
        }
        this.authService.registerDriver(name, password, email, licensePlate, carModel).subscribe(
          () => this.handleSuccess('Driver registered successfully.'),
          (error) => this.handleError('Failed to register driver.', error)
        );
        break;
      case 'SalesPerson':
        this.authService.registerSalesPerson(name, password, email).subscribe(
          () => this.handleSuccess('SalesPerson registered successfully.'),
          (error) => this.handleError('Failed to register SalesPerson.', error)
        );
        break;
      case 'Manager':
        this.authService.registerManager(name, password, email).subscribe(
          () => this.handleSuccess('Manager registered successfully.'),
          (error) => this.handleError('Failed to register Manager.', error)
        );
        break;
      case 'Employee':
        this.authService.registerEmployee(name, password, email).subscribe(
          () => this.handleSuccess('Employee registered successfully.'),
          (error) => this.handleError('Failed to register employee.', error)
        );
        break;
      case 'Client':
          const { phone, address } = this.newClientDetails;
          if (!phone || !address) {
            this.errorMessage = 'Phone and address are required for clients.';
            return;
          }
          this.authService.registerClient(name, email, password, phone, address).subscribe(
            () => this.handleSuccess('Client registered successfully.'),
            (error) => this.handleError('Failed to register client.', error)
          );
          break;
        
      default:
        this.errorMessage = 'Invalid role selected.';
    }
  }

  private handleSuccess(message: string): void {
    this.successMessage = message;
    this.errorMessage = '';
    this.newUser = { name: '', password: '', email: '' };
    this.newDriverDetails = { licensePlate: '', carModel: '' };
    this.loadUsers();
  }

  private handleError(message: string, error: any): void {
    console.error(message, error);
    this.errorMessage = message;
  }

  deleteUser(userId: number): void {
    this.authService.deleteUser(userId).subscribe(
      () => {
        this.users = this.users.filter((user) => user.userId !== userId);
        this.successMessage = 'User deleted successfully.';
      },
      (error) => {
        console.error('Failed to delete user:', error);
        this.errorMessage = 'Failed to delete user. Please try again later.';
      }
    );
  }

  logout(): void {
    
    localStorage.removeItem('authToken');
    this.router.navigate(['/login']);
    localStorage.clear();
  }
  toggleSidebar(): void {
    this.sidebarCollapsed = !this.sidebarCollapsed;
  }
  openSettingsModal() {
    this.isSettingsModalOpen = true;
  }
  
  closeSettingsModal() {
    this.isSettingsModalOpen = false;
  }
  
  // Toggle mute notifications
  toggleMuteNotifications() {
    localStorage.setItem('muteNotifications', String(this.muteNotifications));
  }

}
