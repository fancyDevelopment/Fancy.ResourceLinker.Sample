import { Component } from '@angular/core';
import { FlightEditCardBase } from '../flightEditCardBase.ts';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FlightTimes } from '../../models/models.js';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  selector: 'admin-flight-times-card',
  templateUrl: './flight-times-card.component.html'
})
export class FlightTimesCardComponent extends FlightEditCardBase<FlightTimes> {
  override reset(): void {
    this.viewModel?.patch({ takeOff: '', landing: '' });
  }
}
