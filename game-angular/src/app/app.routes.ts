import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: "",
        pathMatch: "full",
        redirectTo: "login"
    },
    {
        path: "login",
        loadComponent: () => import('./features/login/pages/login.component').then(c => c.LoginComponent)
    },
    {
        path: "lobby",
        loadComponent: () => import('./features/lobby/pages/lobby.component').then(c => c.LobbyComponent)
    },
    {
        path: "menu",
        loadComponent: () => import('./features/menu/pages/menu.component').then(c => c.MenuComponent)
    }
];
