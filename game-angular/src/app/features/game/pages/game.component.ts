import { Component, inject, OnDestroy, ViewChild } from '@angular/core';
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
import { PlayersTurnPacket } from '../../../shared/packets/game/player/player-turn';
import { ReadyPacket } from '../../../shared/packets/game/util/ready';
import { Handler } from '../../../shared/class/handler';
import { ToastService } from '../../../shared/services/toast/toast.service';
import { GameStatePacket } from '../../../shared/packets/game/game-state';
import { EndTurnPacket } from '../../../shared/packets/game/util/end-turn';
import { BuyRequestPacket } from '../../../shared/packets/game/sites/buy-request';
import { PaymentDecisionPacket } from '../../../shared/packets/game/sites/payment-decision';
import { BoughtFieldPacket } from '../../../shared/packets/game/sites/bought-field';
import { PayPlayerPacket } from '../../../shared/packets/game/player/pay-player';
import { AuctionStartPacket } from '../../../shared/packets/game/auction/auction-start';
import { AuctionBidPacket } from '../../../shared/packets/game/auction/auction-bid';
import { AuctionUpdatePacket } from '../../../shared/packets/game/auction/auction-update';
import { AuctionResultPacket } from '../../../shared/packets/game/auction/auction-result';
import { HouseBuiltPacket } from '../../../shared/packets/game/house/house-built';
import { GoToJailPacket } from '../../../shared/packets/game/jail/go-to-jail';
import { JailPayoutPacket } from '../../../shared/packets/game/jail/jail-payout';
import { PayoutSucessPacket } from '../../../shared/packets/game/jail/payout-sucess';

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
export class GameComponent extends Handler implements OnDestroy {
  @ViewChild(DiceComponent)
  protected diceComponent!: DiceComponent;

  @ViewChild(NextTurnComponent)
  protected nextTurnComponent!: NextTurnComponent;

  @ViewChild(BuyTileComponent)
  protected buyTileComponent!: BuyTileComponent;

  @ViewChild(AuctionComponent)
  protected auctionComponent!: AuctionComponent;

  public gameService = inject(GameService);

  // protected players = this.gameService.players;
  // implement handler

  constructor(
    private readonly signalRService: PacketService,
    private readonly toastService: ToastService
  ) {
    super(signalRService, toastService);

    this.handler.set('ROLLED', this.handleRolledPacket.bind(this));
    this.handler.set('PLAYERS_TURN', this.handlePlayersTurnPacket.bind(this));
    this.handler.set('GAME_STATE', this.handleGameState.bind(this));
    this.handler.set('BUY_REQUEST', this.handleBuyRequest.bind(this));
    this.handler.set('BOUGHT_FIELD', this.handleBought.bind(this));
    this.handler.set('PAY_PLAYER', this.handlePayPlayer.bind(this));
    this.handler.set('AUCTION_START', this.handleAuctionStart.bind(this));
    this.handler.set('AUCTION_UPDATE', this.handleAuctionUpdate.bind(this));
    this.handler.set('AUCTION_RESULT', this.handleAuctionResult.bind(this));
    this.handler.set('HOUSE_BUILT', this.handleHouseBuilt.bind(this));
    this.handler.set('GO_TO_JAIL', this.handleGoToJail.bind(this));
    this.handler.set('PAYOUT_SUCESS', this.handlePayoutSucess.bind(this));

    this.signalRService.sendPacket(new ReadyPacket());
  }

  public ngOnDestroy(): void {
    this.stopHandler();
  }

  protected handlePayoutSucess(packet: Packet) {
    const parsed = packet as PayoutSucessPacket;

    const player = this.gameService.getPlayerByName(parsed.PlayerName);

    if (!player) {
      return;
    }

    player.isInJail = false;
    player.currency -= parsed.Cost;
  }

  protected async handleGoToJail(packet: Packet) {
    await this.waitUntilDiceAnimationEnds();
    const parsed = packet as GoToJailPacket;

    const player = this.gameService.getPlayerByName(parsed.PlayerName);

    if (!player) {
      return;
    }

    player.isInJail = true;
    this.gameService.setPlayerPosition(player, 11);
  }

  protected payJailPayout() {
    this.signalRService.sendPacket(new JailPayoutPacket());
  }

  protected handleHouseBuilt(packet: Packet) {
    const parsed = packet as HouseBuiltPacket;

    const player = this.gameService.getPlayerByName(parsed.PlayerName);

    if (!player) {
      return;
    }

    player.currency -= parsed.Cost;
    this.gameService.buildHouse(parsed.FieldId);
  }

