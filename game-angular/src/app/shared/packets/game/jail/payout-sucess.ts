import { Packet } from '../../packet';

export class PayoutSucessPacket extends Packet {
  constructor() {
    super('PAYOUT_SUCESS');
  }

  public PlayerName!: string;
  public Cost!: number;
}

export type TPayoutSucessPacket = PayoutSucessPacket;
