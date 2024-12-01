import { Packet } from "../packets/packet";
import { packetTypeMap } from "../packets/packet-map";

export function parsePacket(json: string): Packet {
    const obj = JSON.parse(JSON.stringify(json));
  
    const parser = packetTypeMap[obj.type];
  
    if (!parser) {
      throw new Error(`Unknown packet type: ${obj.Type}`);
    }
  
    return parser(obj);
  }
  