import { Component, inject, Inject } from '@angular/core';
import { Handler } from '../../../shared/class/handler';
import { PacketService } from '../../../shared/services/packet/packet.service';
import { ToastService } from '../../../shared/services/toast/toast.service';
import { Packet } from '../../../shared/packets/packet';
import { WantStatusPacket } from '../../../shared/packets/lobby/want-status';
import { TStatusPacket } from '../../../shared/packets/lobby/status';
import { TPlayerJoinedPacket } from '../../../shared/packets/rooms/player-joined';
import { TPlayerLeftPacket } from '../../../shared/packets/rooms/player-left';
import { LeaveRoomPacket } from '../../../shared/packets/rooms/leave-room';
import { Router } from '@angular/router';

@Component({
  selector: 'app-lobby',
  imports: [],
  templateUrl: './lobby.component.html',
  styles: ``,
})
export class LobbyComponent extends Handler {
  protected me: string = 'Flask';
  protected players: string[] = ['mightyunicorn', 'onionturtle', 'megaonioun'];

  private readonly router = inject(Router);

  constructor(
    private readonly signalRService: PacketService,
    private readonly toastService: ToastService
  ) {
    super(signalRService, toastService);

    this.handler.set('STATUS', this.handleLobbyStatusPacket.bind(this));
    this.handler.set('PLAYER_JOINED', this.handlePlayerJoinedPacket.bind(this));
    this.handler.set('PLAYER_LEFT', this.handlePlayerLeftPacket.bind(this));
    this.signalRService.sendPacket(new WantStatusPacket());
  }

  protected leaveLobby() {
    this.signalRService.sendPacket(new LeaveRoomPacket());
    this.router.navigate(['menu']);
  }

  private handlePlayerJoinedPacket(packet: Packet) {
    const parsed = packet as TPlayerJoinedPacket;
    this.players.push(`${parsed.PlayerName}`);
  }

  private handleLobbyStatusPacket(packet: Packet) {
    const parsed = packet as TStatusPacket;

    this.me = parsed.Me.Name;
    this.players = parsed.Players.map((player) => player.Name);
  }

  private handlePlayerLeftPacket(packet: Packet) {
    const parsed = packet as TPlayerLeftPacket;

    const index = this.players.findIndex((p) => p === parsed.PlayerName);
    this.players.splice(index, 1);
  }
}
