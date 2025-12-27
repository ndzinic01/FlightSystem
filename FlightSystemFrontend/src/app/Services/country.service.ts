import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Country {
  id: number;
  name: string;
}

export interface CountryAddUpdate {
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class CountryService {
  private apiUrl = 'https://localhost:7251/api/Country';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Country[]> {
    return this.http.get<Country[]>(`${this.apiUrl}/get-all`);
  }

  create(dto: CountryAddUpdate): Observable<Country> {
    return this.http.post<Country>(`${this.apiUrl}`, dto);
  }

  update(id: number, dto: CountryAddUpdate): Observable<Country> {
    return this.http.put<Country>(`${this.apiUrl}/update/${id}`, dto);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }
}

