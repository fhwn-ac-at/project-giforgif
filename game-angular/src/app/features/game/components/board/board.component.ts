import { Component, inject } from '@angular/core';
import { AvatarComponent } from '../avatar/avatar.component';
import { Tile } from '../../../../shared/types/game/tile';
import { SharedModule } from '../../../../shared/shared.module';
import { House } from '../../../../shared/types/game/house';
import { Hotel } from '../../../../shared/types/game/hotel';
import { GameService } from '../../../../shared/services/game/game.service';

@Component({
  selector: 'app-board',
  imports: [AvatarComponent, SharedModule],
  templateUrl: './board.component.html',
  styles: ``,
})
export class BoardComponent {
  private readonly gameService = inject(GameService);
  protected board: Tile[][] = this.gameService.board;
  protected fields = this.gameService.fields;

  public test() {
    // this.fields.set(35, ['Capibara', '1', '2', '3']);
    // this.fields.set(33, ['Capibara']);
    // this.fields.set(40, ['Capibara']);
    // this.fields.set(10, ['Capibara']);
  }
}
