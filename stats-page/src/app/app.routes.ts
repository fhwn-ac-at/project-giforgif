import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home',
  },
  {
    path: 'home',
    loadComponent: () =>
      import('./features/home/pages/home.component').then(
        (c) => c.HomeComponent
      ),
  },
  {
    path: '**',
    redirectTo: 'home',
  },
];
