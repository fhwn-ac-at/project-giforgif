abstract class Packet {
  constructor(public Type: string) {}
}

export class RegisterPacket extends Packet {
  constructor() {
    super('REGISTER');
  }

  public PlayerName: string | undefined;
}

export type TRegisterPacket = RegisterPacket;