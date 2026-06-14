import { Routes } from '@angular/router';

import { ClientsComponent } from './clients.component';
import { ClientListComponent } from './client-list/client-list.component';
import { ClientFormComponent } from './client-form/client-form.component';

export const CLIENT_ROUTES: Routes = [
  {
    path: '',
    component: ClientsComponent,
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: ClientListComponent },
      { path: 'add', component: ClientFormComponent },
      { path: 'edit/:id', component: ClientFormComponent }
    ]
  }
];

