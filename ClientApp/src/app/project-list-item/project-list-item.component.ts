import { Component, Input, EventEmitter, Output } from '@angular/core';
import { ProjectFile } from '../models/project-file.model';

@Component({
  selector: 'app-project-list-item',
  styleUrls: ['./project-list-item.component.css'],
  templateUrl: './project-list-item.component.html'
})
export class ProjectListItemComponent {
  @Input() projectFile: ProjectFile;
  @Output() removeEvent = new EventEmitter<string>();

  public onRemove(projectFileId: string) {
    this.removeEvent.emit(projectFileId);
  }
}
