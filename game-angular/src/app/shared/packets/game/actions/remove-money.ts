import { Packet } from '../../packet';

export class RemoveMoneyPacket extends Packet {
  constructor() {
    super('REMOVE_MONEY');
  }

  public Amount!: number;
  public PlayerName!: string;
  public Description!: string;
}

export type TRemoveMoneyPacket = RemoveMoneyPacket;
