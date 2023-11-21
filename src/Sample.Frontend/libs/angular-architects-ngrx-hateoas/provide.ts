import { EnvironmentProviders, Provider, inject, makeEnvironmentProviders } from "@angular/core";
import { AxiosRequestManager, HateoasClient, RequestManager, SignalRSocketManager } from "fancy-hateoas-client";
import { AngularRequestManager } from "./angular-request-manager";

export enum HateoasFeatureKind {
    AxiosRequestManager,
    AngularRequestManager
}

export interface HateoasFeature {
    kind: HateoasFeatureKind;
    providers: Provider[];
}

export function withAxiosRequestManager(): HateoasFeature {
    return {
        kind: HateoasFeatureKind.AxiosRequestManager,
        providers: [
            {
                provide: RequestManager,
                useFactory: () => new AxiosRequestManager()
            }
        ]
    }
}

export function withAngularRequestManager(): HateoasFeature {
    return {
        kind: HateoasFeatureKind.AngularRequestManager,
        providers: [
            {
                provide: RequestManager,
                useFactory: () => new AngularRequestManager()
            }
        ]
    }
}

export function provideHateoas(...features: HateoasFeature[]): EnvironmentProviders {

    // Validate features
    if(features?.length != 1) {
        throw new Error("You need to specifiy either the 'AxiosRequestManager' or the 'AngularRequestManager'");
    }

    return makeEnvironmentProviders([
        {
            provide: HateoasClient,
            useFactory: () => new HateoasClient(inject(RequestManager), new SignalRSocketManager())
        },
        // Add providers from services
        features?.map(f => f.providers)
    ]);
}