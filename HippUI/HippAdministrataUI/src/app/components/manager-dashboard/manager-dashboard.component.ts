import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { Chart, registerables } from 'chart.js';
import { ProductService } from '../../../services/product.service';
import { OrderService } from '../../../services/order.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-manager-dashboard',
  templateUrl: './manager-dashboard.component.html',
  styleUrls: ['./manager-dashboard.component.css'],
  standalone: true,
  imports: [RouterModule],
})
export class ManagerDashboardComponent implements OnInit {
  activeSection: string = 'manager'; // Default active section

  constructor(
    private router: Router,
    private productService: ProductService,
    private orderService: OrderService
  ) {
    Chart.register(...registerables);
  }

  ngOnInit(): void {
    this.loadProductChart();
    this.loadRevenueChart();
    this.loadRevenueShareChart();
    this.loadTopProductsChart();
    
  }

  // Navigation logic
  navigateTo(path: string): void {
    this.activeSection = path; // Update active section
    this.router.navigate([`/manager/${path}`]);
  }

  // Logout method
  logout(): void {
    localStorage.removeItem('authToken');
    this.router.navigate(['/login']);
  }

  // Load Product Quantities Chart
  loadProductChart(): void {
    this.productService.getProducts().subscribe((products) => {
      const productNames = products.map((product: any) => product.name);
      const productQuantities = products.map(
        (product: any) =>
          product.unlabeledQuantity + product.labeledQuantity
      );

      new Chart('productChart', {
        type: 'bar',
        data: {
          labels: productNames,
          datasets: [
            {
              label: 'Quantities',
              data: productQuantities,
              backgroundColor: 'rgba(75, 192, 192, 0.6)',
              borderColor: 'rgba(75, 192, 192, 1)',
              borderWidth: 1,
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: { display: true },
            title: { display: true, text: 'Product Quantities' },
          },
        },
      });
    });
  }

  // Load Revenue Breakdown Chart
  loadRevenueChart(): void {
    forkJoin({
      orders: this.orderService.getOrders(),
      products: this.productService.getProducts(),
    }).subscribe(({ orders, products }) => {
      const productMap = new Map(
        products.map((product: any) => [product.id, product])
      );

      const productNames: string[] = [];
      const grossRevenue: number[] = [];
      const employeeShare: number[] = [];
      const netRevenue: number[] = [];

      orders.forEach((order) => {
        const product = productMap.get(order.productId);
        if (product) {
          const totalRevenue = order.labeledQuantity * product.price;
          const share =
            (product.pricePercentageForEmployee / 100) * totalRevenue;
          productNames.push(product.name);
          grossRevenue.push(totalRevenue);
          employeeShare.push(share);
          netRevenue.push(totalRevenue - share);
        }
      });

      new Chart('revenueChart', {
        type: 'bar',
        data: {
          labels: productNames,
          datasets: [
            {
              label: 'Gross Revenue',
              data: grossRevenue,
              backgroundColor: 'rgba(75, 192, 192, 0.6)',
              borderColor: 'rgba(75, 192, 192, 1)',
              borderWidth: 1,
            },
            {
              label: "Employee's Share",
              data: employeeShare,
              backgroundColor: 'rgba(255, 99, 132, 0.6)',
              borderColor: 'rgba(255, 99, 132, 1)',
              borderWidth: 1,
            },
            {
              label: 'Net Revenue',
              data: netRevenue,
              backgroundColor: 'rgba(54, 162, 235, 0.6)',
              borderColor: 'rgba(54, 162, 235, 1)',
              borderWidth: 1,
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: { display: true },
            title: { display: true, text: 'Revenue Breakdown' },
          },
          scales: {
            x: { title: { display: true, text: 'Products' } },
            y: { title: { display: true, text: 'Amount ($)' } },
          },
        },
      });
    });
  }

  // Load Revenue Share Chart
  loadRevenueShareChart(): void {
    this.productService.getProducts().subscribe((products) => {
      const productNames = products.map((product: any) => product.name);
      const grossRevenue = products.map(
        (product: any) => product.labeledQuantity * product.price
      );

      new Chart('revenueShareChart', {
        type: 'doughnut',
        data: {
          labels: productNames,
          datasets: [
            {
              label: 'Revenue Share by Product',
              data: grossRevenue,
              backgroundColor: [
                'rgba(255, 99, 132, 0.6)',
                'rgba(54, 162, 235, 0.6)',
                'rgba(255, 206, 86, 0.6)',
                'rgba(75, 192, 192, 0.6)',
                'rgba(153, 102, 255, 0.6)',
              ],
              borderColor: 'rgba(255, 255, 255, 1)',
              borderWidth: 2,
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: { display: true },
            title: { display: true, text: 'Revenue Share by Product' },
          },
        },
      });
    });
  }
  loadTopProductsChart(): void {
    this.productService.getProducts().subscribe((products) => {
      // Sort products by revenue in descending order
      const topProducts = products
        .map((product: any) => ({
          name: product.name,
          revenue: product.labeledQuantity * product.price,
        }))
        .sort((a: any, b: any) => b.revenue - a.revenue)
        .slice(0, 5); // Get top 5 products
  
      const productNames = topProducts.map((product: any) => product.name);
      const revenues = topProducts.map((product: any) => product.revenue);
  
      new Chart('topProductsChart', {
        type: 'bar', // Use 'bar' chart type
        data: {
          labels: productNames,
          datasets: [
            {
              label: 'Revenue',
              data: revenues,
              backgroundColor: [
                'rgba(255, 99, 132, 0.6)',
                'rgba(54, 162, 235, 0.6)',
                'rgba(255, 206, 86, 0.6)',
                'rgba(75, 192, 192, 0.6)',
                'rgba(153, 102, 255, 0.6)',
              ],
              borderColor: 'rgba(255, 255, 255, 1)',
              borderWidth: 1,
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          indexAxis: 'y', // This makes the chart horizontal
          plugins: {
            legend: { display: true },
            title: { display: true, text: 'Top Performing Products' },
          },
          scales: {
            x: {
              title: { display: true, text: 'Revenue ($)' },
            },
            y: {
              title: { display: true, text: 'Products' },
            },
          },
        },
      });
    });
  }
  
  
  
}
