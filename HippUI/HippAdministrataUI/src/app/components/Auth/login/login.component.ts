import { Component } from '@angular/core';
import { AuthService } from '../../../../services/auth-service.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  standalone: true,
  styleUrls: ['./login.component.css'],
  imports: [FormsModule, CommonModule]
})
export class LoginComponent {
  name: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  // Login function
  login(): void {
    // Call the authentication service's login function
    this.authService.login(this.name, this.password).subscribe(
      (response) => {
        const token = response.token.token; // Extract the actual token string
        const { role, roleSpecificId } = response;

        // Save the token, role, and role-specific ID in localStorage
        localStorage.setItem('authToken', token);
        localStorage.setItem('role', role);
        localStorage.setItem('roleSpecificId', roleSpecificId?.toString() || '');

        // Redirect user based on role
        switch (role) {
          case 'Client':
            this.router.navigate(['/client-dashboard']);
            break;
          case 'Admin':
            this.router.navigate(['/admin-dashboard']);
            break;
          case 'SalesPerson':
            this.router.navigate(['/salesperson-dashboard']);
            break;
          case 'Manager':
            this.router.navigate(['/manager-dashboard']);
            break;
          case 'Employee':
            this.router.navigate(['/employee-dashboard']);
            break;
          case 'Driver':
            this.router.navigate(['/driver-dashboard']);
            break;
          default:
            this.errorMessage = 'Unauthorized access.';
        }
      },
      (error) => {
        // Handle login failure
        this.errorMessage = 'Invalid username or password. Please try again.';
      }
    );
  }
}
