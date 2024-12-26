import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ClientService } from '../../../services/client.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { OrderStatus } from '../../../models/OrderStatus';

@Component({
  selector: 'app-client-dashboard',
  templateUrl: './client-dashboard.component.html',
  standalone: true,
  styleUrls: ['./client-dashboard.component.css'],
  imports: [FormsModule, CommonModule],
})
export class ClientDashboardComponent implements OnInit {
  products: any[] = []; // List of products
  orders: any[] = []; // List of orders
  newOrder = { productId: null, quantity: 0, deliveryDestination: '' }; // Order form
  orderStatuses = Object.keys(OrderStatus).filter((key) => isNaN(Number(key)));
  isOrderModalOpen = false;

  constructor(private router: Router, private clientService: ClientService) { }

  ngOnInit(): void {
    this.loadProducts();
    this.loadOrders();
  }

  openOrderModal(): void {
    this.isOrderModalOpen = true;
  }

  closeOrderModal(): void {
    this.isOrderModalOpen = false;
    this.newOrder = { productId: null, quantity: 0, deliveryDestination: '' };
  }

  logout(): void {
    localStorage.removeItem('authToken');
    this.router.navigate(['/login']);
  }

  getOrderStatus(status: number): string {
    return OrderStatus[status];
  }

  loadProducts(): void {
    this.clientService.getProducts().subscribe(
      (data) => (this.products = data),
      (error) => console.error('Failed to load products:', error)
    );
  }

  loadOrders(): void {
    const clientId = Number(localStorage.getItem('roleSpecificId')); // Fetch clientId from localStorage
    if (!clientId) {
      alert('Client ID not found. Please log in again.');
      return;
    }
  
    this.clientService.getOrdersByClientId(clientId).subscribe(
      (orders) => {
        this.orders = orders.map(order => ({
          ...order,
          orderStatusDisplay: this.getOrderStatusDisplay(order.orderStatus),
          productName: this.getProductName(order.productId)
        }));
      },
      (error) => {
        console.error('Failed to load orders:', error);
        alert('Failed to load orders. Please try again later.');
      }
    );
  }
  


  getOrderStatusDisplay(status: number): string {
    const statusMap: { [key: number]: string } = {
      0: 'Created',
      1: 'InProgress',
      2: 'Labeled',
      3: 'Packaged',
      4: 'Ready For Shipping',
      5: 'In Transit',
      6: 'Shipped',
      7: 'Completed',
    };
    return statusMap[status] || 'Unknown';
  }
  getProductName(productId: number): string {
    const product = this.products.find(p => p.id === productId);
    return product ? product.name : 'Unknown Product';
  }


  createOrder(): void {
    const clientId = Number(localStorage.getItem('roleSpecificId')); // Fetch clientId from localStorage
    if (!clientId) {
      alert('Client ID not found. Please log in again.');
      return;
    }

    if (!this.newOrder.productId || !this.newOrder.quantity || !this.newOrder.deliveryDestination) {
      alert('Please fill out all fields.');
      return;
    }

    // Build the correct payload
    const payload = {
      deliveryDestination: this.newOrder.deliveryDestination,
      products: [
        {
          productId: this.newOrder.productId,
          quantity: this.newOrder.quantity
        }
      ]
    };

    console.log('Order Payload:', payload); // Log the payload for debugging
    console.log('Selected Product ID:', this.newOrder.productId);

    this.clientService.createOrder(clientId, payload).subscribe(
      () => {
        alert('Order placed successfully!');
        this.newOrder = { productId: null, quantity: 0, deliveryDestination: '' };
        this.loadOrders();
      },
      (error) => {
        console.error('Failed to place order:', error);
        alert('Failed to place order. Please try again.');
      }
    );
  }

}
