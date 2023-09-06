import { Component, inject } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { ViewBase } from 'fancy-ngx-hateoas-client';
import { FlightConnectionCardComponent } from '../../presenters/flight-connection-card/flight-connection-card.component';
import { FlightTimesCardComponent } from '../../presenters/flight-times-card/flight-times-card.component';
import { FlightOperatorCardComponent } from '../../presenters/flight-operator-card/flight-operator-card.component';

@Component({
  standalone: true,
  selector: 'admin-create-flight',
  imports: [CommonModule, FlightConnectionCardComponent, FlightTimesCardComponent, FlightOperatorCardComponent],
  templateUrl: './create.component.html',
})
export class CreateComponent extends ViewBase {

  private location = inject(Location);

  save() {
    this.viewModel?.create().then(() => {
      this.location.back();
    });
  }
}
