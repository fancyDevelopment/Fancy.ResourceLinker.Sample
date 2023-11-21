import { Observable } from 'rxjs';
import { ResourceBase, ResourceSocket } from './resource';

export enum ConnectionStatus {
    Connected,
    Reconnecting,
    Disconnected
}

export abstract class SocketManager {
    abstract connectionStatus: Observable<ConnectionStatus>;
    abstract createSocketObserver(resource: ResourceBase, socket: ResourceSocket): Observable<any>;
}