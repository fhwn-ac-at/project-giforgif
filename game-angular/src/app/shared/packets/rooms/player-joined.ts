import { Packet } from "../packet";

export class PlayerJoinedPacket extends Packet {
  constructor() {
    super('PLAYER_JOINED');
  }

  public PlayerName: string | undefined;
}

export type TPlayerJoinedPacket = PlayerJoinedPacket;