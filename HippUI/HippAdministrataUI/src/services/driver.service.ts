import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment/environment.component.spec';

@Injectable({
  providedIn: 'root',
})
export class DriverService {
  private apiUrl = environment.apiUrl; // Replace with your backend API base URL

  constructor(private http: HttpClient) { }

  simulateShipping(driverId: number, orderId: number): Observable<any> {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${localStorage.getItem('authToken')}`
    );
    return this.http.post(
      `${this.apiUrl}/driver/simulate-shipping/${orderId}`,
      {}, // Empty body
      {
        headers,
        params: { driverId: driverId.toString() },
      }
    );
  }

  transferProduct(
    productId: number,
    sourceWarehouseId: number,
    destinationWarehouseId: number
  ): Observable<any> {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${localStorage.getItem('authToken')}`
    );
    const payload = {
      sourceWarehouseId,
      destinationWarehouseId,
    };
    return this.http.post(
      `${this.apiUrl}/Driver/${productId}/transfer`,
      payload,
      { headers }
    );
  }

  getDriverAssignedOrders(driverId: number): Observable<any[]> {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${localStorage.getItem('authToken')}`
    );
    return this.http.get<any[]>(
      `${this.apiUrl}/Driver/assigned-orders/${driverId}`,
      { headers, params: { driverId: driverId.toString() } }
    );
  }
}
