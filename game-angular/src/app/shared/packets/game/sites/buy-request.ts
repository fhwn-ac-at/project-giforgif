import { Packet } from '../../packet';

export class BuyRequestPacket extends Packet {
  constructor() {
    super('BUY_REQUEST');
  }
  
  public FieldId!: number;
}

export type TBuyRequestPacket = BuyRequestPacket;
