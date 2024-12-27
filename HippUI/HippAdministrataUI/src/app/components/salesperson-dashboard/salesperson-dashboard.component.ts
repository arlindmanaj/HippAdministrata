import { Component, OnInit } from '@angular/core';
import { SalesPersonService } from '../../../services/salesperson.service';
import { UserService } from '../../../services/user.service';
import { OrderStatus } from '../../../models/OrderStatus';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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

  constructor(
    private userService: UserService,
    private salesPersonService: SalesPersonService
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

  openDetailsModal(order: any): void {
    this.selectedOrder = order;

    // Prefill assignment values if they exist, otherwise reset to null
    this.assignment = {
      employeeId: order.assignment?.employeeId || null,
      driverId: order.assignment?.driverId || null,
      warehouseId: order.assignment?.warehouseId || null,
    };

    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
    this.selectedOrder = null;
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

  

  getOrderStatus(status: number): string {
    const statusMap = {
      0: 'Created',
      1: 'In Progress',
      2: 'Completed',
    };
    return OrderStatus[status] || 'Unknown';
  }
}
