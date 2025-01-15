import { Packet } from '../packet';

export class WantRoomsPacket extends Packet {
  constructor() {
    super('WANT_ROOMS');
  }
}

export type TWantRoomsPacket = WantRoomsPacket;
