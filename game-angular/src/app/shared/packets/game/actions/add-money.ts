import { Packet } from '../../packet';

export class AddMoneyPacket extends Packet {
  constructor() {
    super('ADD_MONEY');
  }

  public Amount!: number;
  public PlayerName!: string;
  public Description!: string;
}

export type TAddMoneyPacket = AddMoneyPacket;
