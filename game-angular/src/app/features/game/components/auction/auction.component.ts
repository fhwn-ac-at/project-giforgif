import { Component, EventEmitter, inject, Output } from '@angular/core';
import { TileCardComponent } from '../tile-card/tile-card.component';
import { interval, Subscription } from 'rxjs';
import { GameService } from '@shared/services/game/game.service';
import { Tile } from '@shared/types/game/tile';

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
  protected highestBidder = '';
  protected currentBid = 0;
  protected tile: Tile | null = null;

  protected timeLeft = 6;
  private countdownSubscription?: Subscription;

  private readonly gameService = inject(GameService);

  // constructor() {
  //   this.showBuyOption(33);
  //   this.startTimer();
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

  public setHighestBidder(name: string) {
    this.highestBidder = name;
  }

  public setCurrentBid(amount: number) {
    this.currentBid = amount;
  }

  public startTimer() {
    this.stopTimer();
    this.timeLeft = 10;

    this.countdownSubscription = interval(1000).subscribe(() => {
      if (this.timeLeft > 0) {
        this.timeLeft--;
      } else {
        this.stopTimer();
      }
    });
  }

  public resetTimer() {
    this.timeLeft = 10;
  }

  public stopTimer() {
    if (this.countdownSubscription) {
      this.countdownSubscription.unsubscribe();
      this.countdownSubscription = undefined;
    }
  }
}
