import { Packet } from '../../packet';

export class PayPlayerPacket extends Packet {
  constructor() {
    super('PAY_PLAYER');
  }
  
  public From!: string;
  public To!: string;
  public Amount!: number;
}

export type TPayPlayerPacket = PayPlayerPacket;
