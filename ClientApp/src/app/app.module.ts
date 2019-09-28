import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { GoogleLoginProvider, FacebookLoginProvider, AuthService } from 'angular-6-social-login';
import { SocialLoginModule, AuthServiceConfig } from 'angular-6-social-login';
import { LoginComponent } from './login/login.component';  
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
import { AppRoutingModule } from './app-routing.module';

export function socialConfigs() {

  const config = new AuthServiceConfig([{
    id: FacebookLoginProvider.PROVIDER_ID,
    provider: new FacebookLoginProvider('app -id')
  },
  {
    id: GoogleLoginProvider.PROVIDER_ID,
    provider: new GoogleLoginProvider('253183284360-cadp9571em2edkb3evmgfu8lae56h5v2.apps.googleusercontent.com')
  }]);
  return config;
} 

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
    LoginComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    StoreModule.forRoot({ projectFiles: projectFilesReducer }),
    HttpClientModule,
    FormsModule,
    /*RouterModule.forRoot([
      { path: '', redirectTo: 'edit', pathMatch: 'full' },
      { path: 'edit/:id', component: EditPageComponent },
      { path: 'edit', component: EditPageComponent },
      { path: 'projects', component: ProjectsPageComponent },

    ]),*/
    MonacoEditorModule.forRoot(),
    StoreDevtoolsModule.instrument({
      maxAge: 25,
      logOnly: false,
      features: {
        pause: false,
        lock: true,
        persist: true
      }
    }),
    AppRoutingModule
  ],
  providers: [AuthService,
    {
      provide: AuthServiceConfig,
      useFactory: socialConfigs
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
