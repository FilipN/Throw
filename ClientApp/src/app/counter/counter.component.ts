import { Component } from '@angular/core';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
})
export class CounterComponent {
  public currentCount = 0;
  public editorOptions = {theme: 'vs-dark', language: 'javascript'};
  public code = 'function x() {\nconsole.log("Hello world!");\n}';
  public incrementCounter() {
    this.currentCount++;
  }
}
