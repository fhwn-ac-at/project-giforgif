import { Packet } from '../../packet';

export class GoToJailPacket extends Packet {
  constructor() {
    super('GO_TO_JAIL');
  }

  public PlayerName!: string;
}

export type TGoToJailPacket = GoToJailPacket;
