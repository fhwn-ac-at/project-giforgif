import { Injectable } from '@angular/core';
import { Tile } from '../../types/game/tile';
import { Player } from '../../types/game/player';
import { House } from '../../types/game/house';
import { Hotel } from '../../types/game/hotel';
import { BoardConfig } from '../../themes/types/board-config';
import { US_VERSION } from '../../themes/us-version';

@Injectable({
  providedIn: 'root',
})
export class GameService {
  public tiles: Map<number, Tile> = new Map<number, Tile>();
  public board: Tile[][] = [];
  public players: Player[] = [];
  public me: Player | null = null;
  public fields = new Map<number, string[]>();
  public theme: BoardConfig = US_VERSION;
  public currentMover: Player | null = null;

  public rentMultiplier = 1;
  public priceMultiplier = 1;

  constructor() {
    this.setupDefaultTiles();
    this.setupBoard();
    this.setUpFields();
  }

  public buildHouse(index: number) {
    const tile = this.tiles.get(index);

    if (!tile) {
      return;
    }

    if (tile.buildings.length <= 3) {
      tile.buildings.push(new House('bg-green-900'));
    } else {
      tile.buildings = [new Hotel('bg-red-900')];
    }

    this.tiles = new Map(this.tiles);
  }

  public removeHouse(index: number) {
    const tile = this.tiles.get(index);

    if (!tile) {
      return;
    }

    if (tile.buildings[0] instanceof Hotel) {
      tile.buildings = [
        new House('bg-green-900'),
        new House('bg-green-900'),
        new House('bg-green-900'),
        new House('bg-green-900'),
      ];
    } else {
      tile.buildings =  tile.buildings.slice(0, -1);
    }

    this.tiles = new Map(this.tiles);
  }

  public giveToPlayer(index: number, from: Player, to: Player) {
    const tile = this.tiles.get(index);

    if (!tile) {
      return;
    }

    from.owns.splice(
      from.owns.findIndex((t) => t === index),
      1
    );
    tile.owner = to;
    tile.buildings = [];
    this.tiles = new Map(this.tiles);
  }

  public giveToBank(index: number) {
    const tile = this.tiles.get(index);

    if (!tile) {
      return;
    }

    tile.owner = undefined;
    tile.buildings = [];

    this.tiles = new Map(this.tiles);
  }

  public payPlayer(from: Player, to: Player, amount: number) {
    from.currency -= amount;
    to.currency += amount;
  }

  public setPlayerPosition(player: Player, position: number) {
    const current = this.fields.get(player.currentPosition);

    if (!current) {
      return;
    }

    this.fields.set(player.currentPosition, [
      ...current.filter((name) => name !== player.name),
    ]);

    const field = this.fields.get(position);

    if (field) {
      field.push(player.name);
      player.currentPosition = position;
      this.fields = new Map(this.fields);
      return;
    }
  }

  public removePolitic() {
    this.rentMultiplier = 1;
    this.priceMultiplier = 1;
  }

  public setOwner(player: Player | undefined, fieldId: number) {
    const tile = this.tiles.get(fieldId);

    if (!tile) {
      return;
    }

    tile.owner = player;
    player?.owns.push(fieldId);
    this.tiles = new Map(this.tiles);
  }

  public getPlayerByName(name: string): Player | undefined {
    return this.players.find((p) => p.name === name);
  }

  public movePlayer(value: number) {
    if (!this.currentMover) {
      return;
    }

    if (this.currentMover.currentPosition + value == 30) {
      return;
    }

    const field = this.fields.get(this.currentMover.currentPosition);

    if (!field) {
      return;
    }

    console.log(field);

    this.fields.set(this.currentMover.currentPosition, [
      ...field.filter((name) => name !== this.currentMover?.name),
    ]);

    let newPosition = (this.currentMover.currentPosition += value);

    if (newPosition > 40) {
      newPosition -= 40;
    }

    this.currentMover.currentPosition = newPosition;

    this.fields
      .get(this.currentMover.currentPosition)
      ?.push(this.currentMover.name);

    this.fields = new Map(this.fields);
  }

  private setUpFields() {
    for (let i = 1; i < 41; i++) {
      this.fields.set(i, []);
    }
  }

