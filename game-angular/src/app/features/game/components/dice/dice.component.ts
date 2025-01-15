import {
  Component,
  EventEmitter,
  OnChanges,
  OnDestroy,
  Output,
  SimpleChanges,
} from '@angular/core';
import { Player } from '@shared/types/game/player';
import { interval, Subscription } from 'rxjs';

@Component({
  selector: 'app-dice',
  imports: [],
  templateUrl: './dice.component.html',
  styles: ``,
})
export class DiceComponent implements OnChanges, OnDestroy {
  @Output()
  public onStoppedDicing: EventEmitter<number> = new EventEmitter();

  public diced: number | null = null;
  public player: Player | null = null;

  public isDicing: boolean = false;

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
    this.isDicing = true;

    this.animationSubscription = interval(100).subscribe(() => {
      this.animationNumber = this.getRandomNumber(1, 12);
    });
  }

  private stopDicing(): void {
    if (this.animationSubscription) {
      this.animationSubscription.unsubscribe();
      this.animationSubscription = null;
    }

    this.onStoppedDicing.emit(this.diced!);
    this.player = null;
    this.diced = null;
    this.isDicing = false;
    this.visible = false;
  }

  private getRandomNumber(min: number, max: number): number {
    return Math.floor(Math.random() * (max - min + 1)) + min;
  }
}
