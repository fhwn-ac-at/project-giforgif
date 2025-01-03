import { Packet } from "../packet";

export class PlayerLeftPacket extends Packet {
  constructor() {
    super('PLAYER_LEFT');
  }

  public PlayerName: string | undefined;
}

export type TPlayerLeftPacket = PlayerLeftPacket;