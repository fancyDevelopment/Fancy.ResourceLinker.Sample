import { AppState } from './../../../app.state';
import { computed, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FlightConnectionCardComponent } from '../../presenters/flight-connection-card/flight-connection-card.component';
import { FlightTimesCardComponent } from '../../presenters/flight-times-card/flight-times-card.component';
import { FlightOperatorCardComponent } from '../../presenters/flight-operator-card/flight-operator-card.component';
import { FlightPriceCardComponent } from '../../presenters/flight-price-card/flight-price-card.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  standalone: true,
  imports: [
    CommonModule, 
    FlightConnectionCardComponent, 
    FlightTimesCardComponent, 
    FlightOperatorCardComponent, 
    FlightPriceCardComponent
  ],
  selector: 'admin-flight-edit',
  templateUrl: './edit.component.html'
})
export class EditComponent {

  activatedRoute = inject(ActivatedRoute);
  appState = inject(AppState);

  viewModel = this.appState.LoadFlightEditVm(this.activatedRoute.snapshot.params['url'], true);

  flightRoute = computed(() => this.viewModel.connection.from() + ' - ' + this.viewModel.connection.to());
  flightRouteViaStore = this.appState.flightRouteViaStore;

  constructor() {
    effect(() => {
      console.log('Flight Route: ' + this.flightRoute());
    });
  }
}
