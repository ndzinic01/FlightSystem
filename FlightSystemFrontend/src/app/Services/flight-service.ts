import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FlightService {

  private apiUrl = 'https://localhost:7251/api';
  // ⬆️ promijeni port ako ti je drugačiji

  constructor(private http: HttpClient) {}

  getByDestination(destinationId: number): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.apiUrl}/Flight/by-destination/${destinationId}`
    );
  }
}
