// manager-dashboard.component.ts
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-manager-dashboard',
  templateUrl: './manager-dashboard.component.html',
  styleUrls: ['./manager-dashboard.component.css']
})
export class ManagerDashboardComponent {
  constructor(private router: Router) { }

  navigateToProducts(): void {
    this.router.navigate(['/manager/products']);
  }

  logout(): void {
    localStorage.removeItem('authToken');
    this.router.navigate(['/login']);
  }
  navigateToOrders(): void {
    this.router.navigate(['/manager/orders']);
  }
}
