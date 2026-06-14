import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AttendanceService } from '../attendance.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-my-attendance',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './my-attendance.component.html',
  styleUrls: ['./my-attendance.component.scss']
})
export class MyAttendanceComponent implements OnInit {

  private attendanceService =
    inject(AttendanceService);

  private authService =
    inject(AuthService);

  workMode = 'WFO';

  records = signal<any[]>([]);

  employeeId = 0;

  ngOnInit(): void {

    this.employeeId =
      this.authService.currentUser()?.employeeId ?? 0;

    const today = new Date();

    this.loadAttendance(
      today.getFullYear(),
      today.getMonth() + 1
    );
  }

  loadAttendance(
    year: number,
    month: number
  ) {

    this.attendanceService
      .getMonthly(
        this.employeeId,
        year,
        month
      )
      .subscribe(res => {

        this.records.set(
          res.data
        );

      });
  }

  checkIn() {

    this.attendanceService
      .checkIn(
        this.employeeId,
        this.workMode
      )
      .subscribe(() => {

        const today = new Date();

        this.loadAttendance(
          today.getFullYear(),
          today.getMonth() + 1
        );

      });
  }

  checkOut() {

    this.attendanceService
      .checkOut(
        this.employeeId
      )
      .subscribe(() => {

        const today = new Date();

        this.loadAttendance(
          today.getFullYear(),
          today.getMonth() + 1
        );

      });
      
  }

  
}