import { Packet } from "@shared/packets";

export class BuyingPriceIncreasePacket extends Packet {
  constructor() {
    super('BUYING_PRICE_INCREASE');
  }

  public NewMultiplier!: number;
}

export type TBuyingPriceIncreasePacket = BuyingPriceIncreasePacket;
