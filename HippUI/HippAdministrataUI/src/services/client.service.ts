import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment/environment.component.spec';

@Injectable({
  providedIn: 'root'
})
export class ClientService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getProducts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/products`);
  }

  getOrders(): Observable<any[]> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('authToken')}`);
    return this.http.get<any[]>(`${this.apiUrl}/order`, { headers });
  }

  createOrder(clientId: number, order: any): Observable<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('authToken')}`);
    return this.http.post(`${this.apiUrl}/clients/${clientId}/orders`, order, { headers });
  }
  
  getOrdersByClientId(clientId: number): Observable<any[]> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('authToken')}`);
    return this.http.get<any[]>(`${this.apiUrl}/Order/client/${clientId}`, { headers });
  }
  createMultipleOrders(clientId: number, orders: any): Observable<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('authToken')}`);
    return this.http.post(`${this.apiUrl}/clients/${clientId}/orders`, orders, { headers });

  }
  

  getClientIdByUserId(userId: number): Observable<number> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('authToken')}`);
    return this.http.get<number>(`${this.apiUrl}/clients/user/${userId}`, { headers });
  }
  createOrderRequest(request: { orderId: number; clientId: number; requestType: string; reason: string }): Observable<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('authToken')}`);
    return this.http.post(`${this.apiUrl}/order/request`, request, { headers });
  }
    // This method will make an API call to get the clientId based on the userId


}
