import { Component } from '@angular/core';
import { AvatarComponent } from '../avatar/avatar.component';
import { Tile } from './tile';
import { SharedModule } from '../../../../shared/shared.module';

@Component({
  selector: 'app-board',
  imports: [AvatarComponent, SharedModule],
  templateUrl: './board.component.html',
  styles: ``,
})
export class BoardComponent {
  public board = [
    [
      new Tile(21, 'bg-black'),
      new Tile(22, 'bg-black'),
      new Tile(23, 'bg-black'),
      new Tile(24, 'bg-black'),
      new Tile(25, 'bg-black', "s"),
      new Tile(26, 'bg-black'),
      new Tile(27, 'bg-black'),
      new Tile(28, 'bg-black'),
      new Tile(29, 'bg-black'),
      new Tile(30, 'bg-black'),
      new Tile(31, 'bg-black'),
    ],
    [new Tile(20, 'bg-black'), new Tile(32, 'bg-black')],
    [new Tile(19, 'bg-black'), new Tile(33, 'bg-black')],
    [new Tile(18, 'bg-black'), new Tile(34, 'bg-black', "s")],
    [new Tile(17, 'bg-black', "s"), new Tile(35, 'bg-black')],
    [new Tile(16, 'bg-black'), new Tile(36, 'bg-black')],
    [new Tile(15, 'bg-black'), new Tile(37, 'bg-black')],
    [new Tile(14, 'bg-black'), new Tile(38, 'bg-black')],
    [new Tile(13, 'bg-black'), new Tile(39, 'bg-black')],
    [new Tile(12, 'bg-black'), new Tile(40, 'bg-black')],
    [
      new Tile(11, 'bg-black'),
      new Tile(10, 'bg-black'),
      new Tile(9, 'bg-black'),
      new Tile(8, 'bg-black'),
      new Tile(7, 'bg-black'),
      new Tile(6, 'bg-black'),
      new Tile(5, 'bg-black', "red"),
      new Tile(4, 'bg-black'),
      new Tile(3, 'bg-black'),
      new Tile(2, 'bg-black'),
      new Tile(1, 'bg-black'),
    ],
  ];

  public fields = new Map<number, string[]>();

  public test() {
    this.fields.set(35, ['Capibara', '1', '2', '3']);
    this.fields.set(33, ['Capibara']);
    this.fields.set(40, ['Capibara']);
    this.fields.set(10, ['Capibara']);
  }
}
