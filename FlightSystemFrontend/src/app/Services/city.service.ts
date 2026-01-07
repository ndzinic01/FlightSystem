import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface City {
  id: number;
  name: string;
  countryId: number;
  countryName: string;
}

export interface CityAddUpdate {
  name: string;
  countryId: number;
}

@Injectable({
  providedIn: 'root'
})
export class CityService {
  private apiUrl = 'https://localhost:7251/api/City';

  constructor(private http: HttpClient) {}

  getAll(): Observable<City[]> {
    return this.http.get<City[]>(`${this.apiUrl}/get-all`);
  }

  // 🆕 NOVI METOD
  getByCountryId(countryId: number): Observable<City[]> {
    return this.http.get<City[]>(`${this.apiUrl}/get-by-country/${countryId}`);
  }

  create(dto: CityAddUpdate): Observable<City> {
    return this.http.post<City>(`${this.apiUrl}`, dto);
  }

  update(id: number, dto: CityAddUpdate): Observable<City> {
    return this.http.put<City>(`${this.apiUrl}/update/${id}`, dto);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }
}

