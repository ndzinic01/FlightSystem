import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AirlineDTO {
  id: number;
  name: string;
  logoURL?: string;
}

export interface AirlineAddUpdateDTO {
  name: string;
  logoURL?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AirlineService {
  private apiUrl = 'https://localhost:7251/api/Airline';

  constructor(private http: HttpClient) {}

  getAll(): Observable<AirlineDTO[]> {
    return this.http.get<AirlineDTO[]>(this.apiUrl);
  }

  getById(id: number): Observable<AirlineDTO> {
    return this.http.get<AirlineDTO>(`${this.apiUrl}/${id}`);
  }

  create(dto: AirlineAddUpdateDTO): Observable<AirlineDTO> {
    return this.http.post<AirlineDTO>(this.apiUrl, dto);
  }

  update(id: number, dto: AirlineAddUpdateDTO): Observable<AirlineDTO> {
    return this.http.put<AirlineDTO>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
