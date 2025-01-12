import { Packet } from '../../packet';

export class PlayerOutOfDebtPacket extends Packet {
  constructor() {
    super('PLAYER_OUT_OF_DEBT');
  }
}

export type TPlayerOutOfDebtPacket = PlayerOutOfDebtPacket;
