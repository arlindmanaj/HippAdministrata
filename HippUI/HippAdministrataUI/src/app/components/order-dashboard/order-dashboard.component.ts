import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OrderService } from '../../../services/order.service';
import { ClientService } from '../../../services/client.service';
import { UserService } from '../../../services/user.service';
import { SalesPersonService } from '../../../services/salesperson.service';
import { OrderStatus } from '../../../models/OrderStatus';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../../services/product.service';
import { trigger, transition, style, animate } from '@angular/animations';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NgZone } from '@angular/core';



import { ActivatedRoute } from '@angular/router';





@Component({
  selector: 'app-order-dashboard',
  templateUrl: './order-dashboard.component.html',
  styleUrls: ['./order-dashboard.component.css'],
  standalone: true,
  
  imports: [CommonModule, FormsModule],
  animations: [
    trigger('fadeTable', [
      transition(':leave', [
        animate('300ms ease-out', style({ opacity: 0, transform: 'scale(0.9)' }))
      ]),
      transition(':enter', [
        style({ opacity: 0, transform: 'scale(0.9)' }),
        animate('500ms ease-in', style({ opacity: 1, transform: 'scale(1)' }))
      ])
    ])
  ]

})
export class OrderDashboardComponent implements OnInit {
  orders: any[] = [];
  clients: any[] = [];
  employees: any[] = [];
  drivers: any[] = [];
  salesPersons: any[] = [];
  selectedClientId: number | null = null;
  clientOrders: any[] = [];
  salesPersonsOrders: any[] = [];
  errorMessage: string = '';
  orderStatuses = Object.keys(OrderStatus).filter((key) => isNaN(Number(key)));
  showAllOrders: boolean = false; // New variable to toggle All Orders view
  selectedSalesPersonId: string | null = null;
  salesPersonId: number = 0;
  sidebarCollapsed: boolean = false;
  filteredOrders: any[] = [];
  searchQuery: string = '';
  startDate: string = '';
  endDate: string = '';
  sortColumn: string = 'id';
  sortAscending: boolean = true;
  currentPage: number = 1;
  ordersPerPage: number = 10;
  sortedColumn: string = 'id';
  sortDirection: 'asc' | 'desc' = 'asc';
  searchTerm: string = '';
  selectedOrders: any[] = [];
  filteredProducts: any[] = [];
  isFadingOut: boolean = false;
  loggedInUser: string | undefined;
  private jwtHelper = new JwtHelperService();
  
  orderId: number | null = null;



  constructor(
    private salesPersonService: SalesPersonService,
    private userService: UserService,
    private clientService: ClientService,
    private productService: ProductService,
    private orderService: OrderService,
    public router: Router,
    private cdr: ChangeDetectorRef ,
    private route: ActivatedRoute,
    private ngZone: NgZone// Add this
  ) { }

  ngOnInit(): void {
    this.loadOrders();
    this.loadClients();
    this.loadEmployees();
    this.loadDrivers();
    this.route.queryParams.subscribe(params => {
      const orderId = params['orderId'];
    
      if (orderId) {
        this.searchOrderId = Number(orderId);
    
        // Trigger input event to force the search
        setTimeout(() => {
          const inputElement = document.getElementById('orderSearchInput') as HTMLInputElement;
          if (inputElement) {
            inputElement.dispatchEvent(new Event('input')); // Trigger input event
          }
        }, 0);
      }
    });
    
    
  
    // Watch for changes in searchOrderId and trigger search
    this.watchSearchChanges();
    
    
    this.loadSalesPersons();

      const token = localStorage.getItem('authToken'); // Retrieve the token with the correct key
    
      if (token) {
        try {
          const decodedToken = this.jwtHelper.decodeToken(token);
          console.log(decodedToken); // Log the decoded token to the console
        } catch (error) {
          console.error('Invalid token', error);
        }
      } else {
        console.log('No token found in localStorage');
      }
    
    
  

  }
  searchByOrderId(searchId: number): void {
    this.filteredOrders = this.orders.filter(order => order.id === searchId);
  }


