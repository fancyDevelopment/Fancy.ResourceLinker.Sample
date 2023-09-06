import { Directive, Input } from "@angular/core";
import { timer } from "rxjs";

@Directive()
export abstract class FlightEditCardBase {
  showSuccess = false;
  showError = false;
  @Input() viewModel: any = {};

  save() {
    this.viewModel.update().then(() => {
      this.showSuccess = true;
      timer(3000).subscribe(() => { this.showSuccess = false; });
    })
    .catch(() => {
      this.showError = true;
      timer(3000).subscribe(() => { this.showError = false; });
    });
  }
}
