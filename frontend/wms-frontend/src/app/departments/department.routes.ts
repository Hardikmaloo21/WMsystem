import { Routes } from '@angular/router';

import { DepartmentsComponent } from './departments.component';
import { DepartmentListComponent } from './department-list/department-list.component';
import { DepartmentFormComponent } from './department-form/department-form.component';

export const DEPARTMENT_ROUTES: Routes = [
  {
    path: '',
    component: DepartmentsComponent,
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: DepartmentListComponent },
      { path: 'add', component: DepartmentFormComponent },
      { path: 'edit/:id', component: DepartmentFormComponent }
    ]
  }
];

