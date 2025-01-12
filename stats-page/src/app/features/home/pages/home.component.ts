import { Component } from '@angular/core';
import { Perform } from '../../../shared/classes';
import { Player } from '../../../shared/types/player/player';
import { PlayerService } from '../../../shared/services/player/player.service';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.component.html',
  styles: ``,
})
export class HomeComponent {
  protected playerPerform = new Perform<Player[]>();

  constructor(private readonly playerService: PlayerService) {
    this.playerPerform.load(this.playerService.getPlayers(), {
      toast: {
        info: 'Erfolgreich Statistik über Spieler gefetched',
        error: 'Ein Fehler ist aufgetreten, bitte versuche es erneut',
      },
    });
  }
}
