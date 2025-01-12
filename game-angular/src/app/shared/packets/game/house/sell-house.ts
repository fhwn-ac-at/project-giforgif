import { Packet } from '../../packet';

export class SellHousePacket extends Packet {
  constructor() {
    super('SELL_HOUSE');
  }

  public FieldId!: number;
}

export type TSellHousePacket = SellHousePacket;
