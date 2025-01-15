import { Component } from '@angular/core';

@Component({
  selector: 'app-event-card',
  imports: [],
  templateUrl: './event-card.component.html',
  styles: ``,
})
export class EventCardComponent {
  protected title: string = 'Ereigniss';
  protected content: string = '';
  public visible = false;

  public open(title: string, content: string) {
    this.title = title;
    this.content = content;
    this.visible = true;

    setTimeout(() => {
      this.close();
    }, 4000);
  }

  public close() {
    this.visible = false;
  }
}
