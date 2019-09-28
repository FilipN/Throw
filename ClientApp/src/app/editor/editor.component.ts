import { Component, Input, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import * as signalR from '@aspnet/signalr';

@Component({
  selector: 'app-editor-component',
  templateUrl: './editor.component.html'
})
export class EditorComponent {
  httpClient;
  basePath;
  router;
  projectGuid = '';
  usrs = ["Filip","Ana"];

  constructor(routerI : Router,http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.httpClient = http;
    this.basePath = baseUrl;
    this.router = routerI;
  }

  public onRunProgram() {
    let message = { "code": this.code, 'guid': this.projectGuid};

    this.httpClient.post(this.basePath + 'api/projects/run', message).subscribe(result => {
      this.outputConsole = result["runResult"]
    }, error => console.error(error));
  }

  public onClearConsole() {
    this.outputConsole = "";
  }

  public onLock() {
    let message = { "lock": this.lock, 'guid': 'sdfsdf234' };

    this.httpClient.post(this.basePath + 'api/projects/run', message).subscribe(result => {
      this.outputConsole = result["runResult"]
    }, error => console.error(error));

  }

  public editorOptions = {theme: 'vs-dark', language: 'python'};

  ngOnInit() {
    let pathParts = this.router.url.split("/");
    let guid = pathParts[pathParts.length - 1];
    let message = { "username": "filip", "guid": guid };
    this.httpClient.post(this.basePath + 'api/projects/open', message).subscribe(result => {

      this.code = result["code"]
    }, error => console.error(error));

    this.projectGuid = guid;

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl("https://localhost:44369/code")
      .build();

    connection.start().then(function () {
      console.log('Connected!');
      connection.invoke("JoinGroup", guid);
    }).catch(function (err) {
      return console.error(err.toString());
    });


    connection.on("codechange", (payload) => {
      alert(payload);
    });

    connection.on("usersrefresh", (payload) => {
      alert(JSON.stringify(payload));
      this.usrs = payload["currentUsers"];
    });

    connection.on("outputchange", (payload) => {
      this.outputConsole = payload["runResult"];
    });

  }

  @Input('code')

  public code = '';
  public lock = false;
  public outputConsole = '';
}