  watchSearchChanges(): void {
    this.ngZone.runOutsideAngular(() => {
      const inputElement = document.querySelector('input[placeholder="Search by Order ID"]') as HTMLInputElement;
  
      inputElement.addEventListener('input', () => {
        this.ngZone.run(() => {
          this.searchOrders(); // Trigger search when user types
        });
      });
    });
  }
  

  
  loadOrders(): void {
    this.orderService.getOrders().subscribe(
      (orders) => {
        console.log('Fetched Orders:', orders); // Debugging line
        this.orders = orders;
        this.cdr.detectChanges();
        
        // Trigger the search after the orders are successfully loaded
        // Pass 'order' as the type, because you're searching by orderId
        this.handleSearch('order');
      },
      (error) => {
        console.error('Failed to load orders:', error);
      }
    );
  }
  getProductName(productId: number): Promise<string> {
    return new Promise((resolve) => {
      this.productService.getProductById(productId).subscribe(
        (product) => resolve(product?.name || "Unknown Product"),
        (error) => {
          console.error(`Error fetching product name for ID ${productId}:`, error);
          resolve("Unknown Product");
        }
      );
    });
  }
  

  


  
  







  loadClients(): void {
    this.userService.getAllClients().subscribe(
      (data) => (this.clients = data),
      (error) => (this.errorMessage = 'Failed to load clients')
    );
  }

  loadClientOrders(event: Event): void {
    const target = event.target as HTMLSelectElement;
    const selectedClientId = Number(target.value);

    if (selectedClientId) {
      this.selectedClientId = selectedClientId;
      this.salesPersonsOrders = []; // Clear SalesPerson orders
      this.showAllOrders = false; // Turn off "All Orders" section

      // Set 'orders' as the active section immediately after selecting client
      this.activeSection = 'orders';

      // Reset SalesPerson dropdown
      (document.getElementById('salesPersonDropdown') as HTMLSelectElement).value = '';

      this.clientService.getOrdersByClientId(selectedClientId).subscribe(
        (data) => (this.clientOrders = data),
        (error) => (this.errorMessage = 'Failed to load client orders')
      );
    } else {
      // If no client is selected, keep the "Orders" section active
      this.clientOrders = [];
      this.showAllOrders = false; // Turn off "All Orders" section if no client is selected
      this.activeSection = 'orders'; // Keep 'orders' as active section
    }
  }


  // Updated navigateTo method for dynamic routing within Manager Dashboard
  navigateTo(route: string): void {
    // Navigate based on the route passed ('products' or 'orders')
    this.router.navigate([`/manager/${route}`]);
  }

  loadEmployees(): void {
    this.userService.getAllEmployees().subscribe(
      (data) => (this.employees = data),
      (error) => console.error('Failed to load employees:', error)
    );
  }

  loadDrivers(): void {
    this.userService.getAllDrivers().subscribe(
      (data) => (this.drivers = data),
      (error) => console.error('Failed to load drivers:', error)
    );
  }


  getOrderStatusLabel(status: number | string): string {
    
    if (typeof status === "number") {
      return OrderStatus[status] ?? "Unknown";
    } else if (typeof status === "string") {
      return Object.values(OrderStatus).includes(status) ? status : "Unknown";
    }
    return "Unknown";
  }
  
  loadSalesPersons(): void {
    this.userService.getAllSalesPersons().subscribe(
      (data) => (this.salesPersons = data),
      (error) => console.error('Failed to load salespersons:', error)
    );
  }

  loadSalesPersonTasks(event: Event): void {
    const target = event.target as HTMLSelectElement;
    const selectedSalesPersonId = Number(target.value);
  
    if (selectedSalesPersonId) {
      this.salesPersonId = selectedSalesPersonId;
      this.clientOrders = []; // Clear Client orders
      this.showAllOrders = false; // Hide All Orders
      this.activeSection = 'orders'; // Ensure the correct section is shown
  
      // Fetch orders for the selected salesperson
      this.salesPersonService.getOrdersBySalesPersonId(selectedSalesPersonId).subscribe(
        (data) => (this.salesPersonsOrders = data),
        (error) => console.error('Failed to load salesperson tasks:', error)
      );
      this.salesPersonService.getOrdersBySalesPersonId(selectedSalesPersonId).subscribe(
        (data) => {
          console.log("Sales Person Orders:", data); // Debugging
          this.salesPersonsOrders = data;
        },
        (error) => console.error('Failed to load salesperson tasks:', error)
      );
      
    } else {
      this.salesPersonsOrders = [];
    }
  }
  


  // Add this to your class
  activeSection: string = 'orders'; // Default section on page load

  setActiveSection(section: string): void {
    this.activeSection = section;
  }



    // searchByOrderId(event: Event): void {
    //   const target = event.target as HTMLInputElement;
    //   const searchId = Number(target.value);
      
    //   this.filteredOrders = !isNaN(searchId)
    //     ? this.orders.filter(order => order.id === searchId)
    //     : [];
    // }
    
