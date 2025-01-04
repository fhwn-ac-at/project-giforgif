import { Player } from './player';
import { Building } from './building';

export class Tile {
  constructor(
    public index: number,
    public style: string,
    public buildings: Building[],
    public owner?: Player
  ) {}
}
