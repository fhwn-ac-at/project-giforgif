import { Packet } from "../packet";

export class RegisterPacket extends Packet {
  constructor() {
    super('REGISTER');
  }

  public PlayerName: string | undefined;
}

export type TRegisterPacket = RegisterPacket;