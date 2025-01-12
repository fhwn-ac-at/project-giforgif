import { Packet } from '../../packet';

export class SellPropertyPacket extends Packet {
  constructor() {
    super('SELL_PROPERTY');
  }

  public FieldId!: number;
}

export type TSellPropertyPacket = SellPropertyPacket;
