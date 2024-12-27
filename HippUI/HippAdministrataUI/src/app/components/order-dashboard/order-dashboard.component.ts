import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OrderService } from '../../../services/order.service';
import { ClientService } from '../../../services/client.service';
import { UserService } from '../../../services/user.service';
import { SalesPersonService } from '../../../services/salesperson.service';
import { OrderStatus } from '../../../models/OrderStatus';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-order-dashboard',
  templateUrl: './order-dashboard.component.html',
  styleUrls: ['./order-dashboard.component.css'],
  standalone: true,
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
  salesPersonsOrders: any[] = [];
  errorMessage: string = '';
  orderStatuses = Object.keys(OrderStatus).filter((key) => isNaN(Number(key)));
  showAllOrders: boolean = false; // New variable to toggle All Orders view

  constructor(
    private salesPersonService: SalesPersonService,
    private userService: UserService,
    private clientService: ClientService,
    private orderService: OrderService,
    private router: Router
  ) {}

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
      (document.getElementById('salesPersonDropdown') as HTMLSelectElement).value = ''; // Reset SalesPerson dropdown
      this.clientService.getOrdersByClientId(selectedClientId).subscribe(
        (data) => (this.clientOrders = data),
        (error) => (this.errorMessage = 'Failed to load client orders')
      );
    } else {
      this.clientOrders = [];
    }
  }
  
  loadSalesPersonTasks(event: Event): void {
    const target = event.target as HTMLSelectElement;
    const selectedSalesPersonId = Number(target.value);
  
    if (selectedSalesPersonId) {
      this.selectedClientId = null; // Clear selected client
      this.clientOrders = []; // Clear client orders
      (document.getElementById('clientDropdown') as HTMLSelectElement).value = ''; // Reset Client dropdown
      this.salesPersonService.getOrdersBySalesPersonId(selectedSalesPersonId).subscribe(
        (data) => (this.salesPersonsOrders = data),
        (error) => (this.errorMessage = 'Failed to load salesperson tasks')
      );
    } else {
      this.salesPersonsOrders = [];
    }
  }
  

  getOrderStatus(status: number): string {
    return OrderStatus[status];
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

  loadSalesPersons(): void {
    this.userService.getAllSalesPersons().subscribe(
      (data) => (this.salesPersons = data),
      (error) => console.error('Failed to load salespersons:', error)
    );
  }

  // loadSalesPersonTasks(salesPersonId: number): void {
  //   this.salesPersonService.getOrdersBySalesPersonId(salesPersonId).subscribe(
  //     (data) => (this.salesPersonsOrders = data),
  //     (error) => console.error('Failed to load salesperson tasks:', error)
  //   );
  // }

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
  
    // Toggle visibility for All Orders
    toggleAllOrders() {
      this.showAllOrders = !this.showAllOrders;
      if (this.showAllOrders) {
        // Load all orders when "All Orders" is clicked
        this.loadOrders();
      }
    }

}
