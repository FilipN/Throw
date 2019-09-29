import { Component, Inject } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ProjectFile } from '../models/project-file.model';
import { Store } from '@ngrx/store';
import { remove } from '../actions/project-files.actions';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-projects-page',
  styleUrls: ['./projects-page.component.css'],
  templateUrl: './projects-page.component.html'
})
export class ProjectsPageComponent {
  public readonly files$: Observable<ProjectFile[]>;
  constructor(routerI: Router, http: HttpClient, @Inject('BASE_URL') baseUrl: string,private store: Store<ProjectFile[]>) {
    //this.files$ = this.store.select('projectFiles');
    this.httpClient = http;
    this.basePath = baseUrl;
    this.router = routerI;
  }


  projects = [{ "url": "asdf" }];
  httpClient;
  basePath;
  router;
  ngOnInit() {
    let un = this.getUserName();

    let pathParts = this.router.url.split("/");
    let guid = pathParts[pathParts.length - 1];

    let message = { "identity": un};
    this.httpClient.post(this.basePath + 'api/projects/projectsforuser', message).subscribe(result => {
      alert('tu smo');

    });
  }
      

  public getUserName() {
    let username = JSON.parse(localStorage.getItem('socialusers'))["email"];
    return username;
  }

  public onRemove(id: string) {
    this.store.dispatch(remove({id}));
  }
}
