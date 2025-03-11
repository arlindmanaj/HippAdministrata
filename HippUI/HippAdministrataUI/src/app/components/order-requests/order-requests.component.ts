import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-order-requests',
  templateUrl: './order-requests.component.html',
  styleUrls: ['./order-requests.component.css'],
  imports:[CommonModule]
})
export class OrderRequestsComponent implements OnInit {
  orderRequests: any[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getOrderRequests();
  }

  getOrderRequests() {
    this.http.get<any[]>('https://localhost:7136/api/Order/pending-requests')
      .subscribe((data) => {
        this.orderRequests = data;
      });
  }

  approveRequest(requestId: number) {
    this.http.post(`https://localhost:7136/api/Order/approve/${requestId}`, {})
      .subscribe(() => this.getOrderRequests());
  }

  rejectRequest(requestId: number) {
    this.http.post(`https://localhost:7136/api/Order/reject/${requestId}`, {})
      .subscribe(() => this.getOrderRequests());
  }
}
