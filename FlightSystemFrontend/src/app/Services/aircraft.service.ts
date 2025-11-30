import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

export interface AircraftGet {
  id: number;
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

  private apiUrl = 'https://localhost:7251/api/Aircraft/get-all'; // prilagodi tvoj URL

  constructor(private http: HttpClient) {}

  getAll(): Observable<AircraftGet[]> {
    return this.http.get<AircraftGet[]>(`${this.apiUrl}`);
  }

}
