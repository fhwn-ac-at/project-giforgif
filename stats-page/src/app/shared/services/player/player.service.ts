import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Player } from '../../types/player/player';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class PlayerService {
  constructor(private readonly http: HttpClient) {}

  public getPlayers(): Observable<Player[]> {
    return this.http.get<Player[]>(`${environment.apiUrl}/stats`);
  }
}
