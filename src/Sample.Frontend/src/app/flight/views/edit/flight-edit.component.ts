import { computed } from '@angular/core';
import { patchState, withComputed } from '@ngrx/signals';
import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { TypedViewBase, ViewBase } from 'fancy-ngx-hateoas-client';
import { FlightConnectionCardComponent } from '../../presenters/flight-connection-card/flight-connection-card.component';
import { FlightTimesCardComponent } from '../../presenters/flight-times-card/flight-times-card.component';
import { FlightOperatorCardComponent } from '../../presenters/flight-operator-card/flight-operator-card.component';
import { FlightPriceCardComponent } from '../../presenters/flight-price-card/flight-price-card.component';
import { FlightEditViewModel } from '../../models/models';
import { signalStore, withMethods, withState } from '@ngrx/signals';
import { toDeepPatchableSignal } from 'fancy-ngrx-deep-patchable-signal';

export const FlightEditStore = signalStore(
  { providedIn: 'root' },
  withState({ flightEditVm: {} as FlightEditViewModel }),
  withComputed(state => ({
    // Does not work because of nullable values at initialization
    //flightRoute: computed(() => state.flightEditVm.connection.from() + ' - ' + state.flightEditVm.connection.to())
  })),
  withMethods((state) => {
    return {
      setFlightEditVm(vm: FlightEditViewModel) {
        patchState(state, { flightEditVm: vm });
      },
      getPatchableFlightEditVm() {
        return toDeepPatchableSignal(state, newVal => ({ flightEditVm: { ...state.flightEditVm(), ...newVal } }), state.flightEditVm);
      }
    }
  })
);

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
export class EditComponent extends TypedViewBase<FlightEditViewModel> {

  flightEditStore = inject(FlightEditStore);

  flightEditVm = this.flightEditStore.getPatchableFlightEditVm();

  connection = this.flightEditVm.connection;

  flightRoute = computed(() => this.flightEditVm.connection.from() + ' - ' + this.flightEditVm.connection.to());

  override viewModelOnLoaded(): void {
    if(this.viewModel) {
      this.flightEditStore.setFlightEditVm(this.viewModel);
    }
  }

}
