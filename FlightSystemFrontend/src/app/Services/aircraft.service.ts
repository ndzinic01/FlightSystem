import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

export interface AircraftGetDTO {
  id: number;
  model: string;
  registrationNumber: string;
  yearManufacturer: number;
  manufacturer: string;
  capacity: number;
  status: boolean;
}

export interface AircraftAddUpdateDTO {
  model: string;
  registrationNumber: string;
  yearManufacturer: number;
  manufacturer: string;
  capacity: number;
  status: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class AircraftService {

  private apiUrl = 'https://localhost:7251/api/Aircraft'; // prilagodi tvoj URL

  constructor(private http: HttpClient) {}

  getAll(): Observable<AircraftGetDTO[]> {
    return this.http.get<AircraftGetDTO[]>(`${this.apiUrl}/get-all`);
  }

  getById(id: number): Observable<AircraftGetDTO> {
    return this.http.get<AircraftGetDTO>(`${this.apiUrl}/get-by-id/${id}`);
  }

  create(aircraft: AircraftAddUpdateDTO): Observable<AircraftGetDTO> {
    return this.http.post<AircraftGetDTO>(`${this.apiUrl}/create`, aircraft);
  }

  update(id: number, aircraft: AircraftAddUpdateDTO): Observable<AircraftGetDTO> {
    return this.http.put<AircraftGetDTO>(`${this.apiUrl}/update/${id}`, aircraft);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }
}
