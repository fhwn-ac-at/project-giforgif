import { Packet } from '../../packet';

export class HouseBuiltPacket extends Packet {
  constructor() {
    super('HOUSE_BUILT');
  }

  public FieldId!: number;
  public Cost!: number;
  public PlayerName!: string;
}

export type THouseBuiltPacket = HouseBuiltPacket;
