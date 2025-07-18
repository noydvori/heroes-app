import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { API_BASE_URL } from '../../app.config';

@Injectable({ providedIn: 'root' })
export class HeroHubService {
  private hubConnection!: signalR.HubConnection;
  private heroChangedSource = new Subject<void>();

  heroChanged$ = this.heroChangedSource.asObservable();

  startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5026/heroHub')

      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('✅ SignalR connected'))
      .catch((err) => console.error('❌ SignalR error:', err));

    this.hubConnection.on('HeroListUpdated', () => {
      console.log('📡 Hero list update received!');
      this.heroChangedSource.next();
    });
  }
}
