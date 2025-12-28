import { Injectable } from '@angular/core';
import {Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';

export interface DestinationDTO {
  id: number;
  fromCity: string;
  toCity: string;
  fromAirportCode: string;
  toAirportCode: string;
  isActive: boolean;
}


@Injectable({
  providedIn: 'root',
})
export class DestinationService {
  private apiUrl = 'https://localhost:7251/api/Destination';

  constructor(private http: HttpClient) {}

  getAll(): Observable<DestinationDTO[]> {
    return this.http.get<DestinationDTO[]>(this.apiUrl);
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
