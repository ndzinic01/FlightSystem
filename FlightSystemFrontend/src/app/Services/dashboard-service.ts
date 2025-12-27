import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


export interface DashboardStats{
  totalFlights: number;
  activeUsers: number;
  todayReservations: number;
  flightStatuses: {
    [key: string]: number;
  };
  monthlyReservations: {
    [key: number]: number; // dan u mjesecu -> broj rezervacija
  };
}

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  private apiUrl = 'https://localhost:7251/api/dashboard/stats';

  constructor(private http: HttpClient) {}

  getStats(): Observable<DashboardStats> {
    return this.http.get<DashboardStats>(this.apiUrl);
  }
}

