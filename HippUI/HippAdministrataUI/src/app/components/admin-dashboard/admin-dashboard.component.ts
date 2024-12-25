import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth-service.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  standalone: true,
  styleUrls: ['./admin-dashboard.component.css'],
  imports: [FormsModule, CommonModule], // Add FormsModule and CommonModule if needed for template bindings
})
export class AdminDashboardComponent implements OnInit {
  users: any[] = [];
  newUser = { name: '', password: '', email: '' };
  newUserRole = ''; // Role selected by the admin
  newDriverDetails = { licensePlate: '', carModel: '' };


  errorMessage = '';
  successMessage = '';


  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  // Load all users
  loadUsers(): void {
    this.authService.getUsers().subscribe(
      (data) => {
        this.users = data;
      },
      (error) => {
        console.error('Failed to load users:', error);
        this.errorMessage = 'Failed to load users. Please try again later.';
      }
    );
  }

  addUser(): void {
    const { name, password, email } = this.newUser;

    if (!name || !password || !this.newUserRole) {
      this.errorMessage = 'Name, password, and role are required.';
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


        console.log('Registering SalesPerson with data:', { name, password });

        this.authService.registerSalesPerson(name, password).subscribe(
          () => this.handleSuccess('SalesPerson registered successfully.'),
          (error) => {
            console.error('Failed to register salesperson:', error);
            this.handleError('Failed to register salesperson.', error);
          }
        );
        break;

      case 'Employee':
        this.authService.registerEmployee(name, password).subscribe(
          () => this.handleSuccess('Employee registered successfully.'),
          (error) => this.handleError('Failed to register employee.', error)
        );
        break;

      case 'Manager':
        this.authService.registerManager(name, password).subscribe(
          () => this.handleSuccess('Manager registered successfully.'),
          (error) => this.handleError('Failed to register manager.', error)
        );
        break;

      default:
        this.errorMessage = 'Invalid role selected.';
        break;
    }
  }



  // Handle successful user addition
  private handleSuccess(message: string): void {
    this.successMessage = message;
    this.errorMessage = '';
    this.newUser = { name: '', password: '', email: '' }; // Reset user form

    this.newDriverDetails = { licensePlate: '', carModel: '' }; // Reset driver details

    this.loadUsers(); // Refresh the user list
  }

  // Handle errors
  private handleError(message: string, error: any): void {
    console.error(message, error);
    this.errorMessage = message;
  }

  // Delete a user
  deleteUser(userId: number): void {
    this.authService.deleteUser(userId).subscribe(
      () => {
        this.users = this.users.filter((user) => user.userId !== userId); // Remove user from the list
        this.successMessage = 'User deleted successfully.';
      },
      (error) => {
        console.error('Failed to delete user:', error);
        this.errorMessage = 'Failed to delete user. Please try again later.';
      }
    );
  }
}
