import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment/environment.component';


@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  labelProduct(employeeId: number, labelingOrder: any): Observable<void> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('authToken')}`).set('employeeId', employeeId.toString());
    return this.http.post<void>(`${this.apiUrl}/employees/label`, labelingOrder, { headers });
  }
  getAssignedOrders(employeeId: number): Observable<any[]> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('authToken')}`).set('employeeId', employeeId.toString());
    return this.http.get<any[]>(`${this.apiUrl}/employees/orders/${employeeId}`, { headers });
  }

}
