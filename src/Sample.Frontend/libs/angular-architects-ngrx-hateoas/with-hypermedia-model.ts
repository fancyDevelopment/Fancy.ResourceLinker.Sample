import { Signal, effect, inject, signal } from "@angular/core";
import { HateoasClient, HypermediaFacade, Resource } from "fancy-hateoas-client";
import { DeepPatchableSignal, Patchable, toDeepPatchableSignal } from "./deep-patchable-signal";
import { SignalStoreFeature, patchState, signalStoreFeature, withMethods, withState } from "@ngrx/signals";

interface HypermediaSignalFacade<TResource> {
    fetchLink<TLinkedResource extends Resource>(rel: string): HypermediaSignalFacade<TLinkedResource>;
}

export type HypermediaSignal<T> = Signal<T> & Patchable<T> & HypermediaSignalFacade<T> &
    (T extends Record<string, unknown>
        ? Readonly<{ [K in keyof T]: HypermediaSignal<T[K]> }>
        : unknown);

export function toHypermediaSignal<T extends HypermediaFacade<Resource>>(modelSignal: DeepPatchableSignal<T>): HypermediaSignal<T> {

    let links: Record<string, HypermediaSignal<any>> = {}

    return new Proxy(modelSignal, {
        get(target: any, prop) {

            if (prop === 'fetchLink') {

                return function<T>(rel: string) {

                    if (!links[rel]) {
                        // Create a new signal
                        const linkedResourceSignal = signal<any>(null);

                        const linkHrefSignal: Signal<string> = (modelSignal as any)._links[rel].href;

                        effect(() => {
                            const model = modelSignal();
                            
                            if (model?._links?.[rel]) {
                                model.fetchLink(rel).then(linkedResource => linkedResourceSignal.set(linkedResource));
                            } else {
                                linkedResourceSignal.set(undefined);
                            }
                        }, { allowSignalWrites: true });

                        const patchablelinkedResourceSignal = toDeepPatchableSignal(newVal => linkedResourceSignal.set(newVal), linkedResourceSignal);
                        const hypermediaSignal = toHypermediaSignal(patchablelinkedResourceSignal);

                        links[rel] = hypermediaSignal;
                    }

                    return links[rel];
                };
            }

            return target[prop];
        },
    });
}

//type VModelState<Resource, PropName extends string> = { [K in PropName]: { [key: string]: Resource } };
type VModelState<TResource, PropName extends string> = { [K in PropName]: TResource };

type ResourceAccessMethod<TResource, MethodName extends string> = { [K in MethodName as `Load${MethodName}`]: (url: string, fromCache?: boolean) => HypermediaSignal<TResource> };

export function withHypermediaModel<TResource extends Resource, PropName extends string>(
    collectionName?: PropName): SignalStoreFeature<
        { state: {}; signals: {}; methods: {} },
        {
            state: VModelState<TResource, PropName>;
            signals: {},
            methods: ResourceAccessMethod<TResource, PropName>;
        }
    >;
export function withHypermediaModel<TResource extends Resource>(collectionName?: string) {

    const resourcesKey = collectionName === undefined ? 'resources' : `${collectionName}Resources`;
    const methodName = collectionName === undefined ? 'resources' : `Load${collectionName}`;

    return signalStoreFeature(
        withState({
            [resourcesKey]: undefined as any as HypermediaFacade<TResource>
        }),
        withMethods(state => {

            const hateoasClient = inject(HateoasClient);

            const modelSignal = toDeepPatchableSignal<HypermediaFacade<TResource>>(newVal => patchState(state, { [resourcesKey]: { ...state[resourcesKey](), ...newVal } }), state[resourcesKey]);
            const hypermediaSignal = toHypermediaSignal(modelSignal);

            return {
                [methodName]: (url: string, fromCache: boolean = false): HypermediaSignal<TResource> => {
                    if(!fromCache || modelSignal()?._links?.self.href !== url) {
                        hateoasClient.fetchHypermedia<TResource>(url).then(resource => modelSignal.patch(resource));
                    }
                    
                    return hypermediaSignal;
                }
            };
        })
    );
}