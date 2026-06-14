// ─────────────────────────────────────────────
// app.routes.ts
// frontend/src/app/app.routes.ts
// ─────────────────────────────────────────────

import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';
import { dashboardGuard } from './core/guards/dashboard.guard';

export const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () =>
      import('./auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./layout/main-layout/main-layout.component')
        .then(m => m.MainLayoutComponent),
    children: [
      {
  path: 'dashboard',
  canActivate: [dashboardGuard],
  loadComponent: () =>
    import('./dashboard/dashboard.component')
      .then(m => m.DashboardComponent)
},
{
  path: 'employee-dashboard',
  loadComponent: () =>
    import('./dashboard/employee-dashboard/employee-dashboard.component')
      .then(m => m.EmployeeDashboardComponent)
},
      {
        path: 'employees',
        canActivate: [roleGuard],
        data: { roles: ['Admin', 'Manager'] },
        loadChildren: () =>
          import('./employees/employee.routes').then(m => m.EMPLOYEE_ROUTES)
      },
      {
        path: 'departments',
        canActivate: [roleGuard],
        data: { roles: ['Admin'] },
        loadChildren: () =>
          import('./departments/department.routes').then(m => m.DEPARTMENT_ROUTES)
      },
      {
        path: 'attendance',
        loadChildren: () =>
          import('./attendance/attendance.routes').then(m => m.ATTENDANCE_ROUTES)
      },
      {
        path: 'leaves',
        loadChildren: () =>
          import('./leaves/leave.routes').then(m => m.LEAVE_ROUTES)
      },
      {
        path: 'projects',
        loadChildren: () =>
          import('./projects/project.routes').then(m => m.PROJECT_ROUTES)
      },
      {
        path: 'clients',
        loadChildren: () =>
          import('./clients/client.routes').then(m => m.CLIENT_ROUTES)
      },
      {
        path: 'allocations',
        loadChildren: () =>
          import('./allocations/allocation.routes').then(m => m.ALLOCATION_ROUTES)
      },
      {
        path: 'announcements',
        loadChildren: () =>
          import('./announcements/announcement.routes').then(m => m.ANNOUNCEMENT_ROUTES)
      },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },
  { path: '**', redirectTo: 'auth/login' }
];

