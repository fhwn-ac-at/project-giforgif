import { Component, inject, Input } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { Tile } from '../../../../shared/types/game/tile';
import { GameService } from '../../../../shared/services/game/game.service';

@Component({
  selector: 'app-tile-card',
  imports: [SharedModule],
  templateUrl: './tile-card.component.html',
  styles: ``
})
export class TileCardComponent {
  @Input()
  public tile: Tile | null = null;

  private readonly gameService = inject(GameService);

  protected theme = this.gameService.theme;
}
