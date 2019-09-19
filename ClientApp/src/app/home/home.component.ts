import { Component } from '@angular/core';
import { ProjectFile } from '../models/project-file.model';
import { Store } from '@ngrx/store';
import { create } from '../actions/project-files.actions';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(
    private store: Store<ProjectFile[]>,
    private router: Router,
    ) {}

  public onCreateNew() {
    // FIXME: generate id on the server side with uuid or something else
    const generatedId = `${(new Date()).getTime()}`;
    this.store.dispatch(create({name: `Project_${generatedId}`, id: generatedId }));
    this.router.navigate(['/', 'edit', generatedId]);
  }
}
