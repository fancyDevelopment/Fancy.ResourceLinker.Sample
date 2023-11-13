import { HttpClient } from "@angular/common/http";
import { Signal, WritableSignal, computed, effect, inject, signal } from "@angular/core";
import { patchState, signalStoreFeature, withHooks, withMethods, withState } from "@ngrx/signals";
import { SignalStateMeta } from "@ngrx/signals/src/signal-state";
import { ResourceBase } from "fancy-hateoas-client";
import { DeepPatchableSignal, isRecord, toDeepPatchableSignal } from "fancy-ngrx-deep-patchable-signal";
import { delay, firstValueFrom } from "rxjs";


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

  let links: Record<string, WritableSignal<any>> = {}

  return new Proxy(modelSignal, {
      get(target: any, prop) {

        if(prop === 'fetchLink') {

          return function(rel: string) {

            if(!links[rel]) {
              // Create a new signal
              links[rel] = signal<any>(null);

              const linkHrefSignal = (modelSignal as any)._links[rel].href;

              effect(() => {
                const linkHref: string = linkHrefSignal();
                if(linkHref) {
                  httpClient.get(linkHref).subscribe((model: any) => {
                    links[rel].set(model);
                  });
                }
              });
            }

            return toHypermediaSignal(links[rel], httpClient);
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

            const modelsPatchable = toDeepPatchableSignal(state, newVal => ({ models: { ...state.models(), ...newVal } }), state.models);

            return {
                async loadModel(url: string) {
                  modelsPatchable.patch({[url]: null});
                  const model = await firstValueFrom(httpClient.get(url).pipe());
                  modelsPatchable.patch({[url]: model});
                },
                getOrLoadModel(url: string): HypermediaSignal<any> {
                  const modelSignal = modelsPatchable[url];
                    if(modelSignal() === undefined) {
                        this.loadModel(url);
                    }
                    return toHypermediaSignal(modelSignal, httpClient);
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