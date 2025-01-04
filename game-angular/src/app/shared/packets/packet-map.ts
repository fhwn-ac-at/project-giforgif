import { TStartPacket } from './lobby/start';
import { TStatusPacket } from './lobby/status';
import { Packet } from './packet';
import { TPlayerJoinedPacket } from './rooms/player-joined';
import { TPlayerLeftPacket } from './rooms/player-left';
import { TRoomsUpdatedPacket } from './rooms/rooms-updated';
import { TErrorPacket } from './util/error';

type PacketParserFunction = (obj: any) => Packet;

const packetTypeMap: { [key: string]: PacketParserFunction } = {
  // SAMPLE: (obj: any) => obj as SamplePacket,
  ROOMS_UPDATED: (obj: any) => obj as TRoomsUpdatedPacket,
  PLAYER_JOINED: (obj: any) => obj as TPlayerJoinedPacket,
  ERROR: (obj: any) => obj as TErrorPacket,
  STATUS: (obj: any) => obj as TStatusPacket,
  PLAYER_LEFT: (obj: any) => obj as TPlayerLeftPacket,
  START: (obj: any) => obj as TStartPacket,
  // Add entries for other packet types
};

export { packetTypeMap };
