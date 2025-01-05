import { Packet } from '../../packet';

export class PlayersTurnPacket extends Packet {
  constructor() {
    super('PLAYERS_TURN');
  }

  public PlayerName!: string;
}

export type TPlayersTurnPacket = PlayersTurnPacket;
