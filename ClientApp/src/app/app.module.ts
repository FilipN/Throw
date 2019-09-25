import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { EditorComponent } from './editor/editor.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { EditPageComponent } from './edit-page/edit-page.component';
import { ProjectsPageComponent } from './projects-page/projects-page.component';
import { ProjectListItemComponent } from './project-list-item/project-list-item.component';
import { StoreModule } from '@ngrx/store';
import { projectFilesReducer } from './reducers/project-files.reducer';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    EditorComponent,
    FetchDataComponent,
    EditPageComponent,
    ProjectsPageComponent,
    ProjectListItemComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    StoreModule.forRoot({ projectFiles: projectFilesReducer }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', redirectTo: 'edit', pathMatch: 'full' },
      { path: 'edit/:id', component: EditPageComponent },
      { path: 'edit', component: EditPageComponent },
      { path: 'projects', component: ProjectsPageComponent },

    ]),
    MonacoEditorModule.forRoot(),
    StoreDevtoolsModule.instrument({
      maxAge: 25,
      logOnly: false,
      features: {
        pause: false,
        lock: true,
        persist: true
      }
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
