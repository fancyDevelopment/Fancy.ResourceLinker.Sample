import { AppState, ROOT_MODEL_URL } from './../../../app.state';
import { CommonModule } from '@angular/common';
import { Component, effect, inject } from '@angular/core';
import { ResourceBase } from 'fancy-hateoas-client';
import { AppComponent } from '../../../app.component';
import * as signalR from '@microsoft/signalr';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'admin-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  viewModel : any = null;
  hubConnection = new signalR.HubConnectionBuilder().withUrl('http://localhost:5101/hubs/home').build();

  appState = inject(AppState);

   constructor(appComponent: AppComponent) {

      appComponent.rootVm$.then((vm: any) => {
         vm.fetch_homeVm().then((homeVm: ResourceBase) => {
            this.viewModel = homeVm;
         })
      });

      const rootVmSignal = this.appState.getOrLoadModel(ROOT_MODEL_URL);
      const homeVmSignal = rootVmSignal.fetchLink('homeVm');

      effect(() => {
         console.log('Root Model in Home', rootVmSignal());
         console.log('Home Model in Home', homeVmSignal());
      });
      
   }
}
