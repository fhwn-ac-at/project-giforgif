import { Room } from "../../types";
import { Packet } from "../packet";

export class RoomsUpdatedPacket extends Packet {
  constructor() {
    super('ROOMS_UPDATED');
  }

  public Rooms: Room[] = [];
}

export type TRoomsUpdatedPacket = RoomsUpdatedPacket;