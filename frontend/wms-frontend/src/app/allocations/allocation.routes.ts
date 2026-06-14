import { Routes } from '@angular/router';

import { AllocationsComponent } from './allocations.component';
import { AllocationListComponent } from './allocation-list/allocation-list.component';
import { AllocationFormComponent } from './allocation-form/allocation-form.component';

export const ALLOCATION_ROUTES: Routes = [
  {
    path: '',
    component: AllocationsComponent,
    children: [
      {
        path: '',
        redirectTo: 'list',
        pathMatch: 'full'
      },
      {
        path: 'list',
        component: AllocationListComponent
      },
      {
        path: 'add',
        component: AllocationFormComponent
      },
      {
  path: 'edit/:id',
  component: AllocationFormComponent
}
    ]
  }
];