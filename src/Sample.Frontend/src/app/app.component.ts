import { Component, effect, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterOutlet } from '@angular/router';
import { HateoasClient, ResourceBase } from 'fancy-ngx-hateoas-client';
import { AppState, ROOT_MODEL_URL } from './app.state';
import { HypermediaSignal } from 'fancy-ngrx-hypermedia-models';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterOutlet],
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'Flight42';
  rootVm$: Promise<ResourceBase>;
  currentUserInfo: any = {};

  appState = inject(AppState);
  rootVmSignal = this.appState.getOrLoadModel(ROOT_MODEL_URL) as HypermediaSignal<ResourceBase>;

  constructor(hateoasClient: HateoasClient) {

    this.rootVm$ = hateoasClient.fetch("http://localhost:5100/api")

    this.rootVm$.then(async (rootVm: ResourceBase) => {
      this.currentUserInfo = await rootVm.fetch_userinfo?.();
    });

    effect(() => {
      console.log('Root model loaded', this.rootVmSignal());
    });

  }

  logIn() {
    window.location.href  = './login?redirectUri=' + encodeURIComponent(window.origin);
  }

  logOut() {
    window.location.href  = './logout';
  }
}
