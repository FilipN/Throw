import { Component, Input, Inject,SimpleChanges } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { SignalrcoService } from '../services/signalrco.service';


@Component({
  selector: 'app-editor-component',
  templateUrl: './editor.component.html'
})
export class EditorComponent {
  httpClient;
  basePath;
  router;
  projectGuid = '';
  usrs = ["Filip", "Ana"];

  public content;
  signalserv;
  constructor(routerI: Router, http: HttpClient, @Inject('BASE_URL') baseUrl: string, private signalsrv: SignalrcoService) {
    this.httpClient = http;
    this.basePath = baseUrl;
    this.router = routerI;
    this.signalserv = signalsrv;
    signalsrv.getCodeChange((message) => {
      this.code = message["newCode"];
    });

    signalsrv.usersRefresh((message) => {
      this.code = message["newCode"];
    });

    signalsrv.outputChange((message) => {
      this.outputConsole = message["runResult"];
    });

      

    let pathParts = this.router.url.split("/");
    let guid = pathParts[pathParts.length - 1];
    signalsrv.setGuid(guid);
  }

  timerStarted = false;

  public onChange(e) {
    console.log(e);
    if (!this.timerStarted) {
      this.timerStarted = true;
      setTimeout(()=> {
        let un = this.getUserName();
        let message = { "code": this.code, 'guid': this.projectGuid, 'identity': un };

        this.httpClient.post(this.basePath + 'api/projects/change', message).subscribe(result => {

        }, error => console.log(error));
        this.timerStarted = false;
      }, 700);


    }

    //this.connection.invoke("CodeChange", e);

    
  }

  public getUserName() {
    let username = JSON.parse(localStorage.getItem('socialusers'))["email"];
    return username;
  }

  public onRunProgram() {
    let un = this.getUserName();
    let message = { "code": this.code, 'guid': this.projectGuid,'identity':un};

    this.httpClient.post(this.basePath + 'api/projects/run', message).subscribe(result => {
      this.outputConsole = result["runResult"]
    }, error => console.error(error));
  }

  public onClearConsole() {
    let un = this.getUserName();

    this.outputConsole = "";
  }



  public onLock() {
    let un = this.getUserName();
    let message = { "lock": this.lock, 'guid': 'sdfsdf234' };

    this.httpClient.post(this.basePath + 'api/projects/run', message).subscribe(result => {
      this.outputConsole = result["runResult"]
    }, error => console.error(error));

  }

  public editorOptions = {theme: 'vs-dark', language: 'python'};
  connection;
  ngOnInit() {

    let un = this.getUserName();

    let pathParts = this.router.url.split("/");
    let guid = pathParts[pathParts.length - 1];

    let message = { "identity": un, "guid": guid };
    this.httpClient.post(this.basePath + 'api/projects/open', message).subscribe(result => {

      this.code = result["code"]
    }, error => console.error(error));

    this.projectGuid = guid;

    /*this.connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl("https://localhost:44369/code")
      .build();

    this.connection.start().then(function () {
      console.log('Connected!');
      this.connection.invoke("JoinGroup", guid);
    }).catch(function (err) {
      return console.error(err.toString());
    });


    this.connection.on("codechange", (payload) => {
      //this.code = payload["newCode"];
    });

    this.connection.on("usersrefresh", (payload) => {
      alert(JSON.stringify(payload));
      this.usrs = payload["currentUsers"];
    });

    this.connection.on("outputchange", (payload) => {
      this.outputConsole = payload["runResult"];
    });*/

  }
  ngOnDestroy(): void {
    this.signalserv.disconnect();
  }

  public code = '';
  public lock = false;
  public outputConsole = '';
}
