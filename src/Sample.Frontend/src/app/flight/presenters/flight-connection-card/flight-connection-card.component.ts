import { Component } from '@angular/core';
import { FlightEditCardBase } from '../flightEditCardBase.ts';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  selector: 'admin-flight-connection-card',
  templateUrl: './flight-connection-card.component.html'
})
export class FlightConnectionCardComponent extends FlightEditCardBase {
}
