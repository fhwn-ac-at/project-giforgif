import {
  Component,
  Input,
  OnChanges,
  OnDestroy,
  SimpleChanges,
} from '@angular/core';
import { interval, Subscription } from 'rxjs';
import { Player } from '../../../../shared/types/game/player';

@Component({
  selector: 'app-dice',
  imports: [],
  templateUrl: './dice.component.html',
  styles: ``,
})
export class DiceComponent implements OnChanges, OnDestroy {
  public diced: number | null = null;
  public player: Player | null = null;

  protected animationNumber = 3;
  protected visible = false;
  private animationSubscription: Subscription | null = null;

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes['diced'] && changes['diced'].currentValue !== null) {
      this.stopDicing();
      this.animationNumber = this.diced!;
    }
  }

  public ngOnDestroy(): void {
    this.stopDicing();
  }

  public setDiced(value: number, player: Player) {
    this.diced = value;
    this.player = player;

    setTimeout(() => {
      this.stopDicing();
    }, 1000);
  }

  public startDicing() {
    if (this.animationSubscription) {
      console.warn('Dicing animation is already running.');
      return;
    }

    this.visible = true;

    this.animationSubscription = interval(100).subscribe(() => {
      this.animationNumber = this.getRandomNumber(1, 6);
    });
  }

  private stopDicing(): void {
    if (this.animationSubscription) {
      this.animationSubscription.unsubscribe();
      this.animationSubscription = null;
    }

    this.player = null;
    this.diced = null;
    this.visible = false;
  }

  private getRandomNumber(min: number, max: number): number {
    return Math.floor(Math.random() * (max - min + 1)) + min;
  }
}