  protected handleAuctionResult(packet: Packet) {
    const parsed = packet as AuctionResultPacket;

    this.auctionComponent.close();
    this.auctionComponent.stopTimer();

    if (!parsed.WinnerPlayerName) {
      return;
    }

    const winner = this.gameService.getPlayerByName(parsed.WinnerPlayerName);

    if (!winner) {
      return;
    }

    this.gameService.setOwner(winner, parsed.PropertyId);
    winner.currency -= parsed.WinningBid;
  }

  protected handleAuctionUpdate(packet: Packet) {
    const parsed = packet as AuctionUpdatePacket;

    this.auctionComponent.setCurrentBid(parsed.CurrentBid);
    this.auctionComponent.setHighestBidder(parsed.HighestBidderName);
    this.auctionComponent.resetTimer();
  }

  protected handleAuctionStart(packet: Packet) {
    const parsed = packet as AuctionStartPacket;

    this.buyTileComponent.close();
    this.auctionComponent.showBuyOption(parsed.FieldId);
    this.auctionComponent.startTimer();
  }

  protected onBid(amount: number) {
    const pkg = new AuctionBidPacket();
    pkg.Bid = amount;
    this.signalRService.sendPacket(pkg);
  }

  protected handlePayPlayer(packet: Packet) {
    const parsed = packet as PayPlayerPacket;

    console.log('PAY_PLAYER', parsed);

    const from = this.gameService.getPlayerByName(parsed.From);
    const to = this.gameService.getPlayerByName(parsed.To);

    if (!from || !to) {
      return;
    }

    this.gameService.payPlayer(from, to, parsed.Amount);
  }

  protected handleBought(packet: Packet) {
    const parsed = packet as BoughtFieldPacket;

    const player = this.gameService.getPlayerByName(parsed.PlayerName);

    if (!player) {
      return;
    }

    player.currency -= parsed.ReducedBy;
    this.gameService.setOwner(player, parsed.FieldId);
    this.buyTileComponent.close();
  }

  protected async handleBuyRequest(packet: Packet) {
    const parsed = packet as BuyRequestPacket;
    await this.waitUntilDiceAnimationEnds();
    this.buyTileComponent.showBuyOption(parsed.FieldId);
  }

  protected onBuyTile(index: number) {
    const decision = new PaymentDecisionPacket();
    decision.WantsToBuy = true;
    this.signalRService.sendPacket(decision);
  }

  protected onAuctionTile(index: number) {
    const decision = new PaymentDecisionPacket();
    decision.WantsToBuy = false;
    this.signalRService.sendPacket(decision);
  }

  protected handleGameState(packet: Packet) {
    const parsed = packet as GameStatePacket;

    this.gameService.me = {
      color: parsed.Me.Color,
      currency: parsed.Me.Currency,
      currentPosition: parsed.Me.CurrentPositionFieldId,
      name: parsed.Me.Name,
      isInJail: false,
    };

    this.gameService.players = parsed.Players.map((p) => {
      return {
        color: p.Color,
        currency: p.Currency,
        currentPosition: p.CurrentPositionFieldId,
        name: p.Name,
        isInJail: false,
      };
    });

    this.gameService.players.push(this.gameService.me);

    for (let player of parsed.Players) {
      this.gameService.setPlayerPosition(
        this.gameService.getPlayerByName(player.Name)!,
        1
      );
    }

    this.gameService.setPlayerPosition(this.gameService.me, 1);

    console.log(this.gameService.fields);
  }

  protected handleDicePress() {
    this.signalRService.sendPacket(new RollDicePacket());
  }

  protected handleEndTurn() {
    this.signalRService.sendPacket(new EndTurnPacket());
  }

  protected handleRolledPacket(packet: Packet) {
    const parsed = packet as RolledDicePacket;

    const player = this.gameService.getPlayerByName(parsed.PlayerName);

    if (!player) {
      return;
    }

    this.diceComponent.startDicing();

    setTimeout(() => {
      this.diceComponent.setDiced(parsed.RolledNumber, player);
    }, 1000);
  }

  protected async handlePlayersTurnPacket(packet?: Packet) {
    await this.waitUntilDiceAnimationEnds();
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

  private async waitUntilDiceAnimationEnds(): Promise<void> {
    while (this.diceComponent?.isDicing) {
      await new Promise((resolve) => setTimeout(resolve, 100));
    }
  }
}
