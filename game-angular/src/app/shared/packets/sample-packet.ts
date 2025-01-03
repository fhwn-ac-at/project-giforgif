import { IPacket } from './packet';

export interface SamplePacket extends IPacket {
  camelCase: number;
  samplE_INTEGER: number;
  samplE_STRING: string;
}
