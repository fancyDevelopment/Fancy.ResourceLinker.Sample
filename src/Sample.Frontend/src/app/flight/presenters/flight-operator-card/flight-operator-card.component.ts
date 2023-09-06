import { Component } from '@angular/core';
import { FlightEditCardBase } from '../flightEditCardBase.ts';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  selector: 'admin-flight-operator-card',
  templateUrl: './flight-operator-card.component.html'
})
export class FlightOperatorCardComponent extends FlightEditCardBase {
}
