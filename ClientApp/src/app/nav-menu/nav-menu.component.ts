import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Store } from '@ngrx/store';
import { Router } from '@angular/router';
import { create } from '../actions/project-files.actions';


@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  httpClient;
  basePath;
  store;
  router;

  constructor(routerI:Router, http: HttpClient, @Inject('BASE_URL') baseUrl: string, storeI: Store<any>, router:Router) {
    this.httpClient = http;
    this.basePath = baseUrl;
    this.router = routerI;
    this.store = storeI;
  }

  public onNewProject() {
    let message = { "username": "filip"};

    this.httpClient.post(this.basePath + 'api/projects/new', message).subscribe(result => {
      if (result != undefined && result["guid"] != undefined) {
        let guid = result["guid"];
        this.store.dispatch(create({ name: `Project_${guid}`, id: guid }));

        this.router.navigate(['/', 'edit', guid]);
      }
    }, error => console.error(error));

  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
