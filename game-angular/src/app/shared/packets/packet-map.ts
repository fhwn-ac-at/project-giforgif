import { Packet } from './packet';
import { SamplePacket } from './sample-packet';

type PacketParserFunction = (obj: any) => Packet;

const packetTypeMap: { [key: string]: PacketParserFunction } = {
  SAMPLE: (obj: any) => obj as SamplePacket,
  // Add entries for other packet types
};

export { packetTypeMap };
