import { Component } from '@angular/core';
import { SharedModule } from '../../../shared/shared.module';
import { BoardComponent } from '../components/board/board.component';

@Component({
  selector: 'app-game',
  imports: [SharedModule, BoardComponent],
  templateUrl: './game.component.html',
  styles: ``
})
export class GameComponent {

}
