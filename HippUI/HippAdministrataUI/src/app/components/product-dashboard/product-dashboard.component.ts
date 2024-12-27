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
  newProduct = {
    name: '',
    unlabeledQuantity: 0,
    labeledQuantity: 0,
    price: 0,
    pricePercentageForEmployee: 0,
    warehouseId: 0
  };

  constructor(private productService: ProductService, private router: Router) {}

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

  // Implement navigation method to switch between dashboards
  navigateTo(path: string): void {
    this.router.navigate([`/manager/${path}`]);
  }
    // Add this to your class
  activeSection: string = 'products'; // Default section on page load

  setActiveSection(section: string): void {
    this.activeSection = section;
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
}
