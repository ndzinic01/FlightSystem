import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export enum NotificationType {
  UserInquiry = 0,
  FlightDelay = 1,
  FlightCancellation = 2,
  FlightReschedule = 3,
  SystemBroadcast = 4
}

export interface NotificationDTO {
  id: number;
  userId?: number;
  userFullName?: string;
  userEmail?: string;
  flightId?: number;
  flightCode?: string;
  type: NotificationType;
  message: string;
  sentAt: string;
  isRead: boolean;
  adminReply?: string;
  isSystemGenerated: boolean;
}

export interface NotificationReplyDTO {
  id: number;
  reply: string;
}

export interface BroadcastNotificationDTO {
  message: string;
  flightId?: number;
}

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private apiUrl = 'https://localhost:7251/api/Notification';

  constructor(private http: HttpClient) {}

  getAll(): Observable<NotificationDTO[]> {
    return this.http.get<NotificationDTO[]>(this.apiUrl);
  }

  markAsRead(id: number): Observable<any> {
    return this.http.put(this.apiUrl, { id, isRead: true });
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  reply(data: NotificationReplyDTO): Observable<any> {
    return this.http.post(`${this.apiUrl}/reply`, data);
  }

  // 🔥 NOVE METODE
  broadcast(data: BroadcastNotificationDTO): Observable<any> {
    return this.http.post(`${this.apiUrl}/broadcast`, data);
  }

  notifyFlightCancellation(flightId: number, reason: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/flight-cancelled/${flightId}`, JSON.stringify(reason), {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  notifyFlightReschedule(flightId: number, newDepartureTime: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/flight-rescheduled/${flightId}`, JSON.stringify(newDepartureTime), {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  notifyFlightDelay(flightId: number, delayMinutes: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/flight-delayed/${flightId}`, delayMinutes);
  }
}
