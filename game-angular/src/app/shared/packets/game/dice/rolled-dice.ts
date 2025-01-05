import { Packet } from '../../packet';

export class RolledDicePacket extends Packet {
  constructor() {
    super('ROLLED');
  }
  
  public RolledNumber!: number;
  public PlayerName!: string;
}

export type TRolledDicePacket = RolledDicePacket;
