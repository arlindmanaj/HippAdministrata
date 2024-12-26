import { Route } from '@angular/router';
import { LoginComponent } from './components/Auth/login/login.component';
import { RegisterComponent } from './components/Auth/register/register.component';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
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
    { path: '', redirectTo: '/login', pathMatch: 'full' },
];