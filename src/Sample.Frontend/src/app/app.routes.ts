import { Routes } from '@angular/router';
import { HomeComponent } from './core/views/home/home.component';
import { SearchComponent } from './flight/views/search/flight-search.component';
import { EditComponent } from './flight/views/edit/flight-edit.component';
import { CreateComponent } from './flight/views/create/create.component';
import { LegacyWrapperComponent } from './core/views/legacy-wrapper/legacy-wrapper.component';

export const APP_ROUTES: Routes = [{
    path: 'home',
    component: HomeComponent
  }, {
    path: 'flight/search/:url',
    component: SearchComponent,
  }, {
    path: 'flight/edit/:url',
     component: EditComponent,
  }, {
    path: 'flight/create/:url',
    component: CreateComponent
  },{
    path: 'passenger/search',
    component: LegacyWrapperComponent,
    data: {
      url: 'http://localhost:5100/Sample.LegacyApp/passengersearch'
    }
  }, {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home'
}];
