import { Component, Input } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { Player } from '@shared/types/game/player';

@Component({
  selector: 'app-player-card',
  imports: [SharedModule],
  templateUrl: './player-card.component.html',
  styles: ``,
})
export class PlayerCardComponent {
  @Input()
  public player!: Player;
}
