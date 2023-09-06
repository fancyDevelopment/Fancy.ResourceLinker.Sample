import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ViewBase } from 'fancy-ngx-hateoas-client';
import { FlightSummaryCardComponent } from '../../presenters/flight-summary-card/flight-summary-card.component';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, FlightSummaryCardComponent],
  selector: 'admin-flight-search',
  templateUrl: './search.component.html'
})
export class SearchComponent extends ViewBase {
}
