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

  constructor(private authService: AuthService, private router: Router) { }

  login(): void {
    this.authService.login(this.name, this.password).subscribe(
      (response) => {
        const token = response.token.token; // Extract the actual token string
        const { role, roleSpecificId } = response;

        // Save the token, role, and role-specific ID in localStorage
        localStorage.setItem('authToken', token);
        localStorage.setItem('role', role);
        localStorage.setItem('roleSpecificId', roleSpecificId?.toString() || '');

        // Redirect based on role
        if (role === 'Client') {
          this.router.navigate(['/client-dashboard']);
        } else if (role === 'Admin') {
          this.router.navigate(['/admin-dashboard']);
        } else if (role === 'SalesPerson') {
          this.router.navigate(['/salesperson-dashboard']);
        } else if (role === 'Manager') {
          this.router.navigate(['/manager-dashboard']);
        } else if (role === 'Employee') {
          this.router.navigate(['/employee-dashboard']);
        } else if (role === 'Driver') {
          this.router.navigate(['/driver-dashboard']);
        } else {
          this.errorMessage = 'Unauthorized access.';
        }
      },
      (error) => {
        this.errorMessage = 'Invalid username or password. Please try again.';
      }
    );
  }

}
