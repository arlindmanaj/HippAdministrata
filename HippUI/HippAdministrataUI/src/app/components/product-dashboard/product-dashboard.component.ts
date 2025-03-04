import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProductService } from '../../../services/product.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-dashboard',
  templateUrl: './product-dashboard.component.html',
  standalone: true,
  styleUrls: ['./product-dashboard.component.css'],
  imports: [FormsModule, CommonModule]
})
export class ProductDashboardComponent implements OnInit {
  products: any[] = [];
  activeSection: string = 'createProduct';
  sidebarCollapsed: boolean = false;

  newProduct = {
    name: '',
    unlabeledQuantity: 0,
    labeledQuantity: 0,
    price: 0,
    pricePercentageForEmployee: 0,
    warehouseId: 0.
    
    
  };

  constructor(private productService: ProductService, public router: Router) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getProducts().subscribe(
      (products) => (this.products = products),
      (error) => console.error('Failed to load products:', error)
    );
  }

  addProduct(): void {
    console.log('Current Product Data:', this.newProduct);
  
    if (
      !this.newProduct.name ||
      this.newProduct.price <= 0 ||
      this.newProduct.unlabeledQuantity < 0 ||
      this.newProduct.pricePercentageForEmployee < 0 ||
      this.newProduct.warehouseId <= 0
    ) {
      alert('Please fill all product details correctly.');
      return;
    }
  
    this.productService.addProduct(this.newProduct).subscribe(
      () => {
        alert('Product added successfully!');
        this.newProduct = {
          name: '',
          unlabeledQuantity: 0,
          labeledQuantity: 0,
          price: 0,
          pricePercentageForEmployee: 0,
          warehouseId: 0
        };
        this.loadProducts();
      },
      (error) => console.error('Failed to add product:', error)
    );
  }
  

  setActiveSection(section: string): void {
    this.activeSection = section;
  }

  navigateTo(path: string): void {
    this.router.navigate([`/manager/${path}`]);
  }

  deleteProduct(productId: number): void {
    this.productService.deleteProduct(productId).subscribe(
      () => {
        alert('Product deleted successfully!');
        this.loadProducts();
      },
      (error) => console.error('Failed to delete product:', error)
    );
  }
    // Logout method
    logout() {
      // Perform any logout logic (like clearing session or tokens)
      localStorage.removeItem('authToken');
      localStorage.clear(); 
      // Redirect to the login page or another route if needed
      this.router.navigate(['/login']);  // Adjust the route as needed
    }
  toggleSidebar(): void {
    this.sidebarCollapsed = !this.sidebarCollapsed;
  }
}