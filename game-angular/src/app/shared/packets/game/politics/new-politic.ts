import { Packet } from "@shared/packets";

export class NewPoliticPacket extends Packet {
  constructor() {
    super('NEW_POLITIC');
  }

  public PoliticId!: number;
}

export type TNewPoliticPacket = NewPoliticPacket;
