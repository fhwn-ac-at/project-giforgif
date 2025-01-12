import { animate, style, transition, trigger } from '@angular/animations';

export const animations = [
  trigger('opacityTranslateY', [
    transition(':enter', [
      style({ transform: 'translateY(0.5rem)', opacity: 0 }),
      animate(
        '300ms ease-out',
        style({ transform: 'translateY(0)', opacity: 1 })
      ),
    ]),
    transition(':leave', [
      style({ transform: 'translateY(0)', opacity: 1 }),
      animate(
        '100ms ease-in',
        style({ transform: 'translateY(0.5rem)', opacity: 0 })
      ),
    ]),
  ]),
];
