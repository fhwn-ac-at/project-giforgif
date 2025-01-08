import { Packet } from '../../packet';

export class AuctionResultPacket extends Packet {
  constructor() {
    super('AUCTION_RESULT');
  }

  public WinningBid!: number;
  public WinnerPlayerName!: string;
  public PropertyId!: number;
}

export type TAuctionResultPacket = AuctionResultPacket;
