/** Contains information to a connected resource. */
export interface ResourceLink {
    /** The url to the connected resource. */
    href: string
}

/** Contains information to an action which can be executed on the resource. */
export interface ResourceAction {
    /** The HTTP verb to use for this action. */
    method: 'PUT' | 'POST' | 'DELETE'
    /** The url where to send the resources to. */
    href: string
}

/** Contains information to a related socket. */
export interface ResourceSocket {
    /** The name of the method the socket sends out. */
    method: string
    /** The url to the socket. */
    href: string
}

/** Defines the base layout of a resource requested by the hateoas client. */
export interface ResourceBase {
    /** Contains all links to connected resources. */
    _links?: { [key: string]: ResourceLink }
    /** Contains all actions which can be executed on this resource. */
    _actions?: { [key: string]: ResourceAction }
    /** Contains all sockets which are related to this resource. */
    _sockets?: { [key: string]: ResourceSocket }
    /** All other properties of the resource. */
    [key: string]: any
}