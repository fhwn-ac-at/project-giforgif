import { Packet } from '../../packet';

export class DrawChancePacket extends Packet {
  constructor() {
    super('DRAW_CHANCE');
  }

  public CardId!: number;
  public PlayerName!: string;
}

export type TDrawChancePacket = DrawChancePacket;
