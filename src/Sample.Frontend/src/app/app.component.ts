import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterOutlet } from '@angular/router';
import { HateoasClient, ResourceBase } from 'fancy-ngx-hateoas-client';

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

  constructor(hateoasClient: HateoasClient) {

    this.rootVm$ = hateoasClient.fetch("http://localhost:5100/api")

    this.rootVm$.then(async (rootVm: ResourceBase) => {
      this.currentUserInfo = await rootVm.fetch_userinfo?.();
    });

  }

  logIn() {
    window.location.href  = './login?redirectUri=' + encodeURIComponent(window.origin);
  }

  logOut() {
    window.location.href  = './logout';
  }
}
