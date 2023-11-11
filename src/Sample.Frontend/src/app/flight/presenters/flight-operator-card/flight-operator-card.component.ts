import { Component } from '@angular/core';
import { FlightEditCardBase } from '../flightEditCardBase.ts';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FlightOperator } from '../../models/models.js';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  selector: 'admin-flight-operator-card',
  templateUrl: './flight-operator-card.component.html'
})
export class FlightOperatorCardComponent extends FlightEditCardBase<FlightOperator> {
  override reset(): void {
    this.viewModel?.patch({ shortName: '' });
  }
}
