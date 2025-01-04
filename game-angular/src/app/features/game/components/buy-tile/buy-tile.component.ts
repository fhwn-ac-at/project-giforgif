import { Component, EventEmitter, inject, Output } from '@angular/core';
import { GameService } from '../../../../shared/services/game/game.service';
import { Tile } from '../../../../shared/types/game/tile';
import { SharedModule } from '../../../../shared/shared.module';
import { TileCardComponent } from '../tile-card/tile-card.component';

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
  private readonly gameService = inject(GameService);

  // constructor() {
  //   this.showBuyOption(4);
  // }

  public showBuyOption(index: number) {
    this.visible = true;
    this.tile = this.gameService.tiles.get(index)!;
  }

  public close() {
    this.visible = false;
  }
}
