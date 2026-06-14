import { Routes } from '@angular/router';
import { AttendanceComponent } from './attendance.component';
import { AttendanceListComponent } from './attendance-list/attendance-list.component';
import { AttendanceReportComponent } from './attendance-report/attendance-report.component';
import { MyAttendanceComponent }
from './my-attendance/my-attendance.component';
export const ATTENDANCE_ROUTES: Routes = [
  {
    path: '',
    component: AttendanceComponent,
    children: [
      {
  path: '',
  redirectTo: 'my',
  pathMatch: 'full'
},
      { path: 'list', component: AttendanceListComponent },
      { path: 'report', component: AttendanceReportComponent },
      {
  path: 'my',
  component: MyAttendanceComponent
}
    ]
  }
];

