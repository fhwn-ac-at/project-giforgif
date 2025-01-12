import { Packet } from '../../packet';

export class PropertySoldPacket extends Packet {
  constructor() {
    super('PROPERTY_SOLD');
  }
  
  public FieldId!: number;
}

export type TPropertySoldPacket = PropertySoldPacket;
