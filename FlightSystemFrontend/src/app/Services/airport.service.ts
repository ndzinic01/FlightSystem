import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AirportDTO {
  id: number;
  name: string;
  cityId: number;
  cityName: string;
  isActive: boolean;
  code: string;
}

export interface AirportAddUpdate {
  name: string;
  cityId: number;
  isActive: boolean;
  code: string;
}

@Injectable({
  providedIn: 'root',
})
export class AirportService {
  private baseUrl = 'https://localhost:7251/api/Airport';

  constructor(private http: HttpClient) {}

  getAll(): Observable<AirportDTO[]> {
    return this.http.get<AirportDTO[]>(`${this.baseUrl}/get-all`);
  }

  // 🆕 NOVI METOD
  getByCityId(cityId: number): Observable<AirportDTO[]> {
    return this.http.get<AirportDTO[]>(`${this.baseUrl}/get-by-city/${cityId}`);
  }

  getById(id: number): Observable<AirportDTO> {
    return this.http.get<AirportDTO>(`${this.baseUrl}/get-by-id/${id}`);
  }

  create(airport: AirportAddUpdate): Observable<AirportDTO> {
    return this.http.post<AirportDTO>(`${this.baseUrl}/create`, airport);
  }

  update(id: number, airport: AirportAddUpdate): Observable<AirportDTO> {
    return this.http.put<AirportDTO>(`${this.baseUrl}/update/${id}`, airport);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/delete/${id}`);
  }
}
