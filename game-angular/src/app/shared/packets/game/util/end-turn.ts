import { Packet } from '../../packet';

export class EndTurnPacket extends Packet {
  constructor() {
    super('END_TURN');
  }
}

export type TEndTurnPacket = EndTurnPacket;
