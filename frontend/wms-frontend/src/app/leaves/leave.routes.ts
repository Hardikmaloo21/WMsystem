import { Routes } from '@angular/router';

import { LeavesComponent } from './leaves.component';
import { LeaveListComponent } from './leave-list/leave-list.component';
import { LeaveFormComponent } from './leave-form/leave-form.component';


export const LEAVE_ROUTES: Routes = [
  {
    path: '',
    component: LeavesComponent,
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: LeaveListComponent },
      { path: 'apply', component: LeaveFormComponent },
    ]
  }
];

