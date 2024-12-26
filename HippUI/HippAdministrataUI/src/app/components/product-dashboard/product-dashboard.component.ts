// product-dashboard.component.ts
import { Component, OnInit } from '@angular/core';
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
  newProduct = { name: '', price: 0, quantity: 0 };

  constructor(private productService: ProductService) {}

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
    if (!this.newProduct.name || !this.newProduct.price || !this.newProduct.quantity) {
      alert('Please fill all product details.');
      return;
    }

    this.productService.addProduct(this.newProduct).subscribe(
      () => {
        alert('Product added successfully!');
        this.newProduct = { name: '', price: 0, quantity: 0 };
        this.loadProducts();
      },
      (error) => console.error('Failed to add product:', error)
    );
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
