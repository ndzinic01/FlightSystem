import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface FlightDTO {
  id: number;
  code: string;
  destination: string;
  airline: string;
  aircraft: string;
  departureTime: string;
  arrivalTime: string;
  status: number | string; // 🔥 Može biti i broj i string
  price: number;
  availableSeats: number;
}

export interface FlightCreateDTO {
  code: string;
  destinationId: number;
  airlineId: number;
  aircraftId: number;
  departureTime: string;
  arrivalTime: string;
  status: number; // 🔥 Backend prima broj (enum)
  price: number;
  availableSeats: number;
}

export interface FlightUpdateDTO {
  code: string;
  destinationId: number;
  airlineId: number;
  aircraftId: number;
  departureTime: string;
  arrivalTime: string;
  status: number; // 🔥 Backend prima broj (enum)
  price: number;
  availableSeats: number;
}

@Injectable({
  providedIn: 'root'
})
export class FlightService {
  private apiUrl = 'https://localhost:7251/api/Flight';

  constructor(private http: HttpClient) {}

  getAll(): Observable<FlightDTO[]> {
    return this.http.get<FlightDTO[]>(this.apiUrl);
  }

  getById(id: number): Observable<FlightDTO> {
    return this.http.get<FlightDTO>(`${this.apiUrl}/${id}`);
  }

  create(dto: FlightCreateDTO): Observable<FlightDTO> {
    return this.http.post<FlightDTO>(this.apiUrl, dto);
  }

  update(id: number, dto: FlightUpdateDTO): Observable<FlightDTO> {
    return this.http.put<FlightDTO>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getByDestination(destinationId: number): Observable<FlightDTO[]> {
    return this.http.get<FlightDTO[]>(`${this.apiUrl}/by-destination/${destinationId}`);
  }
}
