import { Component, input } from '@angular/core';
import { animations } from './animations';
import { SharedModule } from '../../shared.module';

@Component({
  selector: 'app-modal-template',
  imports: [SharedModule],
  templateUrl: './modal-template.component.html',
  animations: animations,
  styles: ``
})
export class ModalTemplateComponent {

  public label = input<string>();

  public visible: boolean = false;

  constructor() {
    this.visible = false;
  }

  public open(): void {
    this.visible = true;
  }

  public close() {
    this.visible = false;
  }
}
