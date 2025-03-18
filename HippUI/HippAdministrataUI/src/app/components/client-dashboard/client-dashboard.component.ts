import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ClientService } from '../../../services/client.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { OrderStatus } from '../../../models/OrderStatus';
import { getOrderStatusLabel } from '../../../services/order-status.util';
import { NotificationService } from '../../../services/notification.service';
import { NotificationComponent } from '../notifications/notification.component';
import { OrderService } from '../../../services/order.service';
import { RealTimeNotificationComponent } from "../real-time-notification/real-time-notification.component";

interface Product {
  productName: string;
  quantity: number;
  productPrice: number;
}

interface Order {
  orderId: number;
  deliveryDestination: string;
  orderStatusDisplay: string;
  products: Product[];
}

@Component({
  selector: 'app-client-dashboard',
  templateUrl: './client-dashboard.component.html',
  standalone: true,
  styleUrls: ['./client-dashboard.component.css'],
  imports: [FormsModule, CommonModule,NotificationComponent, RealTimeNotificationComponent],
})
export class ClientDashboardComponent implements OnInit {
  products: any[] = []; // List of products
  orders: any[] = []; // List of orders
  newOrder = {
    products: [{ productId: null, quantity: 0 }], // Track multiple products
    deliveryDestination: ''
  };
  orderStatuses = Object.keys(OrderStatus).filter((key) => isNaN(Number(key))); // Available statuses
  isOrderModalOpen = false;
  sidebarCollapsed: boolean = false;


  assignedOrders: any[] = [];
  activeSection: string = 'orders'; // Default active section
  groupedOrders: Order[] = []; // Grouped orders for display
  recentOrders: Order[] = []; // To store the two most recent orders
  expandedOrders: Set<number> = new Set(); // Track expanded orders

  isUpdateModalOpen = false;
  orderId!: number;
  reason!: string;
  newDeliveryDestination?: string;
  newQuantity?: number;
  newProductId?: number;
updateRequest = {
  orderId: 0,
  clientId: 0,
  requestType: "Update",
  reason: "",
  newDeliveryDestination: "",
  newQuantity: 0,
  newProductId: 0
};




  constructor(private router: Router, private clientService: ClientService, private orderService: OrderService) { }

  ngOnInit(): void {
    this.loadProducts();
    this.loadOrders();
  }

  // Expose the utility function for order status
  getOrderStatusLabel = getOrderStatusLabel;

  // Section Handling
  viewSection(section: string): void {
    this.activeSection = section;
    if (section === 'create-order') {
      this.loadRecentOrders(); // Load recent orders when switching to "Create Order" page
    }
  }

  // Modal Handling
  openOrderModal(): void {
    this.isOrderModalOpen = true;
  }

  closeOrderModal(): void {
    this.isOrderModalOpen = false;
    this.newOrder = { products: [{ productId: null, quantity: 0 }], deliveryDestination: '' };
  }

  // Load Products
  loadProducts(): void {
    this.clientService.getProducts().subscribe(
      (data) => (this.products = data),
      (error) => console.error('Failed to load products:', error)
    );
  }

  // Load Orders
  loadOrders(): void {
    const clientId = Number(localStorage.getItem('roleSpecificId')); // Fetch clientId from localStorage
    if (!clientId) {
      alert('Client ID not found. Please log in again.');
      return;
    }
    console.log(clientId);
    this.clientService.getOrdersByClientId(clientId).subscribe(
      (orders) => {
        console.log(orders);
        // Map the orders to display order details
        const formattedOrders = orders.map(order => ({
          ...order,
          orderStatusDisplay: order.orderStatus.description,
          productName: this.getProductName(order.productId)
        }));

        // Group orders for frontend display
        this.groupedOrders = this.groupOrders(formattedOrders);
        // console.log(this.groupedOrders)
        this.loadRecentOrders(); // Refresh recent orders whenever orders are loaded
      },
      (error) => {
        console.error('Failed to load orders:', error);
        alert('Failed to load orders. Please try again later.');
      }
    );
  }

  // Load Recent Orders
  loadRecentOrders(): void {
    if (this.groupedOrders.length > 0) {
      this.recentOrders = this.groupedOrders

        .slice(0, 2); // Take the first two orders
    }
  }

