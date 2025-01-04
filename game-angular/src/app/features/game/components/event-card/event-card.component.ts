import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-event-card',
  imports: [],
  templateUrl: './event-card.component.html',
  styles: ``,
})
export class EventCardComponent {
  protected title: string = 'Ereigniss';
  protected content: string = '';
  protected visible = false;

  public open(title: string, content: string) {
    this.title = title;
    this.content = content;
    this.visible = true;
  }

  public close() {
    this.visible = false;
  }
}
