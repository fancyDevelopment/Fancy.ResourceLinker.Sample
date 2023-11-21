import { HateoasClient } from "./hateoas-client";
import { ResourceBase, ResourceLink } from "./resource";

/**
 * Represents an endpoint of a HATEOAS resouce with the functionality to fetch it.
 */
export class ResourceEndpoint implements ResourceLink {

    /**
     * Creates a new instance of a ResourceEndpoint.
     * @param href The base url to read the resource from.
     * @param hateoasClient The hateoas client to use to fetch the resource.
     */
    constructor(public href: string, private hateoasClient: HateoasClient) {}

    /**
     * Fetches the resource from the server.
     * @param queryParams An optional key/value list of additional query params
     * @returns A resource.
     */
    fetch?(queryParams?: {[key: string]: string | number }): Promise<ResourceBase | ResourceBase[] | null> {
        let url = this.href;
        if(queryParams) {
            // Add additional query params
            if(url.indexOf('?') > 0) { url = url + '&'; }
            else { url = url + '?'; }
            for(var key in queryParams) {
                url = url + key + '=' + queryParams[key] + '&';
            }
            // Remove the last not needed concatenation character
            url = url.substr(0, url.length - 1);
        }
        return this.hateoasClient.fetch(url);
    }
}
