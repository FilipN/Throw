import { Component } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ProjectFile } from '../models/project-file.model';
import { Store } from '@ngrx/store';
import { remove } from '../actions/project-files.actions';

@Component({
  selector: 'app-projects-page',
  styleUrls: ['./projects-page.component.css'],
  templateUrl: './projects-page.component.html'
})
export class ProjectsPageComponent {
  public readonly files$: Observable<ProjectFile[]>;
  constructor(private store: Store<ProjectFile[]>) {
    this.files$ = this.store.select('projectFiles');
  }

  public onRemove(id: string) {
    this.store.dispatch(remove({id}));
  }
}
