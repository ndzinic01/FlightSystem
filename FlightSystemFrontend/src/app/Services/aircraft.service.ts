import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AircraftDTO {
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
  providedIn: 'root'
})
export class AircraftService {
  private apiUrl = 'https://localhost:7251/api/Aircraft';

  constructor(private http: HttpClient) {}

  getAll(): Observable<AircraftDTO[]> {
    return this.http.get<AircraftDTO[]>(`${this.apiUrl}/get-all`);
  }

  getById(id: number): Observable<AircraftDTO> {
    return this.http.get<AircraftDTO>(`${this.apiUrl}/get-by-id/${id}`);
  }

  create(dto: AircraftAddUpdateDTO): Observable<AircraftDTO> {
    return this.http.post<AircraftDTO>(`${this.apiUrl}/create`, dto);
  }

  update(id: number, dto: AircraftAddUpdateDTO): Observable<AircraftDTO> {
    return this.http.put<AircraftDTO>(`${this.apiUrl}/update/${id}`, dto);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }
}
