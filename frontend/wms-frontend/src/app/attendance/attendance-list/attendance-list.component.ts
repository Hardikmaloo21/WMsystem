import {
  Component,
  OnInit,
  inject,
  signal,
  computed
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

import { AttendanceService } from '../attendance.service';
import { EmployeeService } from '../../employees/employee.service';

@Component({
  selector: 'app-attendance-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatCardModule,
    MatChipsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule
  ],
  template: `
    <div class="page-header">
      <h1>Attendance Management</h1>
    </div>

    <mat-card class="filter-card">

      <div class="filters">

        <mat-form-field appearance="outline">
          <mat-label>Employee</mat-label>

          <mat-select [(ngModel)]="selectedEmployee">

            <mat-option [value]="0">
              All Employees
            </mat-option>

            <mat-option
              *ngFor="let emp of employees()"
              [value]="emp.employeeId">

              {{ emp.fullName }}

            </mat-option>

          </mat-select>

        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Month</mat-label>

          <input
            matInput
            type="number"
            min="1"
            max="12"
            [(ngModel)]="selectedMonth">
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Year</mat-label>

          <input
            matInput
            type="number"
            [(ngModel)]="selectedYear">
        </mat-form-field>

        <button
          mat-raised-button
          color="primary"
          (click)="search()">

          Search

        </button>

        <button
          mat-raised-button
          (click)="load()">

          Reset

        </button>

      </div>

    </mat-card>

    <div class="stats-grid">

      <mat-card class="stat-card">
        <div class="stat-value">
          {{ totalRecords() }}
        </div>

        <div class="stat-label">
          Total Records
        </div>
      </mat-card>

      <mat-card class="stat-card">
        <div class="stat-value">
          {{ wfoCount() }}
        </div>

        <div class="stat-label">
          WFO
        </div>
      </mat-card>

      <mat-card class="stat-card">
        <div class="stat-value">
          {{ wfhCount() }}
        </div>

        <div class="stat-label">
          WFH
        </div>
      </mat-card>

      <mat-card class="stat-card">
        <div class="stat-value">
          {{ totalHours() }}
        </div>

        <div class="stat-label">
          Total Hours
        </div>
      </mat-card>

    </div>

    <mat-card>

      <table
        mat-table
        [dataSource]="records()">

        <ng-container matColumnDef="employee">
          <th mat-header-cell *matHeaderCellDef>
            Employee
          </th>

          <td mat-cell *matCellDef="let r">
            {{ r.employeeName || '-' }}
          </td>
        </ng-container>

        <ng-container matColumnDef="date">
          <th mat-header-cell *matHeaderCellDef>
            Date
          </th>

          <td mat-cell *matCellDef="let r">
            {{ r.attendanceDate | date:'dd/MM/yyyy' }}
          </td>
        </ng-container>

        <ng-container matColumnDef="checkIn">
          <th mat-header-cell *matHeaderCellDef>
            Check In
          </th>

          <td mat-cell *matCellDef="let r">
            {{ r.checkIn | date:'shortTime' }}
          </td>
        </ng-container>

        <ng-container matColumnDef="checkOut">
          <th mat-header-cell *matHeaderCellDef>
            Check Out
          </th>

          <td mat-cell *matCellDef="let r">
            {{
              r.checkOut
                ? (r.checkOut | date:'shortTime')
                : '-'
            }}
          </td>
        </ng-container>

        <ng-container matColumnDef="hours">
          <th mat-header-cell *matHeaderCellDef>
            Hours
          </th>

          <td mat-cell *matCellDef="let r">
            {{ r.totalHours || 0 }}
          </td>
        </ng-container>

        <ng-container matColumnDef="mode">
          <th mat-header-cell *matHeaderCellDef>
            Work Mode
          </th>

          <td mat-cell *matCellDef="let r">
            {{ r.workMode }}
          </td>
        </ng-container>

        <ng-container matColumnDef="status">
          <th mat-header-cell *matHeaderCellDef>
            Status
          </th>

          <td mat-cell *matCellDef="let r">

            <mat-chip
              [class.complete]="r.status === 'Complete'"
              [class.progress]="r.status !== 'Complete'">

              {{ r.status }}

            </mat-chip>

          </td>

        </ng-container>

        <tr
          mat-header-row
          *matHeaderRowDef="columns">
        </tr>

        <tr
          mat-row
          *matRowDef="
            let row;
            columns: columns
          ">
        </tr>

      </table>

    </mat-card>
  `,
  styles: [`
    .page-header{
      margin-bottom:20px;
    }

    .filter-card{
      margin-bottom:20px;
      padding:16px;
    }

    .filters{
      display:flex;
      gap:16px;
      flex-wrap:wrap;
      align-items:center;
    }

    .stats-grid{
      display:grid;
      grid-template-columns:repeat(4,1fr);
      gap:16px;
      margin-bottom:20px;
    }

    .stat-card{
      padding:20px;
      text-align:center;
    }

    .stat-value{
      font-size:30px;
      font-weight:700;
    }

    .stat-label{
      margin-top:8px;
      color:#666;
    }

    table{
      width:100%;
    }

    .complete{
      background:#d4edda;
      color:#155724;
    }

    .progress{
      background:#fff3cd;
      color:#856404;
    }
  `]
})
export class AttendanceListComponent
implements OnInit {

  private service =
    inject(AttendanceService);

  private employeeService =
    inject(EmployeeService);

  records = signal<any[]>([]);
  employees = signal<any[]>([]);

  selectedEmployee = 0;
  selectedMonth = new Date().getMonth() + 1;
  selectedYear = new Date().getFullYear();

  columns = [
    'employee',
    'date',
    'checkIn',
    'checkOut',
    'hours',
    'mode',
    'status'
  ];

  totalRecords = computed(
    () => this.records().length
  );

  wfoCount = computed(
    () =>
      this.records().filter(
        x => x.workMode === 'WFO'
      ).length
  );

  wfhCount = computed(
    () =>
      this.records().filter(
        x => x.workMode === 'WFH'
      ).length
  );

  totalHours = computed(
    () =>
      this.records()
        .reduce(
          (sum, x) =>
            sum + (x.totalHours || 0),
          0
        )
        .toFixed(2)
  );

  ngOnInit(): void {

    this.load();

    this.employeeService
      .getAll('', null, null, '', 1, 500)
      .subscribe(res => {

        if (res.isSuccess) {

          this.employees.set(
            res.data.items
          );

        }

      });

  }

  load() {

    this.service
      .getToday()
      .subscribe(res => {

        if (res.isSuccess) {

          this.records.set(
            res.data
          );

        }

      });

  }

  search() {

    if (!this.selectedEmployee) {

      this.load();
      return;

    }

    this.service
      .getMonthly(
        this.selectedEmployee,
        this.selectedYear,
        this.selectedMonth
      )
      .subscribe(res => {

        if (res.isSuccess) {

          this.records.set(
            res.data
          );

        }

      });

  }

}