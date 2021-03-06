import { Component, Input, Inject, SimpleChanges, HostListener,HostBinding } from '@angular/core';
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
  usrs = [];

  public content;
  signalserv;
  owner = false;
  constructor(routerI: Router, http: HttpClient, @Inject('BASE_URL') baseUrl: string, private signalsrv: SignalrcoService) {
    this.httpClient = http;
    this.basePath = baseUrl;
    this.router = routerI;
    this.signalserv = signalsrv;
    signalsrv.getCodeChange((message) => {
      this.code = message["newCode"];
    });

    signalsrv.usersRefresh((message) => {
      this.usrs = message["users"];
    });

    signalsrv.outputChange((message) => {
      this.outputConsole = message["runResult"];
    });

      

    let pathParts = this.router.url.split("/");
    let guid = pathParts[pathParts.length - 1];
    signalsrv.setGuid(guid);
  }
  cssStringVar = 'btn-info';
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
    this.outputConsole = "";
  }

  public onSaveCode() {
    let un = this.getUserName();
    let message = { "code": this.code, 'guid': this.projectGuid, 'identity': un };

    this.httpClient.post(this.basePath + 'api/projects/save', message).subscribe(result => {
    }, error => console.error(error));
  }

  public onLock() {
    let un = this.getUserName();
    let lock = false;
    if (this.cssStringVar == 'btn-info') {
      lock = true;
      this.cssStringVar = 'btn-primary';
    }else {
      lock = false;
      this.cssStringVar = 'btn-info';
    }

    let message = { 'lock': lock, 'guid': this.projectGuid, 'identity': un };

    this.httpClient.post(this.basePath + 'api/projects/lock', message).subscribe(result => {

    }, error => console.log(error));
  }

  public editorOptions = {theme: 'vs-dark', language: 'python'};
  connection;
  ngOnInit() {

    let un = this.getUserName();

    let pathParts = this.router.url.split("/");
    let guid = pathParts[pathParts.length - 1];

    let message = { "identity": un, "guid": guid };
    this.httpClient.post(this.basePath + 'api/projects/open', message).subscribe(result => {
      this.usrs = result["users"];
      this.code = result["code"];
      this.owner = result["role"] == 'owner';
    }, error => console.error(error));

    this.projectGuid = guid;

  }

  @HostListener('window:unload', ['$event'])
  unloadHandler(event) {
    let un = this.getUserName();
    let message = { 'guid': this.projectGuid, 'identity': un };
    this.httpClient.post(this.basePath + 'api/projects/leave', message).subscribe(result => {

    }, error => console.log(error));
  }

  ngOnDestroy(): void {
    let un = this.getUserName();
    let message = { 'guid': this.projectGuid, 'identity': un };
    this.httpClient.post(this.basePath + 'api/projects/leave', message).subscribe(result => {

    }, error => console.log(error));

    this.signalserv.disconnect();
  }

  public code = '';
  public lock = false;
  public outputConsole = '';
}
