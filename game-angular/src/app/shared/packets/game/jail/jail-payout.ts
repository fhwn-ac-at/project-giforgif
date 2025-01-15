import { Packet } from '../../packet';

export class JailPayoutPacket extends Packet {
  constructor() {
    super('JAIL_PAYOUT');
  }
}

export type TJailPayoutPacket = JailPayoutPacket;
