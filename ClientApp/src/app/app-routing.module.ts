import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { EditPageComponent } from './edit-page/edit-page.component'
import { ProjectsPageComponent } from './projects-page/projects-page.component'

    /*RouterModule.forRoot([
      { path: '', redirectTo: 'edit', pathMatch: 'full' },
      { path: 'edit/:id', component: EditPageComponent },
      { path: 'edit', component: EditPageComponent },
      { path: 'projects', component: ProjectsPageComponent },

    ]),*/

export const routes: Routes = [
  { path: 'edit/:id', component: EditPageComponent },
  { path: 'edit', component: EditPageComponent },
  { path: 'projects', component: ProjectsPageComponent },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: 'login',
    component: LoginComponent,
    data: {
      title: 'Login Page'
    }  
  },
  {
    path: 'Dashboard',
    component: HomeComponent,
    data: {
      title: 'Dashboard Page'
    }
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
