import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ViewBase } from 'fancy-ngx-hateoas-client';
import { FlightConnectionCardComponent } from '../../presenters/flight-connection-card/flight-connection-card.component';
import { FlightTimesCardComponent } from '../../presenters/flight-times-card/flight-times-card.component';
import { FlightOperatorCardComponent } from '../../presenters/flight-operator-card/flight-operator-card.component';
import { FlightPriceCardComponent } from '../../presenters/flight-price-card/flight-price-card.component';

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
export class EditComponent extends ViewBase {
}
