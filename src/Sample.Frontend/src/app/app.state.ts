import { computed } from "@angular/core";
import { signalStore, withComputed } from "@ngrx/signals";
import { withHypermediaModels } from "fancy-ngrx-hypermedia-models";

export const ROOT_MODEL_URL = "http://localhost:5100/api";

export const AppState = signalStore(
    { providedIn: 'root' },
    withHypermediaModels(ROOT_MODEL_URL)
  );