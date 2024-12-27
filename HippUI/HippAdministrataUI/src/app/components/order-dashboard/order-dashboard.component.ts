// order-dashboard.component.ts
import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../../services/order.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ClientService } from '../../../services/client.service';
import { OrderStatus } from '../../../models/OrderStatus';
import { UserService } from '../../../services/user.service';
import { SalesPersonService } from '../../../services/salesperson.service';

@Component({
  selector: 'app-order-dashboard',
  templateUrl: './order-dashboard.component.html',
  standalone: true,
  styleUrls: ['./order-dashboard.component.css'],
  imports: [CommonModule]
})
export class OrderDashboardComponent implements OnInit {
  orders: any[] = [];
  clients: any[] = [];
  employees: any[] = [];
  drivers: any[] = [];
  salesPersons: any[] = [];
  selectedClientId: number | null = null;
  clientOrders: any[] = [];
  salesPersonsOrders: any[] =[];
  errorMessage: string = '';
  orderStatuses = Object.keys(OrderStatus).filter((key) => isNaN(Number(key)));

  constructor(private salesPersonService: SalesPersonService, private userService: UserService, private clientService: ClientService, private orderService: OrderService, private router: Router) { }

  ngOnInit(): void {
    this.loadOrders();
    this.loadClients();
    this.loadEmployees();
    this.loadDrivers();
    this.loadSalesPersons();
  }

  loadOrders(): void {
    this.orderService.getOrders().subscribe(
      (orders) => (this.orders = orders),
      (error) => console.error('Failed to load orders:', error)
    );
  }
  deleteOrder(orderId: number): void {
    if (confirm('Are you sure you want to delete this order?')) {
      this.orderService.deleteOrder(orderId).subscribe(
        () => {
          alert('Order deleted successfully!');
          this.loadOrders(); // Reload orders after deletion
        },
        (error) => {
          console.error('Failed to delete order:', error);
          alert('Failed to delete order. Please try again.');
        }
      );
    }
  }

  loadClients(): void {
    this.userService.getAllClients().subscribe(
      (data) => (this.clients = data),
      (error) => (this.errorMessage = 'Failed to load clients')
    );
  }

  loadClientOrders(clientId: number): void {
    this.selectedClientId = clientId;
    this.clientService.getOrdersByClientId(clientId).subscribe(
      (data) => (this.clientOrders = data),
      (error) => (this.errorMessage = 'Failed to load client orders')
    );
  }
  getOrderStatus(status: number): string {
    return OrderStatus[status];
  }

  goToManager(): void {
    this.router.navigate(['/manager-dashboard']);
  }
  loadEmployees(): void {
    this.userService.getAllEmployees().subscribe(
      (data) => (this.employees = data),
      (error) => console.error('Failed to load employees:', error)
    );
  }

  loadDrivers(): void {
    this.userService.getAllDrivers().subscribe(
      (data) => (this.drivers = data),
      (error) => console.error('Failed to load drivers:', error)
    );
  }

  loadSalesPersons(): void {
    this.userService.getAllSalesPersons().subscribe(
      (data) => (this.salesPersons = data),
      (error) => console.error('Failed to load salespersons:', error)
    );
  }

  // loadEmployeeTasks(employeeId: number): void {
  //   this.employeeService.getTasksByEmployeeId(employeeId).subscribe(
  //     (data) => (this.employeeTasks = data),
  //     (error) => console.error('Failed to load employee tasks:', error)
  //   );
  // }

  // loadDriverTasks(driverId: number): void {
  //   this.driverService.getTasksByDriverId(driverId).subscribe(
  //     (data) => (this.driverTasks = data),
  //     (error) => console.error('Failed to load driver tasks:', error)
  //   );
  // }

  loadSalesPersonTasks(salesPersonId: number): void {
    this.salesPersonService.getOrdersBySalesPersonId(salesPersonId).subscribe(
      (data) => (this.salesPersonsOrders = data),
      (error) => console.error('Failed to load salesperson tasks:', error)
    );
  }

}
