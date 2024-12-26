// product-dashboard.component.ts
import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../services/product.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

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


  constructor(private productService: ProductService, private router: Router) { }

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

  deleteProduct(productId: number): void {
    console.log('Deleting product with ID:', productId); // Debugging
    this.productService.deleteProduct(productId).subscribe(
      () => {
        alert('Product deleted successfully!');
        this.loadProducts();
      },
      (error) => {
        console.error('Failed to delete product:', error); // Log the error for debugging
      }
    );
  }
  goToManager(): void {
    this.router.navigate(['/manager-dashboard']);
  }

}
