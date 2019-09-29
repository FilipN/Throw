import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalrcoService {
  connection: signalR.HubConnection;

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:44369/code')
      .build();
    this.connect();
  }

  guid = '';

  public connect() {
    if (this.connection.state === signalR.HubConnectionState.Disconnected) {
      this.connection.start().then(()=> {
        console.log('Connected! ' +this.connection.state);
        this.connection.invoke("JoinGroup", this.guid);
      }).catch(function (err) {
        return console.error(err.toString());
      });
    }
  }

  public setGuid(guid) {
    this.guid = guid;
  }

  public getCodeChange(next) {
    this.connection.on('codechange', (message) => {
      next(message);
    });
  }

  public usersRefresh(next) {
    this.connection.on('usersrefresh', (message) => {
      next(message);
    });
  }

  public outputChange(next) {
    this.connection.on('outputchange', (message) => {
      next(message);
    });
  }

  public disconnect() {
    this.guid = '';
    this.connection.stop();
  }
}
