import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment/environment.component.spec';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}`;
  private tokenKey = 'authToken';

  constructor(private http: HttpClient) { }

  // Helper to get headers with token
  private getHeaders(): HttpHeaders {
    const token = this.getToken();
    if (!token) {
      console.warn('No token found for authenticated requests.');
      return new HttpHeaders();
    }
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });
  }

  // Save token to localStorage
  saveToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  // Retrieve token from localStorage
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  // Remove token from localStorage
  removeToken(): void {
    localStorage.removeItem(this.tokenKey);
  }

  // Decode token to get payload
  decodeToken(): any {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload;
    } catch (error) {
      console.error('Error decoding token', error);
      return null;
    }
  }

  // Check if user is authenticated
  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) return false;

    const payload = this.decodeToken();
    if (!payload) return false;

    const currentTime = Math.floor(new Date().getTime() / 1000);
    return payload.exp > currentTime; // Token expiration check
  }

  // Check if user is an Admin
  isAdmin(): boolean {
    const payload = this.decodeToken();
    return payload && payload.role === 'Admin';
  }

  // Get all users
  getUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Users`, { headers: this.getHeaders() });
  }

  // Delete a user by ID
  deleteUser(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Users/${id}`, { headers: this.getHeaders() });
  }

  // Login
  login(name: string, password: string): Observable<{ token: { token: string }; role: string; roleSpecificId: number }> {
    const body = { name, password };
    return this.http.post<{ token: { token: string }; role: string; roleSpecificId: number }>(`${this.apiUrl}/Auth/login`, body);
  }



  // General user registration
  registerUser(name: string, password: string, email?: string): Observable<any> {
    const body = { name, password, email };
    return this.http.post(`${this.apiUrl}/Auth/register`, body);

  }

  // Register Driver
  registerDriver(name: string, password: string, email: string, licensePlate: string, carModel: string): Observable<any> {
    const body = { name, password, email, licensePlate, carModel };
    return this.http.post(`${this.apiUrl}/Auth/register/driver`, body, { headers: this.getHeaders() });
  }

  // Register Employee
  registerEmployee(name: string, password: string): Observable<any> {
    const body = { name, password };
    return this.http.post(`${this.apiUrl}/Auth/register/employee`, body, { headers: this.getHeaders() });
  }

  // Register SalesPerson
  registerSalesPerson(name: string, password: string, email: string): Observable<any> {
    const body = { name, password, email };
    console.log('Calling API to register SalesPerson:', body); // Debugging log
    return this.http.post(`${this.apiUrl}/Auth/register/salesperson`, body, { headers: this.getHeaders() });
  }

  // Register Manager
  registerManager(name: string, password: string, email: string): Observable<any> {
    const body = { name, password, email };
    console.log('Calling API to register Manager:', body); // Debugging log
    return this.http.post(`${this.apiUrl}/Auth/register/manager`, body, { headers: this.getHeaders() });
  }
}
