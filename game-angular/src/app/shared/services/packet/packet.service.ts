import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PacketService {
  private hubConnection: signalR.HubConnection;

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/lobby`)
      .build();
  }

  startConnection(): Observable<void> {
    return new Observable<void>((observer) => {
      this.hubConnection
        .start()
        .then(() => {
          console.log('Connection established with SignalR hub');
          observer.next();
          observer.complete();
        })
        .catch((error) => {
          console.error('Error connecting to SignalR hub:', error);
          observer.error(error);
        });
    });
  }

  receiveMessage(): Observable<string> {
    return new Observable<string>((observer) => {
      this.hubConnection.on('ReceivePacket', (message: string) => {
        observer.next(message);
      });
    });
  }

  sendMessage(message: string): void {
    this.hubConnection.invoke('SendPacketToServer', message);
  }

  tgest(message: string): void {
    this.hubConnection.invoke('JoinRoom', message);
  }
}
