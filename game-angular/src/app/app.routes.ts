import { Routes } from '@angular/router';
import { AuthGuard } from './core/auth/auth.guard';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'login',
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./features/login/pages/login.component').then(
        (c) => c.LoginComponent
      ),
  },
  {
    path: 'lobby',
    loadComponent: () =>
      import('./features/lobby/pages/lobby.component').then(
        (c) => c.LobbyComponent
      ),
    canActivate: [AuthGuard]
  },
  {
    path: 'menu',
    loadComponent: () =>
      import('./features/menu/pages/menu.component').then(
        (c) => c.MenuComponent
      ),
    canActivate: [AuthGuard],
  },
  {
    path: 'game',
    loadComponent: () =>
      import('./features/game/pages/game.component').then(
        (c) => c.GameComponent
      ),
    canActivate: [AuthGuard]
  },
];
