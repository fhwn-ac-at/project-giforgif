import { Component, inject } from '@angular/core';
import { GameService } from '../../../../shared/services/game/game.service';
import { Tile } from '../../../../shared/types/game/tile';
import { TileCardComponent } from '../tile-card/tile-card.component';
import { PacketService } from '../../../../shared/services/packet/packet.service';
import { BuildHousePacket } from '../../../../shared/packets/game/house/build-house';

@Component({
  selector: 'app-buy-house',
  imports: [TileCardComponent],
  templateUrl: './buy-house.component.html',
  styles: ``,
})
export class BuyHouseComponent {
  
  protected canBuy = true;
  protected visible = false;
  protected tile: Tile | null = null;

  protected readonly gameService = inject(GameService);
  private readonly signalRService = inject(PacketService);
  // constructor() {
  //   this.open(9);
  // }

  public open(index: number) {
    this.visible = true;
    this.tile = this.gameService.tiles.get(index)!;
    
    // this.canBuy = this.tile.owner === this.gameService.me;
  }

  public close() {
    this.visible = false;
  }

  public buyHouse() {
    if (!this.tile) {
      return;
    }

    const pkg = new BuildHousePacket();
    pkg.FieldId = this.tile.index;

    console.log(pkg)
    this.signalRService.sendPacket(pkg);
    this.close();
  }
}
