import { Packet } from '../packet';

type PlayerRecord = {
  ConnectionId: string;
  Name: string;
};

export class StatusPacket extends Packet {
  constructor() {
    super('STATUS');
  }

  public Me!: PlayerRecord;
  public Players: PlayerRecord[] = [];
}

export type TStatusPacket = StatusPacket;
