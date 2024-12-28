import { Component, OnInit } from '@angular/core';
import { SalesPersonService } from '../../../services/salesperson.service';
import { UserService } from '../../../services/user.service';
import { OrderStatus } from '../../../models/OrderStatus';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { getOrderStatusLabel } from '../../../services/order-status.util';

@Component({
  selector: 'app-salesperson-dashboard',
  templateUrl: './salesperson-dashboard.component.html',
  standalone: true,
  styleUrls: ['./salesperson-dashboard.component.css'],
  
  imports: [CommonModule, FormsModule]
})
export class SalespersonDashboardComponent implements OnInit {
  orders: any[] = [];
  employees: any[] = [];
  drivers: any[] = [];
  warehouses: any[] = [];
  assignment = { employeeId: null, driverId: null, warehouseId: null };
  selectedOrder: any = null;
  isModalOpen = false;  
  activeSection: string = 'orders'; // Default active section

  constructor(
    private userService: UserService,
    private salesPersonService: SalesPersonService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadSalesPersonOrders();
    this.loadUserData('employees');
    this.loadUserData('drivers');
    this.loadUserData('warehouses');
  }

  loadSalesPersonOrders(): void {
    const salesPersonId = Number(localStorage.getItem('roleSpecificId'));
    if (!salesPersonId) {
      alert('SalesPerson ID not found. Please log in again.');
      return;
    }

    this.salesPersonService.getOrdersBySalesPersonId(salesPersonId).subscribe(
      (orders) => (this.orders = orders),
      (error) => {
        console.error('Failed to load sales person orders:', error);
        alert('Failed to load orders. Please try again.');
      }
    );
  }
  getOrderStatusLabel(status: number | string): string {
    // If the status is already a string, return it
    if (typeof status === 'string') {
      return status;
    }

    // Convert numeric status to the corresponding string value
    return OrderStatus[status] || 'Unknown';
  }

  loadUserData(type: string): void {
    let endpoint;
    switch (type) {
      case 'employees':
        endpoint = this.userService.getAllEmployees();
        break;
      case 'drivers':
        endpoint = this.userService.getAllDrivers();
        break;
      case 'warehouses':
        endpoint = this.userService.getAllWarehouses();
        break;
      default:
        return;
    }

    endpoint.subscribe(
      (data) => {
        if (type === 'employees') this.employees = data;
        if (type === 'drivers') this.drivers = data;
        if (type === 'warehouses') this.warehouses = data;
      },
      (error) => console.error(`Failed to load ${type}:`, error)
    );
  }

    // Open the details modal
    openDetailsModal(order: any): void {
      this.selectedOrder = order;
      this.assignment = {
        employeeId: order.assignment?.employeeId || null,
        driverId: order.assignment?.driverId || null,
        warehouseId: order.assignment?.warehouseId || null,
      };
      this.isModalOpen = true;  // Open the modal
    }
  
    // Close the modal
    closeModal(): void {
      this.isModalOpen = false;  // Close the modal
      this.selectedOrder = null;
    }

  saveAssignment(): void {
    if (!this.assignment.employeeId || !this.assignment.driverId || !this.assignment.warehouseId) {
      alert('Please select Employee, Driver, and Warehouse.');
      return;
    }

    const isCreated = this.selectedOrder.orderStatus === 0; // 0 is "Created"
    const apiCall = isCreated
      ? this.salesPersonService.assignOrder(this.selectedOrder.id, this.assignment)
      : this.salesPersonService.updateOrder(this.selectedOrder.id, this.assignment);

    apiCall.subscribe(
      (response) => {
        alert(isCreated ? 'Duties assigned successfully!' : 'Assignment updated successfully!');

        // Check if the response contains the updated order status and update UI
        if (response.orderStatus && isCreated) {
          this.selectedOrder.orderStatus = response.orderStatus; // Update local order status
        }

        // Refresh orders to ensure the latest data is displayed
        this.loadSalesPersonOrders();

        // Close the modal
        this.closeModal();
      },
      (error) => {
        console.error('Failed to save assignment:', error);
        alert('Failed to save assignment. Please try again.');
      }
    );
  }


  assignAttributes(): void {
    if (!this.assignment.employeeId || !this.assignment.driverId || !this.assignment.warehouseId) {
      alert('Please select Employee, Driver, and Warehouse.');
      return;
    }

    this.salesPersonService.assignOrder(this.selectedOrder.id, this.assignment).subscribe(
      () => {
        alert('Attributes assigned successfully!');
        this.loadSalesPersonOrders();
        this.closeModal();
      },
      (error) => {
        console.error('Failed to assign attributes:', error);
        alert('Failed to assign attributes. Please try again.');
      }
    );
  }




  logout(): void {
    localStorage.removeItem('authToken');
    this.router.navigate(['/login']);
  }


}
