import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { APP_ROUTES } from './app/app.routes';
import { provideHttpClient } from '@angular/common/http';
import { provideHateoasClient, withAngularRequestManager } from 'fancy-ngx-hateoas-client';

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(APP_ROUTES),
    provideHttpClient(),
    provideHateoasClient(withAngularRequestManager())
  ]
})
  .catch((err) => console.error(err));
