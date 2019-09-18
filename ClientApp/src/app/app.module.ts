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
import { AdminPageComponent } from './admin-page/admin-page.component';
import { ProjectListItemComponent } from './project-list-item/project-list-item.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    EditorComponent,
    FetchDataComponent,
    EditPageComponent,
    AdminPageComponent,
    AdminPageComponent,
    ProjectListItemComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'edit/:id', component: EditPageComponent },
      { path: 'admin', component: AdminPageComponent },
    ]),
    MonacoEditorModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
