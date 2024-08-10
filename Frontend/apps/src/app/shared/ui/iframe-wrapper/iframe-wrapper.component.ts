import { Component, inject, signal } from '@angular/core';
import { SafeResourceUrl, DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-iframe-wrapper',
  standalone: true,
  imports: [],
  templateUrl: './iframe-wrapper.component.html'
})
export class IframeWrapperComponent {
  activatedRoute = inject(ActivatedRoute);
  sanitizer = inject(DomSanitizer);

  safeUrl = signal<SafeResourceUrl | undefined>(undefined);

  ngOnInit() {
    this.safeUrl.set(this.sanitizer.bypassSecurityTrustResourceUrl(this.activatedRoute.snapshot.data['url']));
  }
}
