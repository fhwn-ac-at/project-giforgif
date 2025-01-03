import { Packet } from "../packet";

export class JoinRoomPacket extends Packet {
  constructor() {
    super('JOIN_ROOM');
  }

  public RoomName: string | undefined;
}

export type TJoinRoomPacket = JoinRoomPacket;