import { Component, OnInit, ViewChild } from '@angular/core';
import { DriverService } from '../../../services/driver.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { OrderStatus } from '../../../models/OrderStatus';
import { getOrderStatusLabel } from '../../../services/order-status.util';
import { SettingsModalComponent } from '../settings-modal/settings-modal.component';
@Component({
  selector: 'app-driver-dashboard',
  templateUrl: './driver-dashboard.component.html',
  standalone: true,
  styleUrls: ['./driver-dashboard.component.css'],
  imports: [CommonModule, FormsModule,SettingsModalComponent],
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

  // loadDriverAssignedOrders(driverId: number): void {
  //   this.driverService.getDriverAssignedOrders(driverId).subscribe(
  //     (orders) => {
  //       const formattedOrders = orders.map((order) => ({
  //         ...order,
  //         orderStatusDisplay: order.orderStatusDescription, // Use utility function
  //         productName: order.productName,
  //       }));
  //       this.assignedOrders = formattedOrders;
  //     },
  //     (error) => {
  //       console.error('Failed to load assigned orders:', error);
  //       alert('Failed to load assigned orders.');
  //     }
  //   );
  // }
  loadDriverAssignedOrders(driverId: number): void {
    this.driverService.getDriverAssignedOrders(driverId).subscribe(
      (orders) => {
        const formattedOrders = orders.map((order) => ({
          ...order,
          orderStatusDisplay: order.orderStatusDescription, // Use utility function
          productName: order.productName,
        }));
  
        // Sort the orders: orders with orderStatusId === 5 will be moved to the bottom
        this.assignedOrders = formattedOrders.sort((a, b) => {
          if (a.orderStatusId === 5 && b.orderStatusId !== 5) {
            return 1;  // Move 'a' (order with status 5) to the bottom
          }
          if (a.orderStatusId !== 5 && b.orderStatusId === 5) {
            return -1; // Keep orders without status 5 at the top
          }
          return 0; // No change in order if both or neither have status 5
        });
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
        this.loadDriverAssignedOrders(driverId); // Refresh orders after simulation
      },
      (error) => {
        console.error('Failed to simulate shipping:', error);
        
      }
    );
  }
  // isShipping: boolean = false; // New variable to control animation

// simulateShipping(orderId: number): void {
//   this.isShipping = true; // Show truck animation

//   // Simulate the shipping process
//   setTimeout(() => {
//     const driverId = this.driverId;
//     this.driverService.simulateShipping(driverId, orderId).subscribe(
//       (response) => {
//         alert(response.message);
        
//         // Update the order status immediately in the UI
//         const order = this.assignedOrders.find(o => o.id === orderId);
//         if (order) {
//           order.orderStatusDescription = 'Shipped'; // Or update with the appropriate status
//           order.orderStatusId = 4; // Assuming 4 represents 'Shipped' status in your system
//         }

//         this.isShipping = false; // Hide animation after request is complete
//       },
//       (error) => {
//         console.error('Failed to simulate shipping:', error);
//         this.isShipping = false; // Ensure animation stops on error
//       }
//     );
//   }); // Delay API call to sync with animation duration
// }

  
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
    localStorage.clear();
    this.router.navigate(['/login']);
    
  }
  toggleSidebar(): void {
    this.sidebarCollapsed = !this.sidebarCollapsed;
  }
  @ViewChild('settingsModal') settingsModal!: SettingsModalComponent;

  openSettingsModal() {
    this.settingsModal.toggleModal();
  }
}

