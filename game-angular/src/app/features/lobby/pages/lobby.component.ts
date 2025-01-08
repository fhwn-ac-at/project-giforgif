import { Component, inject, Inject, OnDestroy } from '@angular/core';
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
import { interval, Subscription } from 'rxjs';

@Component({
  selector: 'app-lobby',
  imports: [],
  templateUrl: './lobby.component.html',
  styles: ``,
})
export class LobbyComponent extends Handler implements OnDestroy {
  protected me: string = '';
  protected players: string[] = [];
  protected startingIn: number = 11;
  protected starting: boolean = false;

  private readonly router = inject(Router);
  private countdownSubscription: Subscription | null = null;

  constructor(
    private readonly signalRService: PacketService,
    private readonly toastService: ToastService
  ) {
    super(signalRService, toastService);

    this.handler.set('STATUS', this.handleLobbyStatusPacket.bind(this));
    this.handler.set('PLAYER_JOINED', this.handlePlayerJoinedPacket.bind(this));
    this.handler.set('PLAYER_LEFT', this.handlePlayerLeftPacket.bind(this));
    this.handler.set('START', this.handleStartPacket.bind(this));
    this.signalRService.sendPacket(new WantStatusPacket());
  }

  public ngOnDestroy(): void {
    this.stopHandler();
  }

  protected leaveLobby() {
    this.signalRService.sendPacket(new LeaveRoomPacket());
    this.handleStop();
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

    if (this.players.length < 3) {
      this.handleStop();
    }
  }

  private handleStartPacket(packet: Packet) {
    if (this.countdownSubscription) {
      this.countdownSubscription.unsubscribe();
    }

    this.starting = true;
    this.countdownSubscription = interval(1000).subscribe(() => {
      this.startingIn--;

      if (this.startingIn <= 0) {
        this.handleStop();
        this.router.navigate(['game']);
      }
    });
  }

  private handleStop() {
    if (this.countdownSubscription) {
      this.countdownSubscription.unsubscribe();
      this.countdownSubscription = null;
    }

    this.starting = false;
    this.startingIn = 11;
  }
}
