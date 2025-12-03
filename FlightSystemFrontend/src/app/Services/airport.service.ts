import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


export interface Airport {
  id: number;
  name: string;
  cityId: number;
  cityName: string;
  isActive: boolean;
}

export interface AirportAddUpdate {
  name: string;
  cityId: number;
  isActive: boolean;
}


@Injectable({
  providedIn: 'root',
})
export class AirportService {
  private baseUrl = 'https://localhost:7251/api/Airport';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Airport[]> {
    return this.http.get<Airport[]>(`${this.baseUrl}/get-all`);
  }

  getById(id: number): Observable<Airport> {
    return this.http.get<Airport>(`${this.baseUrl}/get-by-id/${id}`);
  }

  create(airport: AirportAddUpdate): Observable<Airport> {
    return this.http.post<Airport>(`${this.baseUrl}/create`, airport);
  }

  update(id: number, airport: AirportAddUpdate): Observable<Airport> {
    return this.http.put<Airport>(`${this.baseUrl}/update/${id}`, airport);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/delete/${id}`);
  }
}
