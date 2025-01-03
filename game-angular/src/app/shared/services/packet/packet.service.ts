import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../../environments/environment';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PacketService {

  private hubConnection: signalR.HubConnection;
  private connectionStatus = new BehaviorSubject<boolean>(false);

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/lobby`)
      .build();
  }

  startConnection(): Observable<void> {
    this.hubConnection.onclose(() => {
      console.warn('SignalR connection closed');
      this.connectionStatus.next(false);
    });

    return new Observable<void>((observer) => {
      this.hubConnection
        .start()
        .then(() => {
          console.log('Connection established with SignalR hub');
          this.connectionStatus.next(true);
          observer.next();
          observer.complete();
        })
        .catch((error) => {
          this.connectionStatus.next(false);
          console.error('Error connecting to SignalR hub:', error);
          observer.error(error);
        });
    });
  }

  getConnectionStatus(): Observable<boolean> {
    return this.connectionStatus.asObservable();
  }

  receiveMessage(): Observable<string> {
    return new Observable<string>((observer) => {
      this.hubConnection.on('ReceivePacket', (message: string) => {
        observer.next(message);
      });
    });
  }

  sendPacket<T>(packet: T): void {
    this.hubConnection.invoke('SendPacketToServer', JSON.stringify(packet));
  }

  joinRoom(message: string): void {
    this.hubConnection.invoke('JoinRoom', message);
  }
}
