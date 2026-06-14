import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';
import { AttendanceService } from '../../attendance/attendance.service';
import { LeaveService } from '../../leaves/leave.service';

@Component({
  selector: 'app-employee-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  template: `
    <div class="dashboard">

      <div class="header">
        <h1>
          Welcome,
          {{ authService.currentUser()?.username }}
        </h1>

        <p>
          Employee Dashboard
        </p>
      </div>

      <div class="stats-grid">

        <div class="stat-card">
          <h3>Attendance Records</h3>
          <div class="value">
            {{ attendanceCount() }}
          </div>
        </div>

        <div class="stat-card">
          <h3>Leaves Applied</h3>
          <div class="value">
            {{ leaveCount() }}
          </div>
        </div>

        <div class="stat-card">
          <h3>Role</h3>
          <div class="value">
            {{ authService.userRole() }}
          </div>
        </div>

      </div>

      <div class="quick-actions">

        <a
          routerLink="/attendance/my"
          class="action-card">

          <h3>Attendance</h3>
          <p>
            Mark WFO / WFH attendance
          </p>

        </a>

        <a
          routerLink="/leaves"
          class="action-card">

          <h3>Leaves</h3>
          <p>
            Apply and track leaves
          </p>

        </a>

        <a
          routerLink="/announcements"
          class="action-card">

          <h3>Announcements</h3>
          <p>
            Company updates
          </p>

        </a>

      </div>

    </div>
  `,
  styles: [`
    .dashboard{
      padding:20px;
    }

    .header{
      margin-bottom:24px;
    }

    .header h1{
      margin:0;
      color:#1a237e;
    }

    .header p{
      color:#666;
      margin-top:5px;
    }

    .stats-grid{
      display:grid;
      grid-template-columns:repeat(auto-fit,minmax(250px,1fr));
      gap:20px;
      margin-bottom:30px;
    }

    .stat-card{
      background:white;
      border-radius:12px;
      padding:20px;
      box-shadow:0 2px 8px rgba(0,0,0,.08);
    }

    .stat-card h3{
      margin:0;
      color:#555;
      font-size:14px;
    }

    .value{
      margin-top:10px;
      font-size:28px;
      font-weight:bold;
      color:#1a237e;
    }

    .quick-actions{
      display:grid;
      grid-template-columns:repeat(auto-fit,minmax(250px,1fr));
      gap:20px;
    }

    .action-card{
      text-decoration:none;
      color:inherit;
      background:white;
      border-radius:12px;
      padding:20px;
      box-shadow:0 2px 8px rgba(0,0,0,.08);
      transition:.2s;
    }

    .action-card:hover{
      transform:translateY(-2px);
    }

    .action-card h3{
      color:#1a237e;
      margin-bottom:10px;
    }
  `]
})
export class EmployeeDashboardComponent implements OnInit {

  authService = inject(AuthService);

  private attendanceService =
    inject(AttendanceService);

  private leaveService =
    inject(LeaveService);

  attendanceCount = signal(0);
  leaveCount = signal(0);

  ngOnInit(): void {

    const empId =
      this.authService.currentUser()?.employeeId;

    if (!empId) return;

    const today = new Date();

    this.attendanceService
      .getMonthly(
        empId,
        today.getFullYear(),
        today.getMonth() + 1
      )
      .subscribe(res => {

        this.attendanceCount.set(
          res.data?.length ?? 0
        );

      });

    this.leaveService
      .getByEmployee(empId, 1)
      .subscribe(res => {

        this.leaveCount.set(
          res.data?.totalCount ?? 0
        );

      });
  }
}