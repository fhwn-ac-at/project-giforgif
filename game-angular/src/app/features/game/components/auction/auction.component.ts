import { Component, EventEmitter, inject, Output } from '@angular/core';
import { TileCardComponent } from '../tile-card/tile-card.component';
import { GameService } from '../../../../shared/services/game/game.service';
import { Tile } from '../../../../shared/types/game/tile';

@Component({
  selector: 'app-auction',
  imports: [TileCardComponent],
  templateUrl: './auction.component.html',
  styles: ``,
})
export class AuctionComponent {
  @Output()
  public onBid: EventEmitter<number> = new EventEmitter();

  protected visible = false;
  protected disabler = false;
  protected tile: Tile | null = null;

  private readonly gameService = inject(GameService);

  // constructor() {
  //   this.showBuyOption(33);
  // }

  protected noInterest() {
    this.disabler = true;
  }

  public showBuyOption(index: number) {
    this.visible = true;
    this.tile = this.gameService.tiles.get(index)!;
  }

  public close() {
    this.visible = false;
  }
}
