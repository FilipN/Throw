import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-editor-component',
  templateUrl: './editor.component.html'
})
export class EditorComponent {
  public editorOptions = {theme: 'vs-dark', language: 'python'};

  @Input('code')
  public code = '';
  public outputConsole ='5'
}
