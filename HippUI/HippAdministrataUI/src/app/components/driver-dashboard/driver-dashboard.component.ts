import { Component, OnInit } from '@angular/core';
import { DriverService } from '../../../services/driver.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { OrderStatus } from '../../../models/OrderStatus';
import { getOrderStatusLabel } from '../../../services/order-status.util';
@Component({
  selector: 'app-driver-dashboard',
  templateUrl: './driver-dashboard.component.html',
  standalone: true,
  styleUrls: ['./driver-dashboard.component.css'],
  imports: [CommonModule, FormsModule],
})
export class DriverDashboardComponent implements OnInit {
  assignedOrders: any[] = [];
  sidebarCollapsed: boolean = false;
  driverId: number = Number(localStorage.getItem('roleSpecificId')); // Get driver ID from localStorage
  transferData: { productId: number; sourceWarehouseId: number; destinationWarehouseId: number } = {
    productId: 0,
    sourceWarehouseId: 0,
    destinationWarehouseId: 0,
  };
  activeSection: string = 'assignedOrders'; // Default section

  constructor(private driverService: DriverService, private router: Router) {}

  ngOnInit(): void {
    this.loadDriverAssignedOrders(this.driverId);
  }

  showSection(section: string): void {
    this.activeSection = section;
  }

  loadDriverAssignedOrders(driverId: number): void {
    this.driverService.getDriverAssignedOrders(driverId).subscribe(
      (orders) => {
        const formattedOrders = orders.map((order) => ({
          ...order,
          orderStatusDisplay: order.orderStatusDescription, // Use utility function
          productName: order.productName,
        }));
        this.assignedOrders = formattedOrders;
      },
      (error) => {
        console.error('Failed to load assigned orders:', error);
        alert('Failed to load assigned orders.');
      }
    );
  }

  simulateShipping(orderId: number): void {
    const driverId = this.driverId; // Get driverId from the logged-in driver
    this.driverService.simulateShipping(driverId, orderId).subscribe(
      (response) => {
        alert(response.message);
        this.loadDriverAssignedOrders(driverId); // Refresh orders after simulation
      },
      (error) => {
        console.error('Failed to simulate shipping:', error);
        
      }
    );
  }

  transferProduct(): void {
    const { productId, sourceWarehouseId, destinationWarehouseId } = this.transferData;

    if (!productId || !sourceWarehouseId || !destinationWarehouseId) {
      alert('Please fill in all transfer fields.');
      return;
    }

    this.driverService.transferProduct(productId, sourceWarehouseId, destinationWarehouseId).subscribe(
      (response) => {
        alert(response); // Show the success message from the API
        this.transferData = { productId: 0, sourceWarehouseId: 0, destinationWarehouseId: 0 }; // Reset transfer data
      },
      (error) => {
        console.error('Failed to transfer product:', error);
        if (error.error) {
          alert(`Failed to transfer product: ${error.error}`); // Show backend error message if available
        } else {
          alert('An unknown error occurred while transferring the product.');
        }
      }
    );
  }

  logout(): void {
    localStorage.removeItem('authToken');
    this.router.navigate(['/login']);
  }
  toggleSidebar(): void {
    this.sidebarCollapsed = !this.sidebarCollapsed;
  }
}

