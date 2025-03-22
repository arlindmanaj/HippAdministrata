import { Component, OnInit, ViewChild } from '@angular/core';
import { EmployeeService } from '../../../services/employee.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ChangeDetectorRef } from '@angular/core'; // Import ChangeDetectorRef
import { SettingsModalComponent } from '../settings-modal/settings-modal.component';
@Component({
  selector: 'app-employee-dashboard',
  templateUrl: './employee-dashboard.component.html',
  standalone: true,
  styleUrls: ['./employee-dashboard.component.css'],
  imports: [CommonModule, FormsModule,SettingsModalComponent],
})
export class EmployeeDashboardComponent implements OnInit {
  orders: any[] = []; // Orders assigned to the employee
  employeeTotalPay: number = 0; // Total pay for the employee
  animatedTotalPay: number = 0; // Animated total pay for smooth transition
  labelingQuantity: { [key: number]: number } = {}; // Quantity input for each order
  employeeId: number = Number(localStorage.getItem('roleSpecificId')); // Employee ID from storage
  sidebarCollapsed: boolean = false;





  constructor(private employeeService: EmployeeService, private router: Router,private cdr: ChangeDetectorRef ) { }

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
        // Calculate total pay before filtering orders
        this.orders = data;
        this.calculateTotalPay(); // Update the total pay based on fetched orders
  
        // Now filter out orders where statusId is 5
        this.orders = this.orders.filter(order => order.orderStatusId !== 5);
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
        1
        // Update total pay
        const productPaymentPerLabel = (order.productPrice * order.pricePercentageForEmployee) / 100;
        this.employeeTotalPay += labelingQuantity * productPaymentPerLabel;
        if(order.unlabeledQuantity === 0){
          order.orderStatusDescription = "Ready For Shipping"; 
          this.cdr.detectChanges();
        }

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
    // this.animateTotalPay(this.employeeTotalPay);
  }
  logout(): void {
    localStorage.removeItem('authToken');
    localStorage.clear();
    this.router.navigate(['/login']);
  }
  toggleSidebar(): void {
    this.sidebarCollapsed = !this.sidebarCollapsed;
  }
  @ViewChild('settingsModal') settingsModal!: SettingsModalComponent;

  openSettingsModal() {
    this.settingsModal.toggleModal();
  }
}
