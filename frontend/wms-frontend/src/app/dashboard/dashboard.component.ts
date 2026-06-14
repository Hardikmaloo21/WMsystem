
// ─────────────────────────────────────────────
// dashboard.component.ts
// frontend/src/app/dashboard/dashboard.component.ts
// ─────────────────────────────────────────────

import {
  Component, OnInit, inject, signal,
  ElementRef, ViewChild
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { Chart, registerables } from 'chart.js';
import { DashboardService } from './dashboard.service';
import { DashboardSummaryDto } from './dashboard.model';

Chart.register(...registerables);

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule, MatCardModule, MatIconModule,
    MatProgressSpinnerModule, MatDividerModule
  ],
  template: `
    <div class="dashboard-container">
      <h1 class="page-title">Dashboard</h1>

      @if (loading()) {
        <div class="loading-center">
          <mat-spinner diameter="48"></mat-spinner>
        </div>
      }

      @if (summary(); as data) {

        <mat-card class="welcome-card">

  <h2>
    Welcome Back
  </h2>

  <p>
    Workforce Management Dashboard
  </p>

</mat-card>
        <!-- KPI Cards -->
        <div class="kpi-grid">
          @for (card of kpiCards(data); track card.label) {
            <mat-card class="kpi-card" [class]="card.colorClass">
              <mat-card-content>
                <div class="kpi-content">
                  <div class="kpi-info">
                    <span class="kpi-value">{{ card.value }}</span>
                    <span class="kpi-label">{{ card.label }}</span>
                  </div>
                  <mat-icon class="kpi-icon">{{ card.icon }}</mat-icon>
                </div>
              </mat-card-content>
            </mat-card>
          }
        </div>

        <!-- Charts Row -->
        <div class="charts-grid">

          <mat-card class="chart-card">
            <mat-card-header>
              <mat-card-title>Attendance Trend (Last 7 Days)</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <canvas #attendanceChart></canvas>
            </mat-card-content>
          </mat-card>

          <mat-card class="chart-card">
            <mat-card-header>
              <mat-card-title>Department Distribution</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <canvas #deptChart></canvas>
            </mat-card-content>
          </mat-card>

          <mat-card class="chart-card">
            <mat-card-header>
              <mat-card-title>Leave Trend (Last 6 Months)</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <canvas #leaveChart></canvas>
            </mat-card-content>
          </mat-card>

          <mat-card class="chart-card">
            <mat-card-header>
              <mat-card-title>Project Status</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <canvas #projectChart></canvas>
            </mat-card-content>
          </mat-card>

        </div>

        <!-- Recent Announcements -->
        <mat-card class="announcements-card">
          <mat-card-header>
            <mat-card-title>
              <mat-icon>campaign</mat-icon> Recent Announcements
            </mat-card-title>
          </mat-card-header>
          <mat-card-content>
            @for (ann of data.recentAnnouncements; track ann.announcementId) {
              <div class="announcement-item">
                <div class="ann-title">{{ ann.title }}</div>
                <div class="ann-meta">
                  {{ ann.createdByName }} · {{ ann.createdOn | date:'mediumDate' }}
                </div>
                <p class="ann-message">{{ ann.message }}</p>
              </div>
              <mat-divider></mat-divider>
            }
            @empty {
              <p class="empty-state">No active announcements.</p>
            }
          </mat-card-content>
        </mat-card>
      }
    </div>
  `,
  styles: [`
    .dashboard-container { max-width: 1400px; }
    .page-title { font-size: 24px; font-weight: 700; color: #1a237e; margin-bottom: 24px; }
    .loading-center { display: flex; justify-content: center; padding: 60px; }

    .kpi-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
      gap: 16px; margin-bottom: 24px;
    }
    .kpi-card { border-radius: 12px; cursor: default; }
    .kpi-content { display: flex; justify-content: space-between; align-items: center; }
    .kpi-info { display: flex; flex-direction: column; gap: 4px; }
    .kpi-value { font-size: 32px; font-weight: 700; color: white; }
    .kpi-label { font-size: 13px; color: rgba(255,255,255,0.85); }
    .kpi-icon { font-size: 40px; height: 40px; width: 40px; opacity: 0.5; color: white; }

    .kpi-blue   { background: linear-gradient(135deg,#1565c0,#1e88e5); }
    .kpi-green  { background: linear-gradient(135deg,#2e7d32,#43a047); }
    .kpi-purple { background: linear-gradient(135deg,#6a1b9a,#ab47bc); }
    .kpi-orange { background: linear-gradient(135deg,#e65100,#fb8c00); }
    .kpi-teal   { background: linear-gradient(135deg,#00695c,#26a69a); }
    .kpi-red    { background: linear-gradient(135deg,#b71c1c,#ef5350); }
    .kpi-indigo { background: linear-gradient(135deg,#283593,#5c6bc0); }
    .kpi-pink   { background: linear-gradient(135deg,#880e4f,#ec407a); }

    .charts-grid {
      display: grid;
      grid-template-columns: repeat(2, 1fr);
      gap: 24px; margin-bottom: 24px;
    }
    .chart-card { border-radius: 12px; }
    .chart-card canvas { max-height: 280px; }
.welcome-card{
  margin-bottom:24px;
  border-radius:12px;
}
    .announcements-card { border-radius: 12px; }
    .announcement-item { padding: 12px 0; }
    .ann-title { font-weight: 600; font-size: 15px; color: #1a237e; }
    .ann-meta { font-size: 12px; color: #888; margin: 4px 0; }
    .ann-message { font-size: 14px; color: #555; margin: 4px 0 0; }
    .empty-state { color: #999; text-align: center; padding: 24px; }

    @media (max-width: 960px) {
      .charts-grid { grid-template-columns: 1fr; }
    }
  `]
})
export class DashboardComponent implements OnInit { 
  private dashService = inject(DashboardService);

