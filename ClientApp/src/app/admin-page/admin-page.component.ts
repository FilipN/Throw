import { Component } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ProjectFile } from '../models/file.model';

@Component({
  selector: 'app-admin-page',
  styleUrls: ['./admin-page.component.css'],
  templateUrl: './admin-page.component.html'
})
export class AdminPageComponent {
    public readonly files$: Observable<ProjectFile[]> = of([
      {
        id: 'uuid-neki-dugacki-jedinstveni',
        name: 'Prvi projekat',
        author: 'id_autora_neki_1',
        created: new Date(),
        lastUpdated: new Date(),
      },
      {
        id: 'isto-uuid-neki-dugacki-jedinstveniji-od-prvog',
        name: 'Nije prvi projekat',
        author: 'id_autora_neki_1',
        created: new Date(),
        lastUpdated: new Date(),
      }
    ]);
}
