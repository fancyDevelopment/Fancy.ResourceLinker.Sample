import { Component, Input } from '@angular/core';
import { FlightEditCardBase } from '../flightEditCardBase.ts';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FlightConnection } from '../../models/models.js';
import { DeepPatchableSignal } from 'fancy-ngrx-deep-patchable-signal';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  selector: 'admin-flight-connection-card',
  templateUrl: './flight-connection-card.component.html'
})
export class FlightConnectionCardComponent extends FlightEditCardBase<FlightConnection> {
  reset() {
    this.viewModel?.patch({ from: '', to: '', icaoFrom: '', icaoTo: ''});
  }
}
