import { Component, inject, OnDestroy } from '@angular/core';
import { ModalTemplateComponent } from '../../../shared/components/modal-template/modal-template.component';
import { parsePacket, Perform } from '../../../shared/class';
import { Room } from '../../../shared/types';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SharedModule } from '../../../shared/shared.module';
import { PacketService } from '../../../shared/services/packet/packet.service';
import { Packet } from '../../../shared/packets/packet';
import { CreateRoomPacket } from '../../../shared/packets/rooms/create-room';
import { TRoomsUpdatedPacket } from '../../../shared/packets/rooms/rooms-updated';
import { TErrorPacket } from '../../../shared/packets/util/error';
import { ToastService } from '../../../shared/services/toast/toast.service';
import { JoinRoomPacket } from '../../../shared/packets/rooms/join-room';
import { Handler } from '../../../shared/class/handler';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu',
  imports: [ModalTemplateComponent, SharedModule],
  templateUrl: './menu.component.html',
  styles: ``,
})
export class MenuComponent extends Handler implements OnDestroy {
  protected rooms: Room[] = [];
  protected create = new Perform<Room>();
  protected roomForm: FormGroup;

  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);

  constructor(
    private readonly signalRService: PacketService,
    private readonly toastService: ToastService
  ) {
    super(signalRService, toastService);
    this.roomForm = this.fb.group({
      roomName: [null, Validators.required],
    });

    this.handler.set('ROOMS_UPDATED', this.handleRoomsUpdatedPacket.bind(this));
    this.handler.set('PLAYER_JOINED', this.handleJoinedRoomPacket.bind(this));
  }

  public ngOnDestroy(): void {
    this.stopHandler();
  }

  protected createRoom() {
    if (this.roomForm.invalid) {
      return;
    }

    const packet = new CreateRoomPacket();
    packet.RoomName = this.roomForm.controls['roomName'].value;

    this.signalRService.sendPacket(packet);
  }

  protected joinRoom(room: string) {
    const packet = new JoinRoomPacket();
    packet.RoomName = room;
    this.signalRService.sendPacket(packet);
  }

  protected handleJoinedRoomPacket(packet: Packet) {
    this.router.navigate(['lobby']);
  }

  protected handleRoomsUpdatedPacket(packet: Packet) {
    const parsed = packet as TRoomsUpdatedPacket;
    this.rooms = parsed.Rooms;
  }
}
