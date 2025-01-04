import { Component, ViewChild } from '@angular/core';
import { SharedModule } from '../../../shared/shared.module';
import { BoardComponent } from '../components/board/board.component';
import { PlayerCardComponent } from '../components/player-card/player-card.component';
import { DiceComponent } from '../components/dice/dice.component';
import { Packet } from '../../../shared/packets/packet';
import { BuyTileComponent } from '../components/buy-tile/buy-tile.component';
import { AuctionComponent } from '../components/auction/auction.component';
import { EventCardComponent } from '../components/event-card/event-card.component';

@Component({
  selector: 'app-game',
  imports: [
    SharedModule,
    BoardComponent,
    PlayerCardComponent,
    DiceComponent,
    BuyTileComponent,
    AuctionComponent,
    EventCardComponent,
  ],
  templateUrl: './game.component.html',
  styles: ``,
})
export class GameComponent {
  @ViewChild(DiceComponent)
  protected diceComponent!: DiceComponent;

  public handleDicePress() {
    this.diceComponent.startDicing();
    this.handleRolledPacket();
  }

  public handleRolledPacket(packet?: Packet) {
    setTimeout(() => {
      this.diceComponent.setDiced(3, {
        name: 'y<a',
        currency: 2,
        color: 'blue',
      });
    }, 1000);
  }
}
