import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-manager-dashboard',
  templateUrl: './manager-dashboard.component.html',
  styleUrls: ['./manager-dashboard.component.css'],
  standalone: true,
  imports: [RouterModule]
})
export class ManagerDashboardComponent {
  constructor(private router: Router) {}

  // Method to navigate to specific paths based on dashboard
  navigateTo(path: string): void {
    this.router.navigate([`/manager/${path}`]);  // Dynamic path for 'products' or 'orders'
  }

  // Method for logout
  logout(): void {
    localStorage.removeItem('authToken');
    this.router.navigate(['/login']); // Navigate to login page
  }
}
