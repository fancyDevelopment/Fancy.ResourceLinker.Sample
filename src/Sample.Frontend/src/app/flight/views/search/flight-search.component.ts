import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FlightSummaryCardComponent } from '../../presenters/flight-summary-card/flight-summary-card.component';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AppState } from 'src/app/app.state';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, FlightSummaryCardComponent],
  selector: 'admin-flight-search',
  templateUrl: './search.component.html'
})
export class SearchComponent {

  appState = inject(AppState);
  activatedRoute = inject(ActivatedRoute);

  viewModel: any = null; 

  constructor() {
    this.activatedRoute.params.subscribe(params => {
      this.viewModel = this.appState.LoadSearchVm(params['url']);
    });
  }

}
