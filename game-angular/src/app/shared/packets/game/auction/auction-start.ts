import { Packet } from '../../packet';

export class AuctionStartPacket extends Packet {
  constructor() {
    super('AUCTION_START');
  }

  public FieldId!: number;
}

export type TAuctionStartPacket = AuctionStartPacket;
