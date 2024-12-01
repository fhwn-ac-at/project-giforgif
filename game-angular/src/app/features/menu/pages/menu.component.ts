import { Component, inject } from '@angular/core';
import { ModalTemplateComponent } from '../../../shared/components/modal-template/modal-template.component';
import { RoomService } from '../../../shared/services/room/room.service';
import { parsePacket, Perform } from '../../../shared/class';
import { Room } from '../../../shared/types';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SharedModule } from '../../../shared/shared.module';
import { PacketService } from '../../../shared/services/packet/packet.service';
import { SamplePacket } from '../../../shared/packets/sample-packet';

@Component({
  selector: 'app-menu',
  imports: [ModalTemplateComponent, SharedModule],
  templateUrl: './menu.component.html',
  styles: ``,
})
export class MenuComponent {
  protected rooms = new Perform<Room[]>();
  protected create = new Perform<Room>();
  protected roomForm: FormGroup;

  private readonly roomService = inject(RoomService);
  private readonly fb = inject(FormBuilder);
  private readonly signalRService = inject(PacketService);

  constructor() {
    this.fetchRooms();
    this.roomForm = this.fb.group({
      roomName: [null, Validators.required],
    });
  }

  protected fetchRooms() {
    this.rooms.load(this.roomService.getRooms(), {
      toast: {
        error: 'Ein Fehler ist aufgetreten, bitte versuchen Sie es erneut',
      },
    });
  }

  protected createRoom() {
    if (this.roomForm.invalid) {
      return;
    }

    this.create.load(this.roomService.createRoom(this.roomForm.value), {
      toast: {
        info: 'Raum erfolgreich erstellt',
        error: 'Ein Fehler ist aufgetreten, bitte versuchen Sie es erneut',
      },
      touch: async () => {
        this.fetchRooms();
      },
    });
  }

  protected joinRoom() {
    this.signalRService.startConnection().subscribe(() => {
      this.signalRService.receiveMessage().subscribe((message) => {
        console.log(message);

        const packet = parsePacket(message);

        if (packet.type == 'SAMPLE') {
          const sample = packet as SamplePacket;
          console.log('SAMÖ', sample);
          console.log(sample.camelCase);
        }
      });
    });
  }

  protected test() {
    this.signalRService.tgest('Hallo');
  }
}
