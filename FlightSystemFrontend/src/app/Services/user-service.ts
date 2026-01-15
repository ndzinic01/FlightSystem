import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface UserDTO {
  id: number;
  firstName: string;
  lastName: string;
  fullName: string;
  username: string;
  email: string;
  phoneNumber: string;
  role: string;
  isActive: boolean;
  isDeleted: boolean;
}
@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = 'https://localhost:7251/api/User';

  constructor(private http: HttpClient) {}

  getAll(): Observable<UserDTO[]> {
    return this.http.get<UserDTO[]>(this.apiUrl);
  }

  update(dto: any) {
    return this.http.put(this.apiUrl, dto);
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
