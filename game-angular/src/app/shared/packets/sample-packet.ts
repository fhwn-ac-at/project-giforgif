import { Packet } from './packet';

export interface SamplePacket extends Packet {
  camelCase: number;
  samplE_INTEGER: number;
  samplE_STRING: string;
}
