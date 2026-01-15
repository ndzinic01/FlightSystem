import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface NotificationDTO {
  id: number;
  userId: number;
  userFullName: string;
  userEmail: string;
  flightCode: string;
  message: string;
  sentAt: string;
  isRead: boolean;
  adminReply?: string;
}

export interface NotificationReplyDTO {
  id: number;
  reply: string;
}

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private apiUrl = 'https://localhost:7251/api/Notification';

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<NotificationDTO[]>(this.apiUrl);
  }

  markAsRead(id: number): Observable<any> {
    return this.http.put(this.apiUrl, { id, isRead: true });
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  reply(data: NotificationReplyDTO) {
    return this.http.post(`${this.apiUrl}/reply`, data);
  }
}
