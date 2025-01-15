import { Packet } from "@shared/packets";

export class ResetPoliticPacket extends Packet {
  constructor() {
    super('POLITIC_RESET');
  }
}

export type TResetPoliticPacket = ResetPoliticPacket;
