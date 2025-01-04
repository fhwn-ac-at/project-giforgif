import { Component, Input } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { Tile } from '../../../../shared/types/game/tile';

@Component({
  selector: 'app-tile-card',
  imports: [SharedModule],
  templateUrl: './tile-card.component.html',
  styles: ``
})
export class TileCardComponent {
  @Input()
  public tile: Tile | null = null;
}
