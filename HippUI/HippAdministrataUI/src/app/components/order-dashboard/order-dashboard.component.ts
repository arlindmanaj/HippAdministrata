// order-dashboard.component.ts
import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../../services/order.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ClientService } from '../../../services/client.service';
import { OrderStatus } from '../../../models/OrderStatus';

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
  selectedClientId: number | null = null;
  clientOrders: any[] = [];
  errorMessage: string = '';
  orderStatuses = Object.keys(OrderStatus).filter((key) => isNaN(Number(key)));

  constructor(private clientService: ClientService, private orderService: OrderService, private router: Router) { }

  ngOnInit(): void {
    this.loadOrders();
    this.loadClients();
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
    this.clientService.getAllClients().subscribe(
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
}
