import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ReservationDTO {
  id: number;
  userId: number;
  userFullName: string;
  flightId: number;
  flightNumber: string;
  destination: string;
  seatNumber: string;
  additionalBaggageId?: number;
  additionalBaggageType?: string;
  totalPrice: number;
  status: number; // enum
  createdAt: string;
}

export interface ReservationAddDTO {
  userId: number;
  flightId: number;
  seatNumber: string;
  additionalBaggageId?: number;
  status: number;
}

export interface ReservationUpdateDTO {
  id: number;
  seatNumber?: string;
  additionalBaggageId?: number;
  status?: number;
}

@Injectable({
  providedIn: 'root'
})
export class ReservationService {
  private apiUrl = 'https://localhost:7251/api/Reservation';

  constructor(private http: HttpClient) {}

  getAll(): Observable<ReservationDTO[]> {
    return this.http.get<ReservationDTO[]>(this.apiUrl);
  }

  getById(id: number): Observable<ReservationDTO> {
    return this.http.get<ReservationDTO>(`${this.apiUrl}/${id}`);
  }

  create(dto: ReservationAddDTO): Observable<ReservationDTO> {
    return this.http.post<ReservationDTO>(this.apiUrl, dto);
  }

  update(dto: ReservationUpdateDTO): Observable<ReservationDTO> {
    return this.http.put<ReservationDTO>(this.apiUrl, dto);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
