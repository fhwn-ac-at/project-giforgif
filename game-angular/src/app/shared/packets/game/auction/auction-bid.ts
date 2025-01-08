import { Packet } from '../../packet';

export class AuctionBidPacket extends Packet {
  constructor() {
    super('AUCTION_BID');
  }

  public Bid!: number;
}

export type TAuctionBidPacket = AuctionBidPacket;
