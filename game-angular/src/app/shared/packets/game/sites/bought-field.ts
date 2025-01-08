import { Packet } from '../../packet';

export class BoughtFieldPacket extends Packet {
  constructor() {
    super('BOUGHT_FIELD');
  }
  
  public PlayerName!: string;
  public FieldId!: number;
  public ReducedBy!: number;
}

export type TBoughtFieldPacket = BoughtFieldPacket;
