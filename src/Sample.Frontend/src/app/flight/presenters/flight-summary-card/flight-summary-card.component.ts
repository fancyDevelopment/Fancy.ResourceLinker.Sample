import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  selector: 'admin-flight-summary-card',
  templateUrl: './flight-summary-card.component.html'
})
export class FlightSummaryCardComponent {
  @Input() flight: any = {};
}
