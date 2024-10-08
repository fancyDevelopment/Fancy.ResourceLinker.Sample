import { Routes } from '@angular/router';
import { CORE_ROUTES } from './core/core.routes';
import { FLIHGT_ROUTES } from './flight/flight.routes';
import { PASSENGER_ROUTES } from './passenger/passenter.routes';

export const routes: Routes = [{
    path: '',
    pathMatch: 'full',
    redirectTo: '/core'
}, {
    path: "core",
    children: CORE_ROUTES
}, {
    path: "flight",
    children: FLIHGT_ROUTES
}, {
    path: "passenger",
    children: PASSENGER_ROUTES
}, {
    path: '**',
    redirectTo: '/core'
}];
