import { Packet } from "../packet";

export class LeaveRoomPacket extends Packet {
  constructor() {
    super('LEAVE_ROOM');
  }
}

export type TLeaveRoomPacket = LeaveRoomPacket;