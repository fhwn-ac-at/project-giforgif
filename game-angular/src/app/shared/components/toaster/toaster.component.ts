import { Component, inject } from '@angular/core';
import { ToastService } from '../../services/toast/toast.service';
import { SharedModule } from '../../shared.module';
import { animations } from './animation';

@Component({
  selector: 'app-toaster',
  imports: [SharedModule],
  templateUrl: './toaster.component.html',
  animations: animations,
  styles: ``,
})
export class ToasterComponent {
  protected toastService: ToastService = inject(ToastService);
}
