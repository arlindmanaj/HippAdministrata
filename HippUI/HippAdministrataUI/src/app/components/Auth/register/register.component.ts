import { Component } from '@angular/core';
import { AuthService } from '../../../../services/auth-service.service';
import { FormsModule } from '@angular/forms'
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  standalone: true,
  styleUrls: ['./register.component.css'],
  imports: [FormsModule, CommonModule]
})
export class RegisterComponent {
  name: string = '';
  password: string = '';
  email: string = '';
  successMessage: string = '';
  errorMessage: string = '';

  constructor(private authService: AuthService) { }

  register(): void {
    this.authService.registerUser(this.name, this.password, this.email).subscribe(
      () => {
        this.successMessage = 'Registration successful!';
        this.name = '';
        this.password = '';
        this.email = '';
      },
      (error) => {
        this.errorMessage = 'Registration failed. Please try again.';
      }
    );
  }
}
