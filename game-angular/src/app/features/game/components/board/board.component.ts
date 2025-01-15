import { Component, inject } from '@angular/core';
import { AvatarComponent } from '../avatar/avatar.component';
import { BuyHouseComponent } from '../buy-house/buy-house.component';
import { GameService } from '@shared/services/game/game.service';
import { SharedModule } from '@shared/shared.module';

@Component({
  selector: 'app-board',
  imports: [AvatarComponent, SharedModule, BuyHouseComponent],
  templateUrl: './board.component.html',
  styles: ``,
})
export class BoardComponent {
  public readonly gameService = inject(GameService);

  public test() {
    // this.fields.set(35, ['Capibara', '1', '2', '3']);
    // this.fields.set(33, ['Capibara']);
    // this.fields.set(40, ['Capibara']);
    // this.fields.set(10, ['Capibara']);
  }
}
