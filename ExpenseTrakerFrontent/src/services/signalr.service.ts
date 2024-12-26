import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';

export interface AppNotification {
  notificationId: number;
  userId: number;
  notificationMessage: string;
  createdAt: string;
  isRead: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  public notifications: AppNotification[] = [];
  private baseUrl = 'http://localhost:5147/';
  private hubConnection: signalR.HubConnection;
  private notificationsSubject = new BehaviorSubject<AppNotification[]>([]);

  constructor(private http: HttpClient) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.baseUrl + 'notificationHub')
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connection started'))
      .catch((err) => console.error('Error starting SignalR connection:', err));
    this.hubConnection.on('ReceiveNotifications', (data: any) => {
      const notification: AppNotification = {
        notificationId: data.notificationId,
        userId: data.userId,
        notificationMessage: data.notificationMessage,
        createdAt: data.createdAt,
        isRead: data.isRead,
      };
      this.notifications.push(notification);
      this.notificationsSubject.next(this.notifications);
    });
  }

    getNotifications(userId: number, markAsRead: boolean): Observable<AppNotification[]> {
      return this.http.get<AppNotification[]>(`${this.baseUrl}api/notification/${userId}/${markAsRead}`);
  }

  getNotificationsSubject(): Observable<AppNotification[]> {
    return this.notificationsSubject.asObservable();
  }
}
