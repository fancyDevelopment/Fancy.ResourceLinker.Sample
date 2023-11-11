import { Signal, WritableSignal, computed, untracked } from "@angular/core";
import { patchState } from "@ngrx/signals";
import { SignalStateMeta } from "@ngrx/signals/src/signal-state";

interface Patchable<T> {
  patch(state: Partial<T>): void;
}

function isRecord(value: unknown): value is Record<string, unknown> {
  return value?.constructor === Object;
}

// The deep patchable signal offers a patch method to update an existing slice of the state and is more tolerant to nullable values
export type DeepPatchableSignal<T> = Signal<T> & Patchable<T> &
  (T extends Record<string, unknown>
    ? Readonly<{ [K in keyof T]: DeepPatchableSignal<T[K]> }>
    : unknown);

export function toDeepPatchableSignal<State extends Record<string, unknown>, T>(state: SignalStateMeta<State>, patchFunc: (newVal: T) => Partial<State>, signal: Signal<T>): DeepPatchableSignal<T> {
  return new Proxy(signal, {
    get(target: any, prop) {

      if (prop === 'patch') {
        return (newVal: T) => {
          patchState(state, patchFunc(newVal))
        }
      }

      if (!target[prop]) {
        target[prop] = computed(() => target()[prop]);
      }

      return toDeepPatchableSignal(state, (newVal: T) => patchFunc({ ...target(), [prop]: isRecord(target[prop]()) ? { ...target[prop](), ...newVal } : newVal }), target[prop]);
    },
  });
}