    searchByProductId(event: Event): void {
      const target = event.target as HTMLInputElement;
      const searchId = Number(target.value);
      
      this.filteredProducts = !isNaN(searchId)
        ? this.orders.filter(order => order.productId === searchId)
        : [];
    }
    searchOrderId: number | null = null;
searchProductId: number | null = null;
combineSearch: boolean = false;

searchOrders(): void {
  // Convert input values to numbers
  const orderId = this.searchOrderId ? Number(this.searchOrderId) : null;
  const productId = this.searchProductId ? Number(this.searchProductId) : null;

  // If both inputs are empty, reset the filtered list
  if (!orderId && !productId) {
    this.filteredOrders = [];
    return;
  }

  if (this.combineSearch) {
    // Show only orders that match BOTH Order ID & Product ID
    this.filteredOrders = this.orders.filter(order =>
      (!orderId || order.id === orderId) &&
      (!productId || order.productId === productId)
    );
  } else {
    // Show orders that match EITHER Order ID OR Product ID
    this.filteredOrders = this.orders.filter(order =>
      (orderId && order.id === orderId) ||
      (productId && order.productId === productId)
    );
  }
}

    

  // Logout method
  logout() {
    localStorage.clear();
    // Redirect to the login page or another route if needed
    this.router.navigate(['/login']);  // Adjust the route as needed
  }
  handleSearch(type: 'order' | 'product'): void {
    if (!this.combineSearch) {
      if (type === 'order') {
        this.searchProductId = null; // Clear Product ID search
      } else if (type === 'product') {
        this.searchOrderId = null; // Clear Order ID search
      }
    }
  
    this.searchOrders(); // Run search logic
  }
  
  // Reset search when checkbox is toggled
  resetSearch(): void {
    if (!this.combineSearch) {
      this.searchOrderId = null;
      this.searchProductId = null;
      this.filteredOrders = [];
    }
  }
  
  
      
  getProductIdByName(productName: string): Promise<number | null> {
    return this.productService.getAllProducts().toPromise().then((products) => {
      // Check if products is defined and contains items
      if (products && Array.isArray(products)) {
        // Make sure you are using the correct field for the name
        const product = products.find(p => p.productName === productName || p.name === productName); // Using "name" or "productName"
        
        if (product) {
          console.log('Product found:', product);  // Debugging: Check if we found the product
          return product.id;
        } else {
          console.log('Product not found for name:', productName);  // Debugging: Product not found
          return null;
        }
      } else {
        console.error("Products data is missing or invalid.");
        return null;
      }
    }).catch((error) => {
      console.error("Error fetching products:", error);
      return null;
    });
  }
  
  

