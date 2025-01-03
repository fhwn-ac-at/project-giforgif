export interface IPacket {
  type: string;
}

export abstract class Packet {
  constructor(public Type: string) {}
}
