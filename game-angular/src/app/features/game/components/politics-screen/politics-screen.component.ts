import {
  Component,
  inject,
  OnChanges,
  OnDestroy,
  SimpleChanges,
} from '@angular/core';
import { GameService } from '@shared/services/game/game.service';
import { Player } from '@shared/types/game/player';
import { Subscription, interval } from 'rxjs';

@Component({
  selector: 'app-politics-screen',
  imports: [],
  templateUrl: './politics-screen.component.html',
  styles: ``,
})
export class PoliticsScreenComponent implements OnDestroy {
  public won: number | null = null;
  public visible = false;
  public gameService = inject(GameService);

  protected animationNumber = 3;
  private animationSubscription: Subscription | null = null;

  // constructor() {
  //   this.start();
  // }

  public ngOnDestroy(): void {
    this.stop();
  }

  public setWon(value: number) {
    this.won = value;
    this.animationNumber = this.won;
    this.stopAnimation();

    setTimeout(() => {
      this.stop();
    }, 5000);
  }

  public start() {
    if (this.animationSubscription) {
      console.warn('Rolling animation is already running.');
      return;
    }

    this.visible = true;

    this.animationSubscription = interval(100).subscribe(() => {
      this.animationNumber = (this.animationNumber + 1) % 5;
    });
  }

  private stopAnimation() {
    if (this.animationSubscription) {
      this.animationSubscription.unsubscribe();
      this.animationSubscription = null;
    }
  }

  private stop(): void {
    this.stopAnimation();
    this.visible = false;
  }
}
