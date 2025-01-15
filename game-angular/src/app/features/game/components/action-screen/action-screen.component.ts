import { Component } from '@angular/core';

@Component({
  selector: 'app-action-screen',
  imports: [],
  templateUrl: './action-screen.component.html',
  styles: [
    `
      .fade-in {
        animation: fadeIn 0.3s ease-in-out;
      }

      @keyframes fadeIn {
        from {
          opacity: 0;
        }
        to {
          opacity: 1;
        }
      }
    `,
  ],
})
export class ActionScreenComponent {
  public isShowing: boolean = false;
  public src: string = '';

  public open(src: string) {
    this.src = src;
    this.isShowing = true;

    setTimeout(() => {
      this.isShowing = false;
    }, 2000);
  }
}
