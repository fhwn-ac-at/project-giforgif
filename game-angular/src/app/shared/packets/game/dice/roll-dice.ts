import { Packet } from '../../packet';

export class RollDicePacket extends Packet {
  constructor() {
    super('ROLL_DICE');
  }
}

export type TRollDicePacket = RollDicePacket;
