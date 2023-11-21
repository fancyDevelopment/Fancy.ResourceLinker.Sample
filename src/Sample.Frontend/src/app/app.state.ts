import { signalStore, withComputed } from "@ngrx/signals";
import { FlightEditViewModel } from "./flight/models/models";
import { DynamicResource } from "fancy-hateoas-client";
import { computed } from "@angular/core";
import { withHypermediaModel } from "@angular-architects/ngrx-hateoas";

export const ROOT_MODEL_URL = "http://localhost:5100/api";

export const AppState = signalStore(
  { providedIn: 'root' },
  withHypermediaModel<DynamicResource, "RootVm">("RootVm"),
  withHypermediaModel<DynamicResource, "SearchVm">("SearchVm"),
  withHypermediaModel<DynamicResource, "CreateVm">("CreateVm"),
  withHypermediaModel<FlightEditViewModel, "FlightEditVm">("FlightEditVm"),
  withComputed(state => ({
    // Not working because no default values are provided and therefore store returns undefined for nested signals
    flightRouteViaStore: computed(() => state.FlightEditVm.connection.from() + ' - ' + state.FlightEditVm.connection.to()) 
  }))
);