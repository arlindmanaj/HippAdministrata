import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment/environment.component';


@Injectable({
  providedIn: 'root'
})
export class ManagerService {
  private apiUrl = 'https://localhost:7136/api/Order'; // Replace with your actual API URL

  constructor(private http: HttpClient) {}

  getOrderRequests(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/pending-requests`);
  }

  approveRequest(requestId: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/approve-request/${requestId}`, {});
  }

  // rejectRequest(requestId: number): Observable<void> {
  //   return this.http.post<void>(`${this.apiUrl}/reject-request/${requestId}`, {});
  // }
  rejectRequest(requestId: number, reason: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/reject-request/${requestId}`, { reason });
  }
}
