import { Component } from '@angular/core';
import {
  RouterLink,
  RouterLinkActive,
  RouterOutlet
} from '@angular/router';

@Component({
  selector: 'app-leaves',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive
  ],
  template: `
    <div class="leave-nav">

      <a
        routerLink="/leaves/list"
        routerLinkActive="active">

        Leave List

      </a>

      <a
        routerLink="/leaves/apply"
        routerLinkActive="active">

        Apply Leave

      </a>



    </div>

    <router-outlet></router-outlet>
  `,
  styles:[`
    .leave-nav{
      display:flex;
      gap:12px;
      margin-bottom:20px;
    }

    .leave-nav a{
      text-decoration:none;
      padding:10px 16px;
      border-radius:8px;
      background:#f5f5f5;
      color:#333;
    }

    .active{
      background:#1976d2;
      color:white !important;
    }
  `]
})
export class LeavesComponent {}