import { animate, style, transition, trigger } from "@angular/animations";

export const animations = [
    trigger('opacity', [
        transition(':enter', [
            style({ opacity: 0 }),
            animate('300ms ease-out', style({ opacity: 1 })),
        ]),
        transition(':leave', [
            style({ opacity: 1 }),
            animate('200ms ease-in', style({ opacity: 0 })),
        ]),
    ]),
    trigger('opacityTranslateY', [
        transition(':enter', [
            style({ opacity: 0, transform: 'translateY(1rem)' }),
            animate(
                '300ms ease-out',
                style({ opacity: 1, transform: 'translateY(0)' })
            ),
        ]),
        transition(':leave', [
            style({ opacity: 1, transform: 'translateY(0)' }),
            animate(
                '200ms ease-in',
                style({ opacity: 0, transform: 'translateY(1rem)' })
            ),
        ]),
    ]),
];