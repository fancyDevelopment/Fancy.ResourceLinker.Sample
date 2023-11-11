import { Component } from '@angular/core';
import { FlightEditCardBase } from '../flightEditCardBase.ts';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FlightPrice } from '../../models/models.js';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  selector: 'admin-flight-price-card',
  templateUrl: './flight-price-card.component.html'
})
export class FlightPriceCardComponent extends FlightEditCardBase<FlightPrice> {
  override reset(): void {
    this.viewModel?.patch({ basePrice: 0, seatReservationSurcharge: 0 });
  }
}
