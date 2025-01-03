import { Packet } from "../packets/packet";
import { packetTypeMap } from "../packets/packet-map";

export function parsePacket(json: string): Packet {
    const obj = JSON.parse(json);
    const parser = packetTypeMap[obj.Type];
  
    if (!parser) {
      throw new Error(`Unknown packet type: ${obj.Type}`);
    }
  
    return parser(obj);
  }
  