  printOrder(order: any, source: string): void {
    if (!order) {
      alert("Order data is missing!");
      return;
    }
  
    console.log("Printing Order:", order, "Source:", source); // Debugging log
  
    let productNamePromise: Promise<string>;
    let productIdPromise: Promise<number | string>;
  
    if (source === "salesPerson") {
      // Use productName directly
      productNamePromise = Promise.resolve(order.productName || "Unknown Product");
      productIdPromise = this.getProductIdByName(order.productName).then(id => id ?? "Unknown ID");
    } else {
      // Use getProductName and take productId from the order
      productNamePromise = this.getProductName(order.productId);
      productIdPromise = Promise.resolve(order.productId || "Unknown ID");
    }
  
    Promise.all([productNamePromise, productIdPromise]).then(([productName, productId]) => {
      productName = productName || "Unknown Product";
  
      const currentDate = new Date();
      const formattedDate = currentDate.toLocaleDateString();
      const formattedTime = currentDate.toLocaleTimeString();
      const managerName = "John Doe"; // Replace with actual manager name
      const clientId = order.clientId ? order.clientId : (order.clientName ? order.clientName : "Unknown ID");
      const totalPrice = order.quantity * order.productPrice;
  
      const printWindow = window.open('', '_blank');
      if (!printWindow) {
        alert("Pop-up blocked! Allow pop-ups for this site.");
        return;
      }
  
      printWindow.document.write(`
       <html>
  <head>
    <title>Order Invoice - ${order.id}</title>
  </head>
  <body>
    <!-- Header Section with Company Logo -->
    <div style="text-align: center; padding: 10px;">
      <img src="https://res.cloudinary.com/dpha0wzqx/image/upload/v1739367747/full_photo-removebg-preview_dbmpcf.png" alt="Company Logo" width="150" />
      <h2>Invoice for Order - ${order.id}</h2>
      <p>Generated on ${formattedDate} at ${formattedTime}</p>
    </div>
    
    <!-- Order Information Section -->
    <div style="margin: 20px 0;">
      <h3>Order Information</h3>
      <p><strong>Order ID:</strong> ${order.id}</p>
      <p><strong>Client Name:</strong> ${clientId}</p>
      <p><strong>Managed By:</strong> ${managerName}</p>
      <p><strong>Delivery Destination:</strong> ${order.deliveryDestination || 'N/A'}</p>
    </div>
    
    <!-- Order Details Table -->
    <h3>Order Details</h3>
    <table style="width: 100%; border-collapse: collapse; margin-top: 15px;">
      <thead>
        <tr style="background-color: #f9f9f9;">
          <th style="padding: 10px; border: 1px solid #ddd;">Product Name</th>
          <th style="padding: 10px; border: 1px solid #ddd;">Product ID</th>
          <th style="padding: 10px; border: 1px solid #ddd;">Quantity</th>
          <th style="padding: 10px; border: 1px solid #ddd;">Price per Product</th>
          <th style="padding: 10px; border: 1px solid #ddd;">Total Price</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td style="padding: 10px; border: 1px solid #ddd;">${productName}</td>
          <td style="padding: 10px; border: 1px solid #ddd;">${productId || 'Not Available'}</td>
          <td style="padding: 10px; border: 1px solid #ddd;">${order.quantity}</td>
          <td style="padding: 10px; border: 1px solid #ddd;">$${order.productPrice.toFixed(2)}</td>
          <td style="padding: 10px; border: 1px solid #ddd;">$${totalPrice.toFixed(2)}</td>
        </tr>
      </tbody>
    </table>
    
    <!-- Grand Total -->
    <p style="font-weight: bold; text-align: right; margin-top: 20px;">Grand Total: $${totalPrice.toFixed(2)}</p>
    
    <!-- Signature Section -->
    <div style="margin-top: 50px; display: flex; justify-content: space-between;">
      <div style="width: 45%; text-align: center;">
        <p>Manager Signature</p>
        <hr style="width: 80%; margin: 0 auto;"/>
      </div>
      <div style="width: 45%; text-align: center;">
        <p>Client Signature</p>
        <hr style="width: 80%; margin: 0 auto;"/>
      </div>
    </div>

    <!-- Footer Note -->
    <div style="text-align: center; margin-top: 30px;">
      <p style="font-size: 16px; font-weight: bold;">Thank you for your order!</p>
    </div>

    <!-- Print Script -->
    <script>
      window.onload = function() {
        window.print();
        setTimeout(() => window.close(), 500);
      };
    </script>
  </body>
</html>

      `);
      printWindow.document.close();
    });
  }
  

// Use this flag to control modal visibility
showExportModal: boolean = false;

// Trigger the modal for selecting export format
openExportModal() {
  if (this.selectedOrders.length === 0) {
    alert('Please select orders to export.');
    return;
  }
  else{
  this.showExportModal = true;
  }
}

// Close the modal
closeExportModal() {
  this.showExportModal = false;
}

  
    // Method to export selected orders to CSV
exportToCSV(): void {
  if (this.selectedOrders.length === 0) {
    alert('No orders selected to export!');
    return;
  }

  // Filter the orders to include only the selected ones
  const selectedData = this.orders.filter(order => this.selectedOrders.includes(order.id));

  // Convert to CSV format
  const csvData = this.convertToCSV(selectedData);
  this.downloadCSV(csvData);
}

// Method to export selected orders to Excel
exportToExcel(): void {
  if (this.selectedOrders.length === 0) {
    alert('No orders selected to export!');
    return;
  }

  // Filter the orders to include only the selected ones
  const selectedData = this.orders.filter(order => this.selectedOrders.includes(order.id));

  // Use a library like 'xlsx' to generate an Excel file
  this.exportToExcelLibrary(selectedData); // Placeholder for Excel library
}

// Convert filtered data to CSV format
convertToCSV(data: any[]): string {
  const header = Object.keys(data[0]);
  const rows = data.map(item => header.map(field => item[field]).join(','));
  return [header.join(','), ...rows].join('\n');
}

// Trigger CSV download (client-side)
downloadCSV(csvData: string): void {
  const blob = new Blob([csvData], { type: 'text/csv' });
  const link = document.createElement('a');
  link.href = URL.createObjectURL(blob);
  link.download = 'selected_orders.csv';
  link.click();
}

// Placeholder for exporting to Excel using an Excel library like xlsx
exportToExcelLibrary(data: any[]): void {
  import('xlsx').then(XLSX => {
    const ws = XLSX.utils.json_to_sheet(data);
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Orders');
    XLSX.writeFile(wb, 'selected_orders.xlsx');
  });
}

  
  

  
  
  
  
