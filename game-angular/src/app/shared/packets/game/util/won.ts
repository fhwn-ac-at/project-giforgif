import { Packet } from '../../packet';

export class WonPacket extends Packet {
  constructor() {
    super('WON');
  }
  
  public PlayerName!: string;
}

export type TWonPacket = WonPacket;
