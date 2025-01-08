import { Packet } from '../../packet';

export class PaymentDecisionPacket extends Packet {
  constructor() {
    super('PAYMENT_DECISION');
  }
  
  public WantsToBuy!: boolean;
}

export type TPaymentDecisionPacket = PaymentDecisionPacket;
