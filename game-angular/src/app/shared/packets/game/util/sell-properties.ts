import { Packet } from '../../packet';

export class SellPropertiesPacket extends Packet {
  constructor() {
    super('SELL_PROPERTIES');
  }
  
  public Amount!: number;
  public PlayerName!: string;
}

export type TSellPropertiesPacket = SellPropertiesPacket;
