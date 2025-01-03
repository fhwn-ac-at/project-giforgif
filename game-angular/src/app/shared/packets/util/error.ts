import { Packet } from "../packet";

export class ErrorPacket extends Packet {
  constructor() {
    super('ERROR');
  }

  public Error: string | undefined;
  public Message: string | undefined;
}

export type TErrorPacket = ErrorPacket;