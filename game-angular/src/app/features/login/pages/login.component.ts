import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { RegisterPacket } from '../../../shared/packets/register/register-packet';
import { PacketService } from '../../../shared/services/packet/packet.service';
import { ToastService } from '../../../shared/services/toast/toast.service';
import { SharedModule } from '../../../shared/shared.module';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [SharedModule],
  templateUrl: './login.component.html',
  styles: ``,
})
export class LoginComponent {
  protected nameForm: FormGroup;

  private readonly fb = inject(FormBuilder);
  private readonly signalRService = inject(PacketService);
  private readonly toastService = inject(ToastService);
  private readonly router = inject(Router);

  constructor() {
    this.nameForm = this.fb.group({
      playerName: [null],
    });
  }

  protected initialServerConnection() {
    this.signalRService.startConnection().subscribe(
      () => {
        const packet = new RegisterPacket();
        packet.PlayerName = this.nameForm.controls['playerName'].value;
        this.signalRService.sendPacket<RegisterPacket>(packet);
        this.toastService.success('Erfolgreich mit dem Server verbunden');
        this.router.navigate(['menu']);
      },
      () => {
        this.toastService.error(
          'Ein Fehler ist aufgetreten, bitte versuchen Sie es erneut'
        );
      }
    );
  }
}