  // Group Orders
  groupOrders(orders: any[]): Order[] {
    return orders.reduce((acc: Order[], order: any) => {
      const existingGroup = acc.find(
        (group: Order) =>
          group.deliveryDestination === order.deliveryDestination &&
          group.orderStatusDisplay === order.orderStatusDisplay
      );
      if (existingGroup) {
        existingGroup.products.push({
          productName: order.productName,
          quantity: order.quantity,
          productPrice: order.productPrice
        });
      } else {
        acc.push({
          orderId: order.id, // Use the first order's ID for display
          deliveryDestination: order.deliveryDestination,
          orderStatusDisplay: order.orderStatusDisplay,
          products: [
            {
              productName: order.productName,
              quantity: order.quantity,
              productPrice: order.productPrice
            }
          ]
        });
      }
      return acc;
    }, []);
  }

  // Helpers
  getOrderStatusDisplay(status: number): string {
    const statusMap: { [key: number]: string } = {
      0: 'Created',
      1: 'InProgress',
      2: 'Labeling',
      4: 'Ready For Shipping',
      5: 'In Transit',
      6: 'Shipped',
    };
    return statusMap[status] || 'Unknown';
  }

  // Get Product Name
  getProductName(productId: number): string {
    const product = this.products.find(p => p.id === productId);
    return product ? product.name : 'Unknown Product';
  }

  // Add Product to Order
  addProduct(): void {
    this.newOrder.products.push({ productId: null, quantity: 0 });
  }

  // Create Multiple Orders
  createMultipleOrders(): void {
    const clientId = Number(localStorage.getItem('roleSpecificId'));
    if (!clientId) {
      alert('Client ID not found. Please log in again.');
      return;
    }

    if (!this.newOrder.deliveryDestination || this.newOrder.products.length === 0) {
      alert('Please fill out all fields.');
      return;
    }

    const payload = {
      deliveryDestination: this.newOrder.deliveryDestination,
      products: this.newOrder.products.filter(p => p.productId && p.quantity > 0)
    };

    if (payload.products.length === 0) {
      alert('Please add at least one valid product with quantity.');
      return;
    }

    this.clientService.createMultipleOrders(clientId, payload).subscribe(
      () => {
        alert('Orders placed successfully!');
        this.newOrder = { products: [{ productId: null, quantity: 0 }], deliveryDestination: '' };
        this.loadOrders();
        this.closeOrderModal();
      },
      (error) => {
        console.error('Failed to place orders:', error);
        alert('Failed to place orders. Please try again.');
      }
    );
  }

  // Expand/Collapse Orders
  toggleOrder(orderId: number): void {
    if (this.expandedOrders.has(orderId)) {
      this.expandedOrders.delete(orderId);
    } else {
      this.expandedOrders.add(orderId);
    }
  }

  isOrderOpen(orderId: number): boolean {
    return this.expandedOrders.has(orderId);
  }

  // Logout
  logout(): void {
    localStorage.removeItem('authToken');
    this.router.navigate(['/login']);
    localStorage.clear();
  }
  toggleSidebar(): void {
    this.sidebarCollapsed = !this.sidebarCollapsed;
  }

  requestOrderAction(orderId: number, requestType: string): void {

  
    const userId = localStorage.getItem('userId'); 
  
    if (!userId) {
      console.error('User ID not found. Please log in again.');
      return;
    }
  
    this.clientService.getClientIdByUserId(Number(userId)).subscribe({
      next: (clientId) => {
        if (!clientId) {
          console.error('Client ID not found');
          return;
        }
  
        this.orderService.requestOrder(orderId, clientId, requestType, 'Reason for request')
          .subscribe({
            next: (response) => {
              console.log('Order request successfully created:', response);
            },
            error: (err) => {
              console.error('Error creating order request:', err);
            }
          });
      },
      error: (err) => {
        console.error('Error fetching client ID:', err);
      }
    });
  }

  submitUpdateRequest() {
    const userId = localStorage.getItem('userId');
  
    if (!userId) {
      console.error('User ID not found. Please log in again.');
      return;
    }
  
    this.clientService.getClientIdByUserId(Number(userId)).subscribe({
      next: (clientId) => {
        if (!clientId) {
          console.error('Client ID not found.');
          return;
        }
  
        this.updateRequest.clientId = clientId;
  
        this.orderService.requestUpdateOrder(this.updateRequest).subscribe({
          next: (response) => {
            console.log('Update request submitted successfully:', response);
            this.closeUpdateModal();
          },
          error: (err) => {
            console.error('Error submitting update request:', err);
          }
        });
      },
      error: (err) => {
        console.error('Error fetching client ID:', err);
      }
    });
  }
  
  openUpdateModal(orderId: number) {
    this.updateRequest.orderId = orderId;
    this.isUpdateModalOpen = true;
  }
  
  // Close the update modal
  closeUpdateModal() {
    this.isUpdateModalOpen = false;
  }
  
}






  
  
  

