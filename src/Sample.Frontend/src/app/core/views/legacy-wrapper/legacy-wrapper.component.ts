import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-legacy-wrapper',
  standalone: true,
  imports: [CommonModule],
  template: `
    <iframe [src]="urlSafe"></iframe>
  `
})
export class LegacyWrapperComponent {
  
  urlSafe: SafeResourceUrl | undefined;

  activatedRoute = inject(ActivatedRoute);
  sanitizer = inject(DomSanitizer);

  ngOnInit() {
    this.urlSafe= this.sanitizer.bypassSecurityTrustResourceUrl(this.activatedRoute.snapshot.data['url']);
  }
}
