import { Component, inject } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { FlightConnectionCardComponent } from '../../presenters/flight-connection-card/flight-connection-card.component';
import { FlightTimesCardComponent } from '../../presenters/flight-times-card/flight-times-card.component';
import { FlightOperatorCardComponent } from '../../presenters/flight-operator-card/flight-operator-card.component';
import { ActivatedRoute } from '@angular/router';
import { AppState } from 'src/app/app.state';

@Component({
  standalone: true,
  selector: 'admin-create-flight',
  imports: [CommonModule, FlightConnectionCardComponent, FlightTimesCardComponent, FlightOperatorCardComponent],
  templateUrl: './create.component.html',
})
export class CreateComponent {

  activatedRoute = inject(ActivatedRoute);
  appState = inject(AppState);

  viewModel = this.appState.LoadCreateVm(this.activatedRoute.snapshot.params['url']);

  private location = inject(Location);

  save() {
    // ToDo: Migrate actions to lib
    // this.viewModel?.create().then(() => {
    //   this.location.back();
    // });
  }
}
