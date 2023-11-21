import { Observable, Subject } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

import { ResourceBase, ResourceSocket } from "./resource";
import { SignalRConnectionManager } from './signalr-connection-manager';
import { ConnectionStatus, SocketManager } from './socket-manager';

export class SignalRSocketManager extends SocketManager {

    public connectionStatus: Observable<ConnectionStatus>;

    public connectionStatusSubject: Subject<ConnectionStatus>;
    private signalRConnections: { [key: string]: SignalRConnectionManager} = { };

    constructor() {
        super();
        this.connectionStatusSubject = new Subject<ConnectionStatus>();
        this.connectionStatus = this.connectionStatusSubject.asObservable();
    }

    public createSocketObserver(resource: ResourceBase, socket: ResourceSocket) {
        const observable = new Observable(subscriber => {
            console.log("Starting observable for " + socket.method);
            // Check if there is already a connection for this url
            if(!this.signalRConnections[socket.href]) {
                // No connection manager is availalbe, create a new one
                this.signalRConnections[socket.href] = new SignalRConnectionManager(socket.href);
            }

            this.signalRConnections[socket.href].addRef();

            this.signalRConnections[socket.href].on(socket.method, (newData: any) => {
              console.log('Received socket event: ' + socket.method);
                subscriber.next(newData);
            });

            // Return the unsubscribe function
            return () => {
              this.signalRConnections[socket.href].removeRef();
            };
        });

        return observable.pipe(startWith({...resource}),
                               map((data: any) => {
                                 // Create a new object
                                var newObj = {...data};
                                this.update(newObj, data);
                                return newObj;
                                }));
    }

    private update (targetObject: any, obj: any) {
      Object.keys(obj).forEach((key) => {

        // delete property if set to undefined or null
        if ( undefined === obj[key] || null === obj[key] ) {
          delete targetObject[key]
        }

        // property value is object, so recurse
        else if (
            'object' === typeof obj[key]
            && !Array.isArray(obj[key])
        ) {

          // target property not object, overwrite with empty object
          if (
            !('object' === typeof targetObject[key]
            && !Array.isArray(targetObject[key]))
          ) {
            targetObject[key] = {}
          }

          // recurse
          this.update(targetObject[key], obj[key])
        }

        // set target property to update property
        else {
          targetObject[key] = obj[key]
        }
      })
    }
}
