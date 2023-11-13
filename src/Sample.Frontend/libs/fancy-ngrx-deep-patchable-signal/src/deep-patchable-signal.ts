import { Signal, computed } from "@angular/core";

interface Patchable<T> {
  patch(state: Partial<T>): void;
}

export function isRecord(value: unknown): value is Record<string, unknown> {
  return value?.constructor === Object;
}

// The deep patchable signal offers a patch method to update an existing slice of the state and is more tolerant to nullable values
export type DeepPatchableSignal<T> = Signal<T> & Patchable<T> &
  (T extends Record<string, unknown>
    ? Readonly<{ [K in keyof T]: DeepPatchableSignal<T[K]> }>
    : unknown);

export function toDeepPatchableSignal<T>(patchFunc: (newVal: T) => void, signal: Signal<T>): DeepPatchableSignal<T> {
  return new Proxy(signal, {
    get(target: any, prop) {

      if (prop === 'patch') {
        return (newVal: T) => {
          patchFunc(newVal)
        }
      }

      if (!target[prop]) {
        target[prop] = computed(() => target() ? target()[prop] : undefined);
      }

      return toDeepPatchableSignal((newVal: T) => patchFunc({ ...target(), [prop]: isRecord(target[prop]()) ? { ...target[prop](), ...newVal } : newVal }), target[prop]);
    },
  });
}