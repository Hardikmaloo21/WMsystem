import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../core/services/auth.service';

@Component({
  selector: 'app-attendance',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    RouterLink,
    RouterLinkActive
  ],
  template: `
    <div class="attendance-nav">

      @if(authService.userRole() === 'Employee') {

        <a
          routerLink="/attendance/my"
          routerLinkActive="active">

          My Attendance

        </a>

      }

      @if(authService.userRole() === 'Admin' ||
          authService.userRole() === 'Manager') {

        <a
          routerLink="/attendance/list"
          routerLinkActive="active">

          Attendance List

        </a>

        <a
          routerLink="/attendance/report"
          routerLinkActive="active">

          Reports

        </a>

      }

    </div>

    <router-outlet></router-outlet>
  `,
  styles: [`
    .attendance-nav{
      display:flex;
      gap:16px;
      margin-bottom:20px;
    }

    .attendance-nav a{
      text-decoration:none;
      padding:10px 16px;
      border-radius:8px;
      background:#f5f5f5;
      color:#333;
    }

    .active{
      background:#1976d2 !important;
      color:white !important;
    }
  `]
})
export class AttendanceComponent {

  authService = inject(AuthService);

}