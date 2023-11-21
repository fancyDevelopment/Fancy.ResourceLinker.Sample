import { ResourceAction, ResourceBase } from "./resource";

export type ActionFunc = (body?: any) => Promise<any>;

export abstract class RequestManager {
  fetch(url: string): Promise<ResourceBase | ResourceBase[]> {
    return this.request('GET', url);
  }

  createActionFunc(resource: ResourceBase, action: ResourceAction): ActionFunc {
    if (action.method === 'PUT' || action.method === 'POST') {
      return (bodyToSend: any) => {
        // If the caller has provided a body to send use it ...
        let bodyContent = bodyToSend;
        // ... if not send the own object back to the server.
        if (!bodyContent) { bodyContent = this.cloneResourceWithoutMetadata(resource); }
        // Execute the request
        return this.request(action.method, action.href, bodyContent);
      };
    } else {
      return () => {
        return this.request(action.method, action.href);
      };
    }
  }

  cloneResourceWithoutMetadata(resource: ResourceBase) {
    let copy: any = null;

    // Handle the 3 simple types, and null or undefined
    if (null == resource || "object" != typeof resource) return resource;

    // Handle Array
    if (resource instanceof Array) {
      copy = [];
      for (let i = 0, len = resource.length; i < len; i++) {
        copy[i] = this.cloneResourceWithoutMetadata(resource[i]);
      }
      return copy;
    }

    // Handle Object
    if (resource instanceof Object) {
      copy = {};
      for (const attr in resource) {
        if (resource.hasOwnProperty(attr) && !attr.startsWith('_') && !attr.endsWith('$') && typeof resource[attr] !== 'function') {
          copy[attr] = this.cloneResourceWithoutMetadata(resource[attr]);
        }
      }
      return copy;
    }

    throw new Error("Unable to copy resource! Its type isn't supported.");
  }

  protected abstract request(method: 'GET' | 'PUT' | 'POST' | 'DELETE', url: string, body?: any): Promise<any>;
}