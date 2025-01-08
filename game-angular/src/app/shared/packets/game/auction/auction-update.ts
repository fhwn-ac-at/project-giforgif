import { Packet } from '../../packet';

export class AuctionUpdatePacket extends Packet {
  constructor() {
    super('AUCTION_UPDATE');
  }

  public CurrentBid!: number;
  public HighestBidderName!: string;
}

export type TAuctionUpdatePacket = AuctionUpdatePacket;
