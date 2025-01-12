import { Packet } from '../../packet';

export class BuyRequestPacket extends Packet {
  constructor() {
    super('BUY_REQUEST');
  }
  
  public FieldId!: number;
  public PlayerName!: string;
}

export type TBuyRequestPacket = BuyRequestPacket;