  loading = signal(true);
  summary = signal<DashboardSummaryDto | null>(null);

  @ViewChild('attendanceChart') attendanceChartRef!: ElementRef;
  @ViewChild('deptChart') deptChartRef!: ElementRef;
  @ViewChild('leaveChart') leaveChartRef!: ElementRef;
  @ViewChild('projectChart') projectChartRef!: ElementRef;

ngOnInit(): void {
  this.dashService.getSummary().subscribe({
    next: res => {
      this.loading.set(false);

      if (res.isSuccess) {
        this.summary.set(res.data);

        setTimeout(() => {
          this.buildCharts(res.data);
        }, 200);
      }
    },
    error: () => {
      this.loading.set(false);
    }
  });
}


  kpiCards(data: DashboardSummaryDto) {
    return [
      { label: 'Total Employees',    value: data.totalEmployees,      icon: 'people',         colorClass: 'kpi-blue' },
      { label: 'Active Employees',   value: data.activeEmployees,     icon: 'person_check',   colorClass: 'kpi-green' },
      { label: 'Departments',        value: data.totalDepartments,    icon: 'apartment',      colorClass: 'kpi-purple' },
      { label: 'Total Projects',     value: data.totalProjects,       icon: 'folder',         colorClass: 'kpi-orange' },
      { label: 'Active Projects',    value: data.activeProjects,      icon: 'work',           colorClass: 'kpi-teal' },
      { label: 'Total Clients',      value: data.totalClients,        icon: 'business',       colorClass: 'kpi-indigo' },
      { label: "Today's Attendance", value: data.todayAttendanceCount,icon: 'how_to_reg',     colorClass: 'kpi-pink' },
      { label: 'Pending Leaves',     value: data.pendingLeaves,       icon: 'pending_actions',colorClass: 'kpi-red' }
    ];
  }

  private buildCharts(data: DashboardSummaryDto): void {

  // Attendance Trend
  new Chart(this.attendanceChartRef.nativeElement, {
    type: 'bar',
    data: {
      labels: data.attendanceTrend.map(a => a.date),
      datasets: [
        {
          label: 'WFO',
          data: data.attendanceTrend.map((a: any) => a.wfo ?? a.wFO ?? 0),
          backgroundColor: '#1565c0'
        },
        {
          label: 'WFH',
          data: data.attendanceTrend.map((a: any) => a.wfh ?? a.wFH ?? 0),
          backgroundColor: '#43a047'
        }
      ]
    },
    options: {
      responsive: true,
      plugins: {
        legend: {
          position: 'bottom'
        }
      },
      scales: {
        y: {
          beginAtZero: true
        }
      }
    }
  });

  // Department Distribution
  new Chart(this.deptChartRef.nativeElement, {
    type: 'doughnut',
    data: {
      labels: data.departmentDistribution.map(d => d.departmentName),
      datasets: [{
        data: data.departmentDistribution.map(d => d.employeeCount),
        backgroundColor: [
          '#1565c0',
          '#43a047',
          '#fb8c00',
          '#ab47bc',
          '#ef5350',
          '#26a69a'
        ]
      }]
    },
    options: {
      responsive: true,
      plugins: {
        legend: {
          position: 'bottom'
        }
      }
    }
  });

  // Leave Trend
  new Chart(this.leaveChartRef.nativeElement, {
    type: 'line',
    data: {
      labels: data.leaveTrend.map(l => l.month),
      datasets: [
        {
          label: 'Approved',
          data: data.leaveTrend.map(l => l.approved),
          borderColor: '#43a047',
          backgroundColor: '#43a047',
          tension: 0.4
        },
        {
          label: 'Pending',
          data: data.leaveTrend.map(l => l.pending),
          borderColor: '#fb8c00',
          backgroundColor: '#fb8c00',
          tension: 0.4
        },
        {
          label: 'Rejected',
          data: data.leaveTrend.map(l => l.rejected),
          borderColor: '#ef5350',
          backgroundColor: '#ef5350',
          tension: 0.4
        }
      ]
    },
    options: {
      responsive: true,
      plugins: {
        legend: {
          position: 'bottom'
        }
      },
      scales: {
        y: {
          beginAtZero: true
        }
      }
    }
  });

  // Project Status
  new Chart(this.projectChartRef.nativeElement, {
    type: 'pie',
    data: {
      labels: data.projectStatusDistribution.map(p => p.status),
      datasets: [{
        data: data.projectStatusDistribution.map(p => p.count),
        backgroundColor: [
          '#1565c0', // Active
          '#43a047', // Completed
          '#fb8c00', // OnHold
          '#ef5350',
          '#ab47bc'
        ]
      }]
    },
    options: {
      responsive: true,
      plugins: {
        legend: {
          position: 'bottom'
        }
      }
    }
  });
}
}

