import { Packet } from '../../packet';

export class BankurptcyPacket extends Packet {
  constructor() {
    super('BANKRUPTCY');
  }

  public PlayerName!: string;
}

export type TBankurptcyPacket = BankurptcyPacket;
