import { Component } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-edit-page',
  styleUrls: ['./edit-page.component.css'],
  templateUrl: './edit-page.component.html'
})
export class EditPageComponent {

  public readonly textId$: Observable<string>;
  code: string;

  constructor(
    private route: ActivatedRoute,
   ) {
    this.textId$ = this.route.paramMap.pipe(
      switchMap((params: ParamMap) => {
        return params.get('id');
      })
    );

    this.textId$.subscribe((textId: string) => {
      // TODO: request actual code from the server here using switchMap
      this.code = `
function x() {\nconsole.log("Hello world!");\n}
\n\n
//btw this is code with id: ${textId};
`;
    });
  }
}
