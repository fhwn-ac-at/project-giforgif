import { Packet } from '../packet';

export class StartPacket extends Packet {
  constructor() {
    super('START');
  }
}

export type TStartPacket = StartPacket;
