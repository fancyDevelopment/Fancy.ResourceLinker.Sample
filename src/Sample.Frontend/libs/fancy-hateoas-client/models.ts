import { ResourceAction, ResourceLink, ResourceSocket } from "./resource";

export type Resource = {
    /** Contains all links to connected resources. */
    _links?: { [key: string]: ResourceLink }
    /** Contains all actions which can be executed on this resource. */
    _actions?: { [key: string]: ResourceAction }
    /** Contains all sockets which are related to this resource. */
    _sockets?: { [key: string]: ResourceSocket }
}

export type DynamicResourceValue = Resource | number | string | boolean | Resource[] | number[] | boolean[] | null;

export type DynamicResource = Resource & {
    [key: string]: DynamicResourceValue;
}

export type AllPropsOf<T> = {
    [K in keyof T] : T[K];
}

export type HypermediaFacade<TResource extends Resource> = AllPropsOf<TResource> & {
    fetchLink<TLinkedResource extends Resource>(rel: string): Promise<HypermediaFacade<TLinkedResource>>;
    executeAction(rel: string, alternateBody?: any): Promise<void>;
}