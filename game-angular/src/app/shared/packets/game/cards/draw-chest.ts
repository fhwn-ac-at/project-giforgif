import { Packet } from '../../packet';

export class DrawChestPacket extends Packet {
  constructor() {
    super('DRAW_CHEST');
  }

  public CardId!: number;
  public PlayerName!: string;
}

export type TDrawChestPacket = DrawChestPacket;
