import { Routes } from '@angular/router';

import { AnnouncementsComponent } from './announcements.component';
import { AnnouncementListComponent } from './announcement-list/announcement-list.component';
import { AnnouncementFormComponent } from './announcement-form/announcement-form.component';

export const ANNOUNCEMENT_ROUTES: Routes = [
  {
    path: '',
    component: AnnouncementsComponent,
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: AnnouncementListComponent },
      { path: 'add', component: AnnouncementFormComponent },
      { path: 'edit/:id', component: AnnouncementFormComponent }
    ]
  }
];

