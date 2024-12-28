import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OrderService } from '../../../services/order.service';
import { ClientService } from '../../../services/client.service';
import { UserService } from '../../../services/user.service';
import { SalesPersonService } from '../../../services/salesperson.service';
import { OrderStatus } from '../../../models/OrderStatus';
import { CommonModule } from '@angular/common';
import { getOrderStatusLabel } from '../../../services/order-status.util';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-order-dashboard',
  templateUrl: './order-dashboard.component.html',
  styleUrls: ['./order-dashboard.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]

})
export class OrderDashboardComponent implements OnInit {
  orders: any[] = [];
  clients: any[] = [];
  employees: any[] = [];
  drivers: any[] = [];
  salesPersons: any[] = [];
  selectedClientId: number | null = null;
  clientOrders: any[] = [];
  salesPersonsOrders: any[] = [];
  errorMessage: string = '';
  orderStatuses = Object.keys(OrderStatus).filter((key) => isNaN(Number(key)));
  showAllOrders: boolean = false; // New variable to toggle All Orders view
  selectedSalesPersonId: string | null = null;
  salesPersonId: number = 0;

  constructor(
    private salesPersonService: SalesPersonService,
    private userService: UserService,
    private clientService: ClientService,
    private orderService: OrderService,
    public router: Router,
    private cdr: ChangeDetectorRef // Add this
  ) { }

  ngOnInit(): void {
    this.loadOrders();
    this.loadClients();
    this.loadEmployees();
    this.loadDrivers();
    this.loadSalesPersons();
  }

  loadOrders(): void {
    this.orderService.getOrders().subscribe(
      (orders) => {
        this.orders = orders; // Update the orders array with the latest data
        this.cdr.detectChanges(); // Trigger change detection
      },
      (error) => {
        console.error('Failed to load orders:', error);
      }
    );
  }


  deleteOrder(orderId: number): void {
    if (confirm('Are you sure you want to delete this order?')) {
      this.orderService.deleteOrder(orderId).subscribe({
        next: (response) => {
          console.log(response);  // Logs the plain text response (e.g., "Order deleted successfully")
          alert('Order deleted successfully!');

          // Update the orders array to remove the deleted order
          this.orders = this.orders.filter(order => order.id !== orderId);

          // Optionally reload orders if All Orders is active
          if (this.showAllOrders) {
            this.loadOrders();
          }

          // Trigger change detection manually
          this.cdr.detectChanges();
        },
        error: (error) => {
          console.error('Failed to delete order:', error);
          alert('Failed to delete the order. Please try again.');
        },
      });
    }
  }







  loadClients(): void {
    this.userService.getAllClients().subscribe(
      (data) => (this.clients = data),
      (error) => (this.errorMessage = 'Failed to load clients')
    );
  }

  // loadClientOrders(clientId: number): void {
  //   this.selectedClientId = clientId;
  //   this.clientService.getOrdersByClientId(clientId).subscribe(
  //     (data) => (this.clientOrders = data),
  //     (error) => (this.errorMessage = 'Failed to load client orders')
  //   );
  // }
  loadClientOrders(event: Event): void {
    const target = event.target as HTMLSelectElement;
    const selectedClientId = Number(target.value);

    if (selectedClientId) {
      this.selectedClientId = selectedClientId;
      this.salesPersonsOrders = []; // Clear SalesPerson orders
      this.showAllOrders = false; // Turn off "All Orders" section

      // Set 'orders' as the active section immediately after selecting client
      this.activeSection = 'orders';

      // Reset SalesPerson dropdown
      (document.getElementById('salesPersonDropdown') as HTMLSelectElement).value = '';

      this.clientService.getOrdersByClientId(selectedClientId).subscribe(
        (data) => (this.clientOrders = data),
        (error) => (this.errorMessage = 'Failed to load client orders')
      );
    } else {
      // If no client is selected, keep the "Orders" section active
      this.clientOrders = [];
      this.showAllOrders = false; // Turn off "All Orders" section if no client is selected
      this.activeSection = 'orders'; // Keep 'orders' as active section
    }
  }


  // Updated navigateTo method for dynamic routing within Manager Dashboard
  navigateTo(route: string): void {
    // Navigate based on the route passed ('products' or 'orders')
    this.router.navigate([`/manager/${route}`]);
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
  getOrderStatusLabel(status: number | string): string {
    // If the status is already a string, return it
    if (typeof status === 'string') {
      return status;
    }

    // Convert numeric status to the corresponding string value
    return OrderStatus[status] || 'Unknown';
  }
  loadSalesPersons(): void {
    this.userService.getAllSalesPersons().subscribe(
      (data) => (this.salesPersons = data),
      (error) => console.error('Failed to load salespersons:', error)
    );
  }

  loadSalesPersonTasks(salesPersonId: number): void {
    this.salesPersonId = salesPersonId
    this.salesPersonService.getOrdersBySalesPersonId(salesPersonId).subscribe(
      (data) => (this.salesPersonsOrders = data),
      (error) => console.error('Failed to load salesperson tasks:', error)
    );
  }

  // Add this to your class
  activeSection: string = 'orders'; // Default section on page load

  setActiveSection(section: string): void {
    this.activeSection = section;
  }

  roles: string[] = ['SalesPerson', 'Driver', 'Employee'];
  filteredPeople: any[] = [];




  // Logout method
  logout() {
    // Perform any logout logic (like clearing session or tokens)
    console.log('Logging out...');

    // Redirect to the login page or another route if needed
    this.router.navigate(['/login']);  // Adjust the route as needed
  }

  toggleAllOrders(): void {
    this.showAllOrders = !this.showAllOrders;

    if (this.showAllOrders) {
      this.activeSection = 'allOrders';
      this.selectedClientId = null;
      this.selectedSalesPersonId = null;
      this.clientOrders = [];
      this.salesPersonsOrders = [];
    } else {
      this.activeSection = 'orders';
    }
  }
}
