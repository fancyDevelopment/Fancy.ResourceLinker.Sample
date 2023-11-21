import { AppState, ROOT_MODEL_URL } from './../../../app.state';
import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { DynamicResource } from 'fancy-hateoas-client';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'admin-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
   appState = inject(AppState);
   rootVm = this.appState.LoadRootVm(ROOT_MODEL_URL);

   // Get a linked view model of the root view model
   viewModel: any = this.rootVm.fetchLink<DynamicResource>('homeVm');
}
