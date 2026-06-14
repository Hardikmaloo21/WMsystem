import { Routes } from '@angular/router';

import { ProjectsComponent } from './projects.component';
import { ProjectListComponent } from './project-list/project-list.component';
import { ProjectFormComponent } from './project-form/project-form.component';

export const PROJECT_ROUTES: Routes = [
  {
    path: '',
    component: ProjectsComponent,
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: ProjectListComponent },
      { path: 'add', component: ProjectFormComponent },
      { path: 'edit/:id', component: ProjectFormComponent }
    ]
  }
];

