import { Packet } from '../../packet';

export class ReadyPacket extends Packet {
  constructor() {
    super('READY');
  }
}

export type TReadyPacket = ReadyPacket;
