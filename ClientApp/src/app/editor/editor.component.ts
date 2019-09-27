import { Component, Input, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-editor-component',
  templateUrl: './editor.component.html'
})
export class EditorComponent {
  //public forecasts: WeatherForecast[];
  httpClient;
  basePath;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.httpClient = http;
    this.basePath = baseUrl;
    /*http.get<any>(baseUrl + 'api/projects/run').subscribe(result => {
      alert(result);
    }, error => console.error(error));*/
  }

  public onCreateNew() {
    // FIXME: generate id on the server side with uuid or something else
    //const generatedId = `${(new Date()).getTime()}`;
    this.httpClient.post(this.basePath + 'api/projects/run', {}).subscribe(result => {
      alert(result);
    }, error => console.error(error));
    //this.store.dispatch(create({ name: `Project_${generatedId}`, id: generatedId }));
    //this.router.navigate(['/', 'edit', generatedId]);
  }

  public editorOptions = {theme: 'vs-dark', language: 'python'};

  @Input('code')
  public code = '';
  public outputConsole ='5'
}
