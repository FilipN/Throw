import { Component, Input } from '@angular/core';
import { ProjectFile } from '../models/file.model';

@Component({
  selector: 'app-project-list-item',
  styleUrls: ['./project-list-item.component.css'],
  templateUrl: './project-list-item.component.html'
})
export class ProjectListItemComponent {
  @Input() projectFile: ProjectFile;
}
