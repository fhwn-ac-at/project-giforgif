import { Packet } from "../packet";

export class CreateRoomPacket extends Packet {
  constructor() {
    super('CREATE_ROOM');
  }

  public RoomName: string | undefined;
}

export type TCreateRoomPacket = CreateRoomPacket;