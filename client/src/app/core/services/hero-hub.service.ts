import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { API_BASE_URL, BASE_URL } from '../../app.config';
import { Hero } from '../models/hero.model';

@Injectable({ providedIn: 'root' })
export class HeroHubService {
  private hubConnection!: signalR.HubConnection;
  public heroChanged$ = new Subject<Hero>();

  startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${BASE_URL}/heroHub`)
      .build();

    this.hubConnection
      .start()
      .catch((err) => console.error('SignalR Connection Error:', err));

    this.hubConnection.on('HeroChanged', (hero: Hero) => {
      this.heroChanged$.next(hero);
    });
  }
}
