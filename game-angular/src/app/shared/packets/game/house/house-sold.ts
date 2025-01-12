import { Packet } from '../../packet';

export class HouseSoldPacket extends Packet {
  constructor() {
    super('HOUSE_SOLD');
  }
  
  public FieldId!: number;
}

export type THouseSoldPacket = HouseSoldPacket;
