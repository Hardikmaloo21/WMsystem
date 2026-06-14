
// ─────────────────────────────────────────────
// main-layout.component.ts
// frontend/src/app/layout/main-layout/main-layout.component.ts
// ─────────────────────────────────────────────

import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatBadgeModule } from '@angular/material/badge';
import { AuthService } from '../../core/services/auth.service';
import { LoadingService } from '../../core/services/loading.services';

interface NavItem {
  label: string;
  icon: string;
  route: string;
  roles?: string[];
}

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    CommonModule, RouterOutlet, RouterLink, RouterLinkActive,
    MatSidenavModule, MatToolbarModule, MatListModule,
    MatIconModule, MatButtonModule, MatMenuModule,
    MatProgressBarModule, MatBadgeModule
  ],
  template: `
    <mat-sidenav-container class="sidenav-container">

      <!-- Sidebar -->
      <mat-sidenav #drawer mode="side" opened class="sidenav">
        <div class="sidenav-header">
          <mat-icon class="logo-icon">business_center</mat-icon>
          <span class="logo-text">WMS</span>
        </div>

        <mat-nav-list>
          @for (item of visibleNavItems(); track item.route) {
            <a mat-list-item
               [routerLink]="item.route"
               routerLinkActive="active-link"
               class="nav-item">
              <mat-icon matListItemIcon>{{ item.icon }}</mat-icon>
              <span matListItemTitle>{{ item.label }}</span>
            </a>
          }
        </mat-nav-list>
      </mat-sidenav>

      <!-- Main content -->
      <mat-sidenav-content class="main-content">

        <!-- Top toolbar -->
        <mat-toolbar class="toolbar">
          <button mat-icon-button (click)="drawer.toggle()">
            <mat-icon>menu</mat-icon>
          </button>
          <span class="toolbar-title">Workforce Management System</span>
          <span class="spacer"></span>

          <!-- User menu -->
          <button mat-button [matMenuTriggerFor]="userMenu" class="user-menu-btn">
            <mat-icon>account_circle</mat-icon>
            <span class="username">{{ authService.currentUser()?.username }}</span>
            <mat-icon>arrow_drop_down</mat-icon>
          </button>

          <mat-menu #userMenu="matMenu">
            <div class="menu-user-info">
              <strong>{{ authService.currentUser()?.username }}</strong>
              <span class="role-badge">{{ authService.userRole() }}</span>
            </div>
            <mat-divider></mat-divider>
            <button mat-menu-item [routerLink]="['/auth/change-password']">
              <mat-icon>lock</mat-icon> Change Password
            </button>
            <button mat-menu-item (click)="logout()">
              <mat-icon>logout</mat-icon> Logout
            </button>
          </mat-menu>
        </mat-toolbar>

        <!-- Loading bar -->
        @if (loadingService.isLoading()) {
          <mat-progress-bar mode="indeterminate" color="accent"></mat-progress-bar>
        }

        <!-- Page content -->
        <div class="page-content">
          <router-outlet></router-outlet>
        </div>

      </mat-sidenav-content>
    </mat-sidenav-container>
  `,
  styles: [`
    .sidenav-container { height: 100vh; }
    .sidenav {
      width: 240px;
      background: #1a237e;
      color: white;
    }
    .sidenav-header {
      display: flex; align-items: center; gap: 12px;
      padding: 20px 16px;
      background: rgba(0,0,0,0.2);
    }
    .logo-icon { color: #90caf9; font-size: 32px; }
    .logo-text { font-size: 22px; font-weight: 700; color: white; }
    .nav-item { color: rgba(255,255,255,0.85); margin: 2px 8px; border-radius: 8px; }
    .nav-item:hover { background: rgba(255,255,255,0.1); }
    .active-link { background: rgba(255,255,255,0.2) !important; color: white !important; }
    .toolbar { background: white; box-shadow: 0 2px 4px rgba(0,0,0,0.1); position: sticky; top: 0; z-index: 100; }
    .toolbar-title { font-size: 18px; font-weight: 600; color: #1a237e; }
    .spacer { flex: 1; }
    .user-menu-btn { color: #1a237e; }
    .username { margin: 0 4px; font-weight: 500; }
    .menu-user-info { padding: 12px 16px; display: flex; flex-direction: column; gap: 4px; }
    .role-badge {
      background: #e3f2fd; color: #1565c0;
      padding: 2px 8px; border-radius: 12px;
      font-size: 12px; width: fit-content;
    }
    .page-content { padding: 24px; }
    mat-progress-bar { position: sticky; top: 64px; z-index: 99; }
  `]
})
export class MainLayoutComponent {
  authService = inject(AuthService);
  loadingService = inject(LoadingService);

 private navItems: NavItem[] = [

  {
    label: 'Dashboard',
    icon: 'dashboard',
    route: '/dashboard'
  },

  {
    label: 'Employees',
    icon: 'people',
    route: '/employees',
    roles: ['Admin','Manager']
  },

  {
    label: 'Departments',
    icon: 'apartment',
    route: '/departments',
    roles: ['Admin']
  },

  {
  label: 'My Attendance',
  icon: 'access_time',
  route: '/attendance/my',
  roles: ['Employee']
},
{
  label: 'Attendance',
  icon: 'access_time',
  route: '/attendance/list',
  roles: ['Admin','Manager']
},

  {
    label: 'Leaves',
    icon: 'event_busy',
    route: '/leaves'
  },

  {
    label: 'Projects',
    icon: 'folder',
    route: '/projects',
    roles: ['Admin','Manager']
  },

  {
    label: 'Clients',
    icon: 'business',
    route: '/clients',
    roles: ['Admin','Manager']
  },

  {
    label: 'Allocations',
    icon: 'assignment',
    route: '/allocations',
    roles: ['Admin','Manager']
  },

  {
    label: 'Announcements',
    icon: 'campaign',
    route: '/announcements'
  }
];

  visibleNavItems = signal(this.filterNavItems());

  private filterNavItems(): NavItem[] {
    const role = this.authService.userRole();
    return this.navItems.filter(item =>
      !item.roles || item.roles.includes(role)
    );
  }

  logout(): void {
    this.authService.logout().subscribe();
  }
}

