import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-editor-component',
  templateUrl: './editor.component.html'
})
export class EditorComponent {
  public editorOptions = {theme: 'vs-dark', language: 'javascript'};

  @Input('code')
  public code = 'function x() {\nconsole.log("Hello world!");\n}';
}
