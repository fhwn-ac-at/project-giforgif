import { Packet } from '../../packet';

export class MovePlayerPacket extends Packet {
  constructor() {
    super('MOVE_PLAYER');
  }

  public PlayerName!: string;
  public FieldId!: number;
}

export type TMovePlayerPacket = MovePlayerPacket;
