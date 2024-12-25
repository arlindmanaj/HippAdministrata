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
    console.log('Attempting to log in with:', { name: this.name, password: this.password });

    this.authService.login(this.name, this.password).subscribe(
      (response) => {
        console.log('Login successful, response:', response);
        const { token, role } = response;

        // Save token
        this.authService.saveToken(token);

        // Redirect based on role
        if (role === 'Admin') {
          this.router.navigate(['/admin-dashboard']);
        } else {
          this.errorMessage = 'Unauthorized access';
        }
      },
      (error) => {
        console.error('Login failed, error:', error);
        this.errorMessage = 'Invalid username or password. Please try again.';
      }
    );
  }



}

