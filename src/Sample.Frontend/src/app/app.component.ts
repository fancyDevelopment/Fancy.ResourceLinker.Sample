import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterOutlet } from '@angular/router';
import { AppState, ROOT_MODEL_URL } from './app.state';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterOutlet],
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'Flight42';

  appState = inject(AppState);

  rootVm = this.appState.LoadRootVm(ROOT_MODEL_URL);
  currentUserInfo: any = this.rootVm.fetchLink('userInfo');

  logIn() {
    window.location.href  = './login?redirectUri=' + encodeURIComponent(window.origin);
  }

  logOut() {
    window.location.href  = './logout';
  }
}
