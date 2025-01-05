import { Packet } from '../packet';

export class GameStatePacket extends Packet {
  constructor() {
    super('GAME_STATE');
  }
}

export type TGameStatePacket = GameStatePacket;
