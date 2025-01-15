import { Component, EventEmitter, inject, Output } from '@angular/core';
import { TileCardComponent } from '../tile-card/tile-card.component';
import { GameService } from '@shared/services/game/game.service';
import { SharedModule } from '@shared/shared.module';
import { Tile } from '@shared/types/game/tile';

@Component({
  selector: 'app-buy-tile',
  imports: [SharedModule, TileCardComponent],
  templateUrl: './buy-tile.component.html',
  styles: ``,
})
export class BuyTileComponent {
  @Output()
  public buyTile: EventEmitter<number> = new EventEmitter();

  @Output()
  public openAuction: EventEmitter<number> = new EventEmitter();

  protected visible = false;
  protected tile: Tile | null = null;
  public gameService = inject(GameService);

  public showBuyOption(index: number) {
    this.visible = true;
    this.tile = this.gameService.tiles.get(index)!;
  }

  public close() {
    this.visible = false;
  }
}
