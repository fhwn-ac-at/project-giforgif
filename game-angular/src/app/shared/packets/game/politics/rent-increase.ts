import { Packet } from "@shared/packets";

export class RentIncreasePacket extends Packet {
  constructor() {
    super('RENT_INCREASE');
  }

  public NewMultiplier!: number;
}

export type TRentIncreasePacket = RentIncreasePacket;
