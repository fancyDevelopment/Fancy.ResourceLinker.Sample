import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { ConnectionStatus } from './socket-manager';

export class SignalRConnectionManager {

  public connectionStatus = new Subject<ConnectionStatus>();

  private _hubConnection: signalR.HubConnection;

  private _refCount = 0;

  constructor(private _url: string) {
    this._hubConnection = new signalR.HubConnectionBuilder().withUrl(_url).build();
    this._hubConnection.onclose(() => {
      this.connectionStatus.next(ConnectionStatus.Disconnected);
    });
    this._hubConnection.onreconnecting(() => {
      this.connectionStatus.next(ConnectionStatus.Reconnecting);
    });
  }

  public startConnection() {
    this._hubConnection.start()
    .then(() => {
      this.connectionStatus.next(ConnectionStatus.Connected);
    })
    .catch(() => {
      // Error connecting to socket
    });
  }

  public stopConnection() {
    console.log('Stopping connection of ' + this._url);
    this._hubConnection.stop().then(() => {
      this.connectionStatus.next(ConnectionStatus.Disconnected);
    });
  }

  public addRef() {
    this._refCount++
    if(this._refCount === 1) {
      this.startConnection();
    }
  }

  public removeRef() {
    if(this._refCount > 0) {
      this._refCount--;
      if(this._refCount === 0) {
        this.stopConnection();
      }
    }
  }

  public on(method: string, callback: (...args: any[]) => void) {
    this._hubConnection.on(method, callback);
  }

}
