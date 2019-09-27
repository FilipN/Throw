import { Component, Input, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';


@Component({
  selector: 'app-editor-component',
  templateUrl: './editor.component.html'
})
export class EditorComponent {
  //public forecasts: WeatherForecast[];
  httpClient;
  basePath;
  router;

  constructor(routerI : Router,http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.httpClient = http;
    this.basePath = baseUrl;
    this.router = routerI;
    /*http.get<any>(baseUrl + 'api/projects/run').subscribe(result => {
      alert(result);
    }, error => console.error(error));*/
  }

  public onRunProgram() {
    let message = { "code": this.code, 'guid':'sdfsdf234'};

    this.httpClient.post(this.basePath + 'api/projects/run', message).subscribe(result => {
      this.outputConsole = result["runResult"]
    }, error => console.error(error));
    //this.store.dispatch(create({ name: `Project_${generatedId}`, id: generatedId }));
    //this.router.navigate(['/', 'edit', generatedId]);
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
    alert("loaded");
    this.httpClient.post(this.basePath + 'api/projects/open', message).subscribe(result => {
      this.code = result["code"]
    }, error => console.error(error));
  }

  @Input('code')
  public code = '';
  public lock = false;
  public outputConsole = '';
}
