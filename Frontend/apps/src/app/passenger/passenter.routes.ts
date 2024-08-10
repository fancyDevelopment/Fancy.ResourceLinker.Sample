import { Routes } from "@angular/router";
import { IframeWrapperComponent } from "../shared/ui/iframe-wrapper/iframe-wrapper.component";

export const PASSENGER_ROUTES: Routes = [{
    path: '',
    pathMatch: 'full',
    redirectTo: 'search'
}, {
    path: "search",
    component: IframeWrapperComponent,
    data: {
        url: 'http://localhost:5100/LegacyWebFormsApp/pages/passengersearch'
    }
}];
