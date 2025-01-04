import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-avatar',
  imports: [],
  templateUrl: './avatar.component.html',
  styles: ``,
})
export class AvatarComponent {
  @Input({ required: true })
  public players: string[] | undefined;
}
