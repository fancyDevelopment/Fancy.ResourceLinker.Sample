import { Component } from '@angular/core';
import { FlightEditCardBase } from '../flightEditCardBase.ts';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  selector: 'admin-flight-times-card',
  templateUrl: './flight-times-card.component.html'
})
export class FlightTimesCardComponent extends FlightEditCardBase {
}
