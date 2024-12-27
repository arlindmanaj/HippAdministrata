import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment/environment.component.spec';
import { ClientService } from './client.service';
import { ProductService } from './product.service';

@Injectable({
  providedIn: 'root'
})
export class SalesPersonService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient, private clientService: ClientService, private productService: ProductService) { }

  getOrdersBySalesPersonId(salesPersonId: number): Observable<any[]> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('authToken')}`);
    return this.http.get<any[]>(`${this.apiUrl}/SalesPerson/orders?salesPersonId=${salesPersonId}`, { headers });
  }




  assignOrder(orderId: number, assignment: any): Observable<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('authToken')}`);
    return this.http.put(`${this.apiUrl}/SalesPerson/orders/${orderId}/assign`, assignment, { headers });
  }

  updateOrder(orderId: number, assignment: any): Observable<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('authToken')}`);
    return this.http.put(`${this.apiUrl}/SalesPerson/orders/${orderId}/update-assignment`, assignment, { headers });
  }



}
