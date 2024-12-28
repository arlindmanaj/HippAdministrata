import { Route } from '@angular/router';
import { LoginComponent } from './components/Auth/login/login.component';
import { RegisterComponent } from './components/Auth/register/register.component';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { ManagerDashboardComponent } from './components/manager-dashboard/manager-dashboard.component';
import { ProductDashboardComponent } from './components/product-dashboard/product-dashboard.component';
import { OrderDashboardComponent } from './components/order-dashboard/order-dashboard.component';
import { EmployeeDashboardComponent } from './components/employee-dashboard/employee-dashboard.component';
export const route: Route[] = [
    {
        path: 'login',
        loadComponent: () =>
            import('./components/Auth/login/login.component').then(
                (m) => m.LoginComponent
            ),
    },
    {
        path: 'admin-dashboard',
        loadComponent: () =>
            import('./components/admin-dashboard/admin-dashboard.component').then(
                (m) => m.AdminDashboardComponent
            ),
    },
    {
        path: 'register',
        loadComponent: () =>
            import('./components/Auth/register/register.component').then(
                (m) => m.RegisterComponent
            ),
    },
    {
        path: 'client-dashboard',
        loadComponent: () =>
            import('./components/client-dashboard/client-dashboard.component').then(
                (m) => m.ClientDashboardComponent
            ),
    },
    {
        path: 'manager-dashboard',
        loadComponent: () =>
            import('./components/manager-dashboard/manager-dashboard.component').then(
                (m) => m.ManagerDashboardComponent
            ),
    },
    {
        path: 'manager/products',
        loadComponent: () =>
            import('./components/product-dashboard/product-dashboard.component').then(
                (m) => m.ProductDashboardComponent
            ),
    },
    {
        path: 'manager/orders',
        loadComponent: () =>
            import('./components/order-dashboard/order-dashboard.component').then(
                (m) => m.OrderDashboardComponent
            ),
    },
    
    {
        path: 'salesperson-dashboard',
        loadComponent: () =>
            import('./components/salesperson-dashboard/salesperson-dashboard.component').then(
                (m) => m.SalespersonDashboardComponent
            ),
    },
    {
        path: 'employee-dashboard',
        loadComponent: () =>
            import('./components/employee-dashboard/employee-dashboard.component').then(
                (m) => m.EmployeeDashboardComponent
            )
    },

    { path: '', redirectTo: '/login', pathMatch: 'full' },
];