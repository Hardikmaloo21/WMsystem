import { Routes } from '@angular/router';

import { EmployeesComponent } from './employees.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { EmployeeFormComponent } from './employee-form/employee-form.component';
import { EmployeeProfileComponent } from './employee-profile/employee-profile.component';

export const EMPLOYEE_ROUTES: Routes = [
  {
    path: '',
    component: EmployeesComponent,
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: EmployeeListComponent },
      { path: 'add', component: EmployeeFormComponent },
      { path: 'edit/:id', component: EmployeeFormComponent },
      { path: 'profile/:id', component: EmployeeProfileComponent }
    ]
  }
];