  private setupDefaultTiles() {
    this.tiles.set(21, new Tile(21, 'bg-black', []));
    this.tiles.set(22, new Tile(22, 'bg-red-300', []));
    this.tiles.set(23, new Tile(23, "bg-[url('/chance.png')]", []));
    this.tiles.set(24, new Tile(24, 'bg-red-400 ', []));
    this.tiles.set(25, new Tile(25, 'bg-red-500', []));
    this.tiles.set(26, new Tile(26, "bg-[url('/train.png')]", []));
    this.tiles.set(27, new Tile(27, 'bg-yellow-300', []));
    this.tiles.set(28, new Tile(28, 'bg-yellow-400', []));
    this.tiles.set(29, new Tile(29, 'bg-indigo-300', []));
    this.tiles.set(30, new Tile(30, 'bg-yellow-500', []));
    this.tiles.set(31, new Tile(31, 'bg-black', []));

    this.tiles.set(20, new Tile(20, 'bg-orange-500', []));
    this.tiles.set(32, new Tile(32, 'bg-green-300', []));

    this.tiles.set(19, new Tile(19, 'bg-orange-400', []));
    this.tiles.set(33, new Tile(33, 'bg-green-400', []));

    this.tiles.set(18, new Tile(18, "bg-[url('/chest.png')]", []));
    this.tiles.set(34, new Tile(34, "bg-[url('/chest.png')]", []));

    this.tiles.set(17, new Tile(17, 'bg-orange-300', []));
    this.tiles.set(35, new Tile(35, 'bg-green-500', []));

    this.tiles.set(16, new Tile(16, "bg-[url('/train.png')]", []));
    this.tiles.set(36, new Tile(36, "bg-[url('/train.png')]", []));

    this.tiles.set(15, new Tile(15, 'bg-pink-500', []));
    this.tiles.set(37, new Tile(37, "bg-[url('/chance.png')]", []));

    this.tiles.set(14, new Tile(14, 'bg-pink-400', []));
    this.tiles.set(38, new Tile(38, 'bg-blue-600', []));

    this.tiles.set(13, new Tile(13, 'bg-indigo-400', []));
    this.tiles.set(39, new Tile(39, 'bg-gray-600', []));

    this.tiles.set(12, new Tile(12, 'bg-pink-300', []));
    this.tiles.set(40, new Tile(40, 'bg-blue-700', []));

    this.tiles.set(11, new Tile(11, 'bg-black', []));
    this.tiles.set(10, new Tile(10, 'bg-sky-500', []));
    this.tiles.set(9, new Tile(9, 'bg-sky-400', []));
    this.tiles.set(8, new Tile(8, "bg-[url('/chance.png')]", []));
    this.tiles.set(7, new Tile(7, 'bg-sky-300', []));
    this.tiles.set(6, new Tile(6, "bg-[url('/train.png')]", []));
    this.tiles.set(5, new Tile(5, 'bg-gray-600', []));
    this.tiles.set(4, new Tile(4, 'bg-orange-950', []));
    this.tiles.set(3, new Tile(3, "bg-[url('/chest.png')]", []));
    this.tiles.set(2, new Tile(2, 'bg-orange-900', []));
    this.tiles.set(1, new Tile(1, 'bg-black', []));
  }

  private setupBoard() {
    this.board.push([
      this.tiles.get(21)!,
      this.tiles.get(22)!,
      this.tiles.get(23)!,
      this.tiles.get(24)!,
      this.tiles.get(25)!,
      this.tiles.get(26)!,
      this.tiles.get(27)!,
      this.tiles.get(28)!,
      this.tiles.get(29)!,
      this.tiles.get(30)!,
      this.tiles.get(31)!,
    ]);

    this.board.push([this.tiles.get(20)!, this.tiles.get(32)!]);
    this.board.push([this.tiles.get(19)!, this.tiles.get(33)!]);
    this.board.push([this.tiles.get(18)!, this.tiles.get(34)!]);
    this.board.push([this.tiles.get(17)!, this.tiles.get(35)!]);
    this.board.push([this.tiles.get(16)!, this.tiles.get(36)!]);
    this.board.push([this.tiles.get(15)!, this.tiles.get(37)!]);
    this.board.push([this.tiles.get(14)!, this.tiles.get(38)!]);
    this.board.push([this.tiles.get(13)!, this.tiles.get(39)!]);
    this.board.push([this.tiles.get(12)!, this.tiles.get(40)!]);

    this.board.push([
      this.tiles.get(11)!,
      this.tiles.get(10)!,
      this.tiles.get(9)!,
      this.tiles.get(8)!,
      this.tiles.get(7)!,
      this.tiles.get(6)!,
      this.tiles.get(5)!,
      this.tiles.get(4)!,
      this.tiles.get(3)!,
      this.tiles.get(2)!,
      this.tiles.get(1)!,
    ]);
  }
}
