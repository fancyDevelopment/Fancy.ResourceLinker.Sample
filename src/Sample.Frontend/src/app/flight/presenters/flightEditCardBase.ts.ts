import { DeepPatchableSignal } from "@angular-architects/ngrx-hateoas";
import { Directive, Input } from "@angular/core";

@Directive()
export abstract class FlightEditCardBase<T> {
  showSuccess = false;
  showError = false;
  canUpdate = true;
  @Input() viewModel: DeepPatchableSignal<T> | undefined;

  save() {
    
    // ToDo

  }

  abstract reset(): void;
}
