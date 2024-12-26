// order-dashboard.component.ts
import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../../services/order.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-order-dashboard',
  templateUrl: './order-dashboard.component.html',
  standalone: true,
  styleUrls: ['./order-dashboard.component.css'],
  imports: [CommonModule]
})
export class OrderDashboardComponent implements OnInit {
  orders: any[] = [];

  constructor(private orderService: OrderService) { }

  ngOnInit(): void {
    this.loadOrders();
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
}
