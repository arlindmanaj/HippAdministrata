// order.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment/environment.component.spec';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getOrders(): Observable<any[]> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('authToken')}`);
    return this.http.get<any[]>(`${this.apiUrl}/Order`, { headers });
  }
  deleteOrder(orderId: number): Observable<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('authToken')}`);
    // Add responseType: 'text' to handle the plain text response from the backend
    return this.http.delete(`${this.apiUrl}/Order/${orderId}`, { headers, responseType: 'text' });
  }
  requestOrder(orderId: number, clientId: number, requestType: string, reason: string): Observable<any> {
    const requestData = {
      orderId: orderId,
      clientId: clientId,
      requestType: requestType,
      reason: reason
    };

    return this.http.post('https://localhost:7136/api/Order/request', requestData);
  }
}
