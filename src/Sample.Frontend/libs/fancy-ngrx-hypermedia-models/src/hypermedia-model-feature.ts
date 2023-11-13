import { HttpClient } from "@angular/common/http";
import { Signal, effect, inject, signal } from "@angular/core";
import { patchState, signalStoreFeature, withHooks, withMethods, withState } from "@ngrx/signals";
import { toDeepPatchableSignal } from "fancy-ngrx-deep-patchable-signal";
import { firstValueFrom } from "rxjs";


interface Hypermedia<T> {
    patch(state: Partial<T>): void;
    actions: Record<string, () => void>;
    fetchLink: (rel: string) => HypermediaSignal<any>;
}

export type HypermediaSignal<T> = Signal<T> & Hypermedia<T> &
(T extends Record<string, unknown>
  ? Readonly<{ [K in keyof T]: HypermediaSignal<T[K]> }>
  : unknown);

export function toHypermediaSignal<T>(modelSignal: Signal<T>, httpClient: HttpClient): HypermediaSignal<T> {

  let links: Record<string, HypermediaSignal<any>> = {}

  return new Proxy(modelSignal, {
      get(target: any, prop) {

        if(prop === 'fetchLink') {

          return function(rel: string) {

            if(!links[rel]) {
              // Create a new signal
              const linkedModelSignal = signal<any>(null);

              const linkHrefSignal = (modelSignal as any)._links[rel].href;

              effect(() => {
                const linkHref: string = linkHrefSignal();
                if(linkHref) {
                  httpClient.get(linkHref).subscribe((model: any) => {
                    linkedModelSignal.set(model);
                  });
                } else {
                  linkedModelSignal.set(undefined);
                }
              }, { allowSignalWrites: true });

              const patchableModelSignal = toDeepPatchableSignal(newVal => linkedModelSignal.set(newVal), linkedModelSignal);
              const hypermediaSignal = toHypermediaSignal(patchableModelSignal, httpClient);

              links[rel] = hypermediaSignal;
            }

            return links[rel];
          };
        }

        return target[prop];
      },
    });
  }

export const ROOT_MODEL = Symbol('ROOT_MODEL');

export function withHypermediaModels(apiRootUrl?: string) {
    return signalStoreFeature(
        withState({ models: {} as Record<string, unknown> }),
        withMethods(state => {

            const httpClient = inject(HttpClient);

            const modelsPatchable = toDeepPatchableSignal(newVal => patchState(state, { models: { ...state.models(), ...newVal } }), state.models);

            const hypermediaSignals: Record<string, HypermediaSignal<any>> = {};

            return {
                async loadModel(url: string) {
                  modelsPatchable.patch({[url]: null});
                  const model = await firstValueFrom(httpClient.get(url).pipe());
                  modelsPatchable.patch({[url]: model});
                },
                getOrLoadModel<TM>(url: string): HypermediaSignal<TM> {

                  if(!hypermediaSignals[url]) {

                    console.log('Loading model from ', url);

                    const modelSignal = modelsPatchable[url] as Signal<TM>;
                    if(modelSignal() === undefined) {
                      this.loadModel(url);
                    }
                  
                    hypermediaSignals[url] = toHypermediaSignal<TM>(modelSignal, httpClient);
                  }

                  return hypermediaSignals[url] as HypermediaSignal<TM>;
                }
            }
        }),
        withHooks({
            onInit({ loadModel }) {
                if(apiRootUrl) {
                    loadModel(apiRootUrl);
                }
            }
        })
    );
}