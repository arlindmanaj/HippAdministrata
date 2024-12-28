import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../../../services/employee.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OrderStatus } from '../../../models/OrderStatus';
<<<<<<< HEAD
import { getOrderStatusLabel } from '../../../services/order-status.util';
import { Router } from '@angular/router';
=======
import { Chart } from 'chart.js';
import { Router } from '@angular/router';  // Import Router

>>>>>>> f8e85cfe04a3401b5b96eff1a87de72801965624

@Component({
  selector: 'app-employee-dashboard',
  templateUrl: './employee-dashboard.component.html',
  standalone: true,
  styleUrls: ['./employee-dashboard.component.css'],
  imports: [CommonModule, FormsModule],
})
export class EmployeeDashboardComponent implements OnInit {
  orders: any[] = []; // Orders assigned to the employee
  employeeTotalPay: number = 0; // Total pay for the employee
  animatedTotalPay: number = 0; // Animated total pay for smooth transition
  labelingQuantity: { [key: number]: number } = {}; // Quantity input for each order
  employeeId: number = Number(localStorage.getItem('roleSpecificId')); // Employee ID from storage

<<<<<<< HEAD



=======
  // Removed the manual declaration of router, as it's being injected
>>>>>>> f8e85cfe04a3401b5b96eff1a87de72801965624
  constructor(private employeeService: EmployeeService, private router: Router) { }

  ngOnInit(): void {
    this.loadEmployeeTasks();
  }

  loadEmployeeTasks(): void {
    if (!this.employeeId) {
      alert('Employee ID not found. Please log in again.');
      return;
    }

    this.employeeService.getAssignedOrders(this.employeeId).subscribe(
      (data) => {
        this.orders = data;
        this.calculateTotalPay(); // Update the total pay based on fetched orders
      },
      (error) => console.error('Failed to load employee tasks:', error)
    );
  }

  addLabels(orderId: number, labelingQuantity: number): void {
    if (labelingQuantity <= 0) {
      alert('Please enter a valid labeling quantity');
      return;
    }

    const order = this.orders.find((o) => o.id === orderId);
    if (!order) {
      alert('Order not found.');
      return;
    }

    if (labelingQuantity > order.unlabeledQuantity) {
      alert('Labeling quantity exceeds available unlabeled quantity.');
      return;
    }

    const payload = {
      orderId,
      labelingQuantity,
    };

    this.employeeService.labelProduct(this.employeeId, payload).subscribe(
      () => {
        // Update order quantities in the UI
        order.labeledQuantity += labelingQuantity;
        order.unlabeledQuantity -= labelingQuantity;

        // Update total pay
        const productPaymentPerLabel = (order.productPrice * order.pricePercentageForEmployee) / 100;
        this.employeeTotalPay += labelingQuantity * productPaymentPerLabel;

        // Trigger total pay animation
        this.animateTotalPay(this.employeeTotalPay);

        // Check if order is fully labeled
      },
      (error) => {
        console.error('Failed to label product:', error);
        alert('Failed to add labels. Please try again.');
      }
    );
  }

  calculateTotalPay(): void {
    // Calculate total pay based on labeled quantities
    this.employeeTotalPay = this.orders.reduce((total, order) => {
      const productPaymentPerLabel = (order.productPrice * order.pricePercentageForEmployee) / 100;
      return total + order.labeledQuantity * productPaymentPerLabel;
    }, 0);

    // Trigger the animation after calculation
    this.animateTotalPay(this.employeeTotalPay);
  }
<<<<<<< HEAD
  logout(): void {
    localStorage.removeItem('authToken');
    this.router.navigate(['/login']);
  }
}
=======

  animateTotalPay(targetPay: number): void {
    let currentPay = 0;
    const interval = setInterval(() => {
      this.animatedTotalPay = currentPay;
      currentPay += 50;  // Increase by 50 each time
      if (currentPay >= targetPay) {
        this.animatedTotalPay = targetPay;  // Set to target when done
        clearInterval(interval);
      }
    }, 20);  // Speed of the animation (milliseconds)
  }

  logout(): void {
    // Clear the session or token
    localStorage.removeItem('authToken');
    
    // Navigate to login page after logging out
    this.router.navigate(['/login']);
  }
}
>>>>>>> f8e85cfe04a3401b5b96eff1a87de72801965624