  clearPage(): void {
    this.isFadingOut = true; 
    setTimeout(() => {
      // Reset search inputs
    this.searchOrderId = null;
    this.searchProductId = null;
    this.searchQuery = '';
    this.searchTerm = '';
  
    // Reset filtered results
    this.filteredOrders = [];
    this.filteredProducts = [];
  
    // Reset dropdown selections (if applicable)
    this.selectedClientId = null;
    this.selectedSalesPersonId = null;
    this.salesPersonId = 0;
  
    // Reset combine search checkbox
    this.combineSearch = false;
  
    // Optionally reset any other displayed orders
    this.showAllOrders = false;
    this.clientOrders = [];
    this.salesPersonsOrders = [];
  
    // Reset active section to default orders view
    this.activeSection = 'orders';
    this.isFadingOut = false; 

    }, 300); 
  
    console.log("Page cleared!");
  }
  

  
  
  
  

  toggleAllOrders(): void {
    this.showAllOrders = !this.showAllOrders;

    if (this.showAllOrders) {
      this.activeSection = 'allOrders';
      this.selectedClientId = null;
      this.selectedSalesPersonId = null;
      this.clientOrders = [];
      this.salesPersonsOrders = [];
    } else {
      this.activeSection = 'orders';
    }
  }
  toggleSidebar(): void {
    this.sidebarCollapsed = !this.sidebarCollapsed;
  }


selectAllChecked: boolean = false;
showDeleteModal: boolean = false;
managerPassword: string = '';

toggleOrderSelection(orderId: number): void {
  const index = this.selectedOrders.indexOf(orderId);
  if (index === -1) {
    this.selectedOrders.push(orderId);
  } else {
    this.selectedOrders.splice(index, 1);
  }
  this.selectAllChecked = this.selectedOrders.length === this.orders.length;
}

toggleSelectAll(): void {
  if (this.selectAllChecked) {
    this.selectedOrders = [];
  } else {
    this.selectedOrders = this.orders.map(order => order.id);
  }
  this.selectAllChecked = !this.selectAllChecked;
}


selectAllOrders(): void {
  this.selectAllChecked = true;
  this.selectedOrders = this.orders.map(order => order.id);
}

confirmDelete(): void {
  if (this.selectedOrders.length === 0) {
    alert('Please select orders to delete.');
    return;
  }
}


cancelDelete(): void {
  this.managerPassword = '';
}

// deleteSelectedOrders(): void {
//   if (this.managerPassword === 'Rodriguez123!') {
//     this.selectedOrders.forEach(orderId => {
//       this.deleteOrder(orderId);
//     });
//     this.selectedOrders = [];
//     this.selectAllChecked = false;
//     this.showDeleteModal = false;
//     this.managerPassword = '';
//   } else {
//     alert('Incorrect password!');
//   }
// }
deleteSelectedOrders(): void {
  if (this.selectedOrders.length === 0) {
    alert('Please select orders to delete.');
    return;
  }

  // Loop through the selected orders and delete them
  this.selectedOrders.forEach(orderId => {
    this.deleteOrder(orderId); // Call the method to delete each order
  });

  // After deletion, reset the selected orders
  this.selectedOrders = [];
  this.selectAllChecked = false; // Uncheck the "Select All" checkbox
  // No modal is involved, so no need to hide the delete modal
}

deleteOrder(orderId: number): void {
  this.orderService.deleteOrder(orderId).subscribe({
    next: () => {
      // Instead of mutating, create a new reference to trigger change detection
      this.orders = this.orders.filter(order => order.id !== orderId);
      this.salesPersonsOrders = [...this.salesPersonsOrders.filter(order => order.id !== orderId)];
      this.clientOrders = [...this.clientOrders.filter(order => order.id !== orderId)];
      this.filteredOrders = [...this.filteredOrders.filter(order => order.id !== orderId)];
      
      // Also update selected orders to reflect that the order is deleted
      this.selectedOrders = this.selectedOrders.filter(id => id !== orderId);

    },
    error: (error) => {
      console.error('Error deleting order:', error);
      alert('Failed to delete the order. Please try again.');
    }
  });
}






}



