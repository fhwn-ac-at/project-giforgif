import { Packet } from '../packet';

type PlayerRecord = {
  ConnectionId: string;
  Name: string;
  Color: 'red' | 'green' | 'yellow' | 'blue';
  Currency: number;
  CurrentPositionFieldId: number;
};

export class GameStatePacket extends Packet {
  constructor() {
    super('GAME_STATE');
  }

  public Me!: PlayerRecord;
  public Players: PlayerRecord[] = [];
}

export type TGameStatePacket = GameStatePacket;
