import { Packet } from '../packets/packet';
import { TErrorPacket } from '../packets/util/error';
import { PacketService } from '../services/packet/packet.service';
import { ToastService } from '../services/toast/toast.service';
import { parsePacket } from './parsePacket';

export abstract class Handler {
  protected readonly handler = new Map<string, (packet: Packet) => void>();

  constructor(
    private readonly service: PacketService,
    private readonly toaster: ToastService
  ) {
    this.service.receiveMessage().subscribe(this.handlePacket.bind(this));

    this.handler.set('ERROR', this.handleErrorPacket.bind(this));
  }

  private handlePacket(message: string) {
    console.log(message);
    const packet = parsePacket(message);
    const handler = this.handler.get(packet.Type);

    if (!handler) {
      return;
    }

    handler(packet);
  }

  protected handleErrorPacket(packet: Packet) {
    const parsed = packet as TErrorPacket;
    this.toaster.error(`${parsed.Message}`);
  }
}
