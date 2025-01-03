import { Packet } from "../packet";

export class WantStatusPacket extends Packet {
  constructor() {
    super('WANT_STATUS');
  }
}

export type TWantStatusPacket = WantStatusPacket;