import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { CreateRoom, Room } from '../../types';

@Injectable({
  providedIn: 'root',
})
export class RoomService {
  constructor(private readonly http: HttpClient) {}

  public getRooms(): Observable<Room[]> {
    return this.http.get<Room[]>(`${environment.apiUrl}/room`);
  }

  public createRoom(dto: CreateRoom): Observable<Room> {
    return this.http.post<Room>(`${environment.apiUrl}/room`, dto);
  }
}
