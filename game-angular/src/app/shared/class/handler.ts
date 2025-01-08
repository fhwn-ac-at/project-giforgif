import { Subscription } from 'rxjs';
import { Packet } from '../packets/packet';
import { TErrorPacket } from '../packets/util/error';
import { PacketService } from '../services/packet/packet.service';
import { ToastService } from '../services/toast/toast.service';
import { parsePacket } from './parsePacket';
import { Queue } from './queue';

// Maybe machen mit queue oder das ich ihm erst sage "YO jetzt will ich ein neues packet"
export abstract class Handler {
  protected readonly handler = new Map<string, (packet: Packet) => void>();
  protected readonly queue = new Queue<string>();
  private messageSubscription: Subscription;

  constructor(
    private readonly service: PacketService,
    private readonly toaster: ToastService
  ) {
    this.messageSubscription = this.service
      .receiveMessage()
      .subscribe(this.handlePacket.bind(this));
    this.handler.set('ERROR', this.handleErrorPacket.bind(this));
  }

  protected startPacketHandler() {}

  private handlePacket(message: string) {
    console.log(message)
    const packet = parsePacket(message);
    const handler = this.handler.get(packet.Type);

    console.log(packet);

    if (!handler) {
      return;
    }

    handler(packet);
  }

  protected handleErrorPacket(packet: Packet) {
    const parsed = packet as TErrorPacket;
    this.toaster.error(`${parsed.Message}`);
  }

  protected stopHandler() {
    this.messageSubscription.unsubscribe();
  }
}
