import {
  Component,
  inject,
  signal
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';

import { AttendanceService }
from '../attendance.service';

import { EmployeeService }
from '../../employees/employee.service';

@Component({
  selector:'app-attendance-report',
  standalone:true,
  imports:[
    CommonModule,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatSelectModule,
    MatButtonModule
  ],
  template:`

    <h1>Attendance Reports</h1>

    <mat-card class="filter-card">

      <mat-form-field>

        <mat-label>Employee</mat-label>

        <mat-select
          [(ngModel)]="employeeId">

          <mat-option
            *ngFor="let e of employees()"
            [value]="e.employeeId">

            {{ e.fullName }}

          </mat-option>

        </mat-select>

      </mat-form-field>

      <button
        mat-raised-button
        color="primary"
        (click)="generate()">

        Generate Report

      </button>

    </mat-card>

    <div
      class="stats"
      *ngIf="records().length">

      <mat-card>

        <h2>
          {{ records().length }}
        </h2>

        <p>Present Days</p>

      </mat-card>

      <mat-card>

        <h2>
          {{ totalHours }}
        </h2>

        <p>Total Hours</p>

      </mat-card>

      <mat-card>

        <h2>
          {{ averageHours }}
        </h2>

        <p>Average Hours</p>

      </mat-card>

    </div>

  `,
  styles:[`
    .filter-card{
      padding:20px;
      margin-bottom:20px;
    }

    .stats{
      display:grid;
      grid-template-columns:
      repeat(3,1fr);
      gap:16px;
    }

    mat-card{
      text-align:center;
      padding:20px;
    }
  `]
})
export class AttendanceReportComponent{

  private attendanceService =
    inject(AttendanceService);

  private employeeService =
    inject(EmployeeService);

  employees = signal<any[]>([]);
  records = signal<any[]>([]);

  employeeId = 0;

  totalHours = 0;
  averageHours = 0;

  constructor(){

    this.employeeService
      .getAll('',null,null,'',1,500)
      .subscribe(res=>{

        if(res.isSuccess){

          this.employees.set(
            res.data.items
          );

        }

      });

  }

  generate(){

    if(!this.employeeId)
      return;

    const now = new Date();

    this.attendanceService
      .getMonthly(
        this.employeeId,
        now.getFullYear(),
        now.getMonth()+1
      )
      .subscribe(res=>{

        if(res.isSuccess){

          this.records.set(
            res.data
          );

          this.totalHours =
            res.data.reduce(
              (s:any,x:any)=>
                s+(x.totalHours||0),
              0
            );

          this.averageHours =
            this.records().length
              ? Number(
                  (
                    this.totalHours /
                    this.records().length
                  ).toFixed(2)
                )
              : 0;
        }

      });

  }
}