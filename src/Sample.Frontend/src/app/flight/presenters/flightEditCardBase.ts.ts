import { Directive, Input } from "@angular/core";
import { DeepPatchableSignal } from "fancy-ngrx-deep-patchable-signal";

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
