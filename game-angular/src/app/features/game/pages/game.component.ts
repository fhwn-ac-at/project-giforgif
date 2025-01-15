import { Component, inject, OnDestroy, ViewChild } from '@angular/core';
import { BoardComponent } from '../components/board/board.component';
import { PlayerCardComponent } from '../components/player-card/player-card.component';
import { DiceComponent } from '../components/dice/dice.component';
import { BuyTileComponent } from '../components/buy-tile/buy-tile.component';
import { AuctionComponent } from '../components/auction/auction.component';
import { EventCardComponent } from '../components/event-card/event-card.component';
import { NextTurnComponent } from '../components/next-turn/next-turn.component';
import { ActionScreenComponent } from '../components/action-screen/action-screen.component';
import { Router } from '@angular/router';
import {
  AddMoneyPacket,
  AuctionBidPacket,
  AuctionResultPacket,
  AuctionStartPacket,
  AuctionUpdatePacket,
  BankurptcyPacket,
  BoughtFieldPacket,
  BuyingPriceIncreasePacket,
  BuyRequestPacket,
  DrawChancePacket,
  DrawChestPacket,
  EndTurnPacket,
  GameStatePacket,
  GoToJailPacket,
  HouseBuiltPacket,
  HouseSoldPacket,
  JailPayoutPacket,
  MovePlayerPacket,
  NewPoliticPacket,
  Packet,
  PaymentDecisionPacket,
  PayoutSucessPacket,
  PayPlayerPacket,
  PlayersTurnPacket,
  PropertySoldPacket,
  ReadyPacket,
  RemoveMoneyPacket,
  RentIncreasePacket,
  RollDicePacket,
  RolledDicePacket,
  SellPropertiesPacket,
  TransferPropertiesPacket,
  WonPacket,
} from '@shared/packets';
import { ToastService } from '@shared/services/toast/toast.service';
import { Handler } from '@shared/class/handler';
import { GameService } from '@shared/services/game/game.service';
import { PacketService } from '@shared/services/packet/packet.service';
import { SharedModule } from '@shared/shared.module';
import { PoliticsScreenComponent } from '../components/politics-screen/politics-screen.component';

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
    ActionScreenComponent,
    PoliticsScreenComponent,
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

  @ViewChild(EventCardComponent)
  protected eventCardComponent!: EventCardComponent;

  @ViewChild(ActionScreenComponent)
  protected actionScreen!: ActionScreenComponent;

  @ViewChild(PoliticsScreenComponent)
  protected politicScreen!: PoliticsScreenComponent;

  protected inDebt: boolean = false;
  protected debt: number = 0;

  public gameService = inject(GameService);
  protected router = inject(Router);

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
    this.handler.set('ADD_MONEY', this.handleAddMoney.bind(this));
    this.handler.set('REMOVE_MONEY', this.handleRemoveMoney.bind(this));
    this.handler.set('BANKRUPTCY', this.handleBankruptcy.bind(this));
    this.handler.set('SELL_PROPERTIES', this.handleSellProperties.bind(this));
    this.handler.set(
      'PLAYER_OUT_OF_DEBT',
      this.handlePlayerOutOfDebt.bind(this)
    );
    this.handler.set('PROPERTY_SOLD', this.handlePropertySold.bind(this));
    this.handler.set('HOUSE_SOLD', this.handleHouseSold.bind(this));
    this.handler.set(
      'TRANSFER_PROPERTIES',
      this.handleTransferProperties.bind(this)
    );
    this.handler.set('WON', this.handleWon.bind(this));
    this.handler.set('MOVE_PLAYER', this.handleMovePlayer.bind(this));
    this.handler.set('DRAW_CHANCE', this.handleDrawChance.bind(this));
    this.handler.set('DRAW_CHEST', this.handleDrawChest.bind(this));
    this.handler.set('NEW_POLITIC', this.handleNewPolitic.bind(this));
    this.handler.set('POLITIC_RESET', this.handlePoliticReset.bind(this));
    this.handler.set(
      'BUYING_PRICE_INCREASE',
      this.handleBuyingPriceIncrease.bind(this)
    );
    this.handler.set('RENT_INCREASE', this.handleRentIncrease.bind(this));

    this.signalRService.sendPacket(new ReadyPacket());
  }

  public ngOnDestroy(): void {
    this.stopHandler();
  }

  protected handleBuyingPriceIncrease(packet: Packet) {
    const parsed = packet as BuyingPriceIncreasePacket;

    this.gameService.priceMultiplier = parsed.NewMultiplier;
  }

  protected handleRentIncrease(packet: Packet) {
    const parsed = packet as RentIncreasePacket;

    this.gameService.priceMultiplier = parsed.NewMultiplier;
  }

  protected handlePoliticReset(packet: Packet) {
    this.gameService.removePolitic();
  }

  protected async handleNewPolitic(packet: Packet) {
    await this.waitUntilDiceAnimationEnds();

    const parsed = packet as NewPoliticPacket;

    this.politicScreen.start();

    setTimeout(() => {
      this.politicScreen.setWon(parsed.PoliticId);
    }, 2000);
  }

  protected async handleDrawChance(packet: Packet) {
    await this.waitUntilPoliticAnimationEnds();
    await this.waitUntilDiceAnimationEnds();

    const parsed = packet as DrawChancePacket;

    if (parsed.PlayerName !== this.gameService.me?.name) {
      return;
    }

    this.eventCardComponent.open(
      'Chance',
      this.gameService.theme.chances[parsed.CardId]
    );
  }

  protected async handleDrawChest(packet: Packet) {
    await this.waitUntilPoliticAnimationEnds();
    await this.waitUntilDiceAnimationEnds();

    const parsed = packet as DrawChestPacket;

    if (parsed.PlayerName !== this.gameService.me?.name) {
      return;
    }

    this.eventCardComponent.open(
      'Community Chest',
      this.gameService.theme.chests[parsed.CardId]
    );
  }

  protected async handleMovePlayer(packet: Packet) {
    await this.waitUntilPoliticAnimationEnds();
    await this.waitUntilDiceAnimationEnds();
    await this.waitUntilEventCardEnds();
    const parsed = packet as MovePlayerPacket;

    const player = this.gameService.getPlayerByName(parsed.PlayerName);

    if (!player) {
      return;
    }

    this.gameService.setPlayerPosition(player, parsed.FieldId);
  }

  protected async handleWon(packet: Packet) {
    await this.waitUntilPoliticAnimationEnds();
    await this.waitUntilEventCardEnds();
    await this.waitUntilDiceAnimationEnds();
    const parsed = packet as WonPacket;

    if (parsed.PlayerName === this.gameService.me?.name) {
      this.actionScreen.open(this.gameService.theme.winScreen);
    }

    setTimeout(() => {
      this.router.navigate(['menu']);
    }, 3000);
  }

  protected handleTransferProperties(packet: Packet) {
    const parsed = packet as TransferPropertiesPacket;
    const player = this.gameService.getPlayerByName(parsed.From);

    if (!player) {
      return;
    }

    if (parsed.To === null) {
      for (let tile of player.owns) {
        this.gameService.giveToBank(tile);
      }

      return;
    }

    const to = this.gameService.getPlayerByName(parsed.To);

    if (!to) {
      return;
    }

    for (let tile of player.owns) {
      this.gameService.giveToPlayer(tile, player, to);
    }
  }

  protected handleHouseSold(packet: Packet) {
    const parsed = packet as HouseSoldPacket;

    this.gameService.removeHouse(parsed.FieldId);
  }

  protected handlePropertySold(packet: Packet) {
    const parsed = packet as PropertySoldPacket;

    this.gameService.setOwner(undefined, parsed.FieldId);
  }

  protected handlePlayerOutOfDebt(packet: Packet) {
    this.inDebt = false;
    this.debt = 0;
  }

  protected handleSellProperties(packet: Packet) {
    const parsed = packet as SellPropertiesPacket;

    if (parsed.PlayerName !== this.gameService.me?.name) {
      return;
    }

    this.inDebt = true;
    this.debt = parsed.Amount;
  }

  protected async handleBankruptcy(packet: Packet) {
    await this.waitUntilPoliticAnimationEnds();
    await this.waitUntilEventCardEnds();
    await this.waitUntilDiceAnimationEnds();

    const parsed = packet as BankurptcyPacket;
    if (parsed.PlayerName === this.gameService.me?.name) {
      this.actionScreen.open(this.gameService.theme.bankruptScreen);
    }
  }

  protected handleAddMoney(packet: Packet) {
    const parsed = packet as AddMoneyPacket;

    const player = this.gameService.getPlayerByName(parsed.PlayerName);

    if (!player) {
      return;
    }

    player.currency += parsed.Amount;
  }

  protected handleRemoveMoney(packet: Packet) {
    const parsed = packet as RemoveMoneyPacket;

    console.log('THISIS MY PP');

    const player = this.gameService.getPlayerByName(parsed.PlayerName);

    if (!player) {
      return;
    }

    player.currency -= parsed.Amount;
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
    await this.waitUntilEventCardEnds();
    await this.waitUntilDiceAnimationEnds();
    await this.waitUntilPoliticAnimationEnds();
    const parsed = packet as GoToJailPacket;

    if (parsed.PlayerName === this.gameService.me?.name) {
      this.actionScreen.open(this.gameService.theme.jailScreen);
    }
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
    await this.waitUntilPoliticAnimationEnds();
    await this.waitUntilDiceAnimationEnds();
    await this.waitUntilEventCardEnds();

    const parsed = packet as BuyRequestPacket;

    if (parsed.PlayerName !== this.gameService.me?.name) {
      return;
    }

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
      owns: [],
    };

    this.gameService.players = parsed.Players.map((p) => {
      return {
        color: p.Color,
        currency: p.Currency,
        currentPosition: p.CurrentPositionFieldId,
        name: p.Name,
        isInJail: false,
        owns: [],
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
    await this.waitUntilEventCardEnds();
    await this.waitUntilDiceAnimationEnds();
    await this.waitUntilActionScreenEnds();
    await this.waitUntilPoliticAnimationEnds();
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

  private async waitUntilEventCardEnds(): Promise<void> {
    while (this.eventCardComponent.visible) {
      await new Promise((resolve) => setTimeout(resolve, 100));
    }
  }

  private async waitUntilActionScreenEnds(): Promise<void> {
    while (this.actionScreen?.isShowing) {
      await new Promise((resolve) => setTimeout(resolve, 100));
    }
  }

  private async waitUntilDiceAnimationEnds(): Promise<void> {
    while (this.diceComponent?.isDicing) {
      await new Promise((resolve) => setTimeout(resolve, 100));
    }
  }

  private async waitUntilPoliticAnimationEnds(): Promise<void> {
    while (this.politicScreen.visible) {
      await new Promise((resolve) => setTimeout(resolve, 100));
    }
  }
}
