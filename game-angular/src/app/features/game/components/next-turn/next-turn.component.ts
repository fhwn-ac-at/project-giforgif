import { Component } from '@angular/core';

@Component({
  selector: 'app-next-turn',
  imports: [],
  templateUrl: './next-turn.component.html',
  styles: ``,
})
export class NextTurnComponent {
  protected name: string = '';
  protected visible = false;

  public open(name: string) {
    this.name = name;
    this.visible = true;
    setTimeout(() => {
      this.visible = false;
    }, 1500);
  }
}
