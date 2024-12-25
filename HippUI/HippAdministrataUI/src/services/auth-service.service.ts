import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment/environment.component.spec';
import { RegisterRequest } from '../models/register-request.model';
import { HttpHeaders } from '@angular/common/http';


@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}`;
  private tokenKey = 'authToken';
  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('authToken');
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
  }

  constructor(private http: HttpClient) { }

  saveToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  // Retrieve the token from localStorage
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  // Remove the token (logout)
  removeToken(): void {
    localStorage.removeItem(this.tokenKey);
  }

  // Decode the token (requires a JWT decoding library like `jwt-decode`)
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


  // Check if the user is authenticated
  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) return false;

    const payload = this.decodeToken();
    if (!payload) return false;

    const currentTime = Math.floor(new Date().getTime() / 1000);
    return payload.exp > currentTime; // Check if token is expired
  }

  // Check if the user is an Admin
  isAdmin(): boolean {
    const payload = this.decodeToken();
    return payload && payload.role === 'Admin';
  }

  getUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Users`);
  }

  // Delete user
  deleteUser(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }

  // Login
  login(name: string, password: string): Observable<{ token: string; role: string }> {
    const body = { name, password };
    return this.http.post<{ token: string; role: string }>(`${this.apiUrl}/Auth/login`, body);
  }

  // Register User
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

  // Register Manager
  registerManager(name: string, password: string): Observable<any> {
    const body = { name, password };
    return this.http.post(`${this.apiUrl}/Auth/register/manager`, body, { headers: this.getHeaders() });
  }

  // Register SalesPerson
  registerSalesPerson(name: string, password: string, location: string): Observable<any> {
    const body = { username: name, password, location };
    return this.http.post(`${this.apiUrl}/Auth/register/salesperson`, body, { headers: this.getHeaders() });
  }
}
