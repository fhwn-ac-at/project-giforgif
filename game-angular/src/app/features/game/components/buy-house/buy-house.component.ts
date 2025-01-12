import { Component, inject } from '@angular/core';
import { GameService } from '../../../../shared/services/game/game.service';
import { Tile } from '../../../../shared/types/game/tile';
import { TileCardComponent } from '../tile-card/tile-card.component';
import { PacketService } from '../../../../shared/services/packet/packet.service';
import { BuildHousePacket } from '../../../../shared/packets/game/house/build-house';
import { SellHousePacket } from '../../../../shared/packets/game/house/sell-house';
import { SellPropertyPacket } from '../../../../shared/packets/game/sites/sell-property';

@Component({
  selector: 'app-buy-house',
  imports: [TileCardComponent],
  templateUrl: './buy-house.component.html',
  styles: ``,
})
export class BuyHouseComponent {
  protected canBuy = true;
  protected canSell = true;
  protected canSellHouse = true;
  protected visible = false;
  protected tile: Tile | null = null;

  protected readonly gameService = inject(GameService);
  private readonly signalRService = inject(PacketService);

  public open(index: number) {
    this.visible = true;
    this.tile = this.gameService.tiles.get(index)!;

    this.canBuy =
      this.tile.owner === this.gameService.me &&
      this.gameService.theme.fields[index - 1].type === 'Site';

    // this.canSell = this.tile.owner === this.gameService.me && this.gameService.tiles.get(index)!.buildings.length == 0;

    this.canSellHouse = this.gameService.tiles.get(index)!.buildings.length > 0;
  }

  public close() {
    this.visible = false;
  }

  public sellSite() {
    if (!this.tile) {
      return;
    }

    const pkg = new SellPropertyPacket();
    pkg.FieldId = this.tile.index;

    this.signalRService.sendPacket(pkg);
    this.close();
  }

  public sellHouse() {
    if (!this.tile) {
      return;
    }

    const pkg = new SellHousePacket();
    pkg.FieldId = this.tile.index;

    this.signalRService.sendPacket(pkg);
    this.close();
  }

  public buyHouse() {
    if (!this.tile) {
      return;
    }

    const pkg = new BuildHousePacket();
    pkg.FieldId = this.tile.index;

    this.signalRService.sendPacket(pkg);
    this.close();
  }
}
