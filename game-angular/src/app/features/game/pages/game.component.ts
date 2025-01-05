import { Component, inject, ViewChild } from '@angular/core';
import { SharedModule } from '../../../shared/shared.module';
import { BoardComponent } from '../components/board/board.component';
import { PlayerCardComponent } from '../components/player-card/player-card.component';
import { DiceComponent } from '../components/dice/dice.component';
import { Packet } from '../../../shared/packets/packet';
import { BuyTileComponent } from '../components/buy-tile/buy-tile.component';
import { AuctionComponent } from '../components/auction/auction.component';
import { EventCardComponent } from '../components/event-card/event-card.component';
import { NextTurnComponent } from '../components/next-turn/next-turn.component';
import { RolledDicePacket } from '../../../shared/packets/game/dice/rolled-dice';
import { GameService } from '../../../shared/services/game/game.service';
import { PacketService } from '../../../shared/services/packet/packet.service';
import { RollDicePacket } from '../../../shared/packets/game/dice/roll-dice';
import { PlayersTurnPacket } from '../../../shared/packets/game/util/player-turn';
import { GameStatePacket } from '../../../shared/packets/game/state';

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
    NextTurnComponent,
  ],
  templateUrl: './game.component.html',
  styles: ``,
})
export class GameComponent {
  @ViewChild(DiceComponent)
  protected diceComponent!: DiceComponent;

  @ViewChild(NextTurnComponent)
  protected nextTurnComponent!: NextTurnComponent;

  private gameService = inject(GameService);
  private signalRService = inject(PacketService);
  // implement handler

  constructor() {
    this.signalRService.sendPacket(new GameStatePacket())
  }

  protected handleDicePress() {
    this.signalRService.sendPacket(new RollDicePacket());
    this.handleRolledPacket();
  }

  protected handleRolledPacket(packet?: Packet) {
    const parsed = packet as RolledDicePacket;
    this.diceComponent.startDicing();

    const player = this.gameService.getPlayerByName(parsed.PlayerName);

    if (!player) {
      return;
    }

    setTimeout(() => {
      this.diceComponent.setDiced(parsed.RolledNumber, player);
    }, 1000);
  }

  protected handlePlayersTurnPacket(packet?: Packet) {
    const parsed = packet as PlayersTurnPacket;

    const player = this.gameService.getPlayerByName(parsed.PlayerName);

    if (!player) {
      return;
    }

    this.gameService.currentMover = player;
    this.nextTurnComponent.open(parsed.PlayerName);
  }

  protected movePlayer(value: number) {
    this.gameService.movePlayer(value);
  }
}
