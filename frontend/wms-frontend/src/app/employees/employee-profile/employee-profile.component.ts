import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { EmployeeService } from '../employee.service';

@Component({
  selector: 'app-employee-profile',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIconModule
  ],
  template: `
    @if(employee()) {

      <div class="page-header">
        <h1>Employee Profile</h1>

        <button
          mat-raised-button
          color="primary"
          [routerLink]="['/employees/edit', employee()?.employeeId]">

          <mat-icon>edit</mat-icon>
          Edit
        </button>
      </div>

      <mat-card class="profile-card">

        <div class="profile-header">

          <div class="avatar">
            {{ employee()?.firstName?.[0] }}
            {{ employee()?.lastName?.[0] }}
          </div>

          <div>
            <h2>{{ employee()?.fullName }}</h2>
            <p>{{ employee()?.email }}</p>
          </div>

        </div>

        <div class="grid">

          <div>
            <strong>Phone</strong>
            <p>{{ employee()?.phoneNumber }}</p>
          </div>

          <div>
            <strong>Gender</strong>
            <p>{{ employee()?.gender }}</p>
          </div>

          <div>
            <strong>Department</strong>
            <p>{{ employee()?.departmentName }}</p>
          </div>

          <div>
            <strong>Role</strong>
            <p>{{ employee()?.roleName }}</p>
          </div>

          <div>
            <strong>DOB</strong>
            <p>{{ employee()?.dob | date }}</p>
          </div>

          <div>
            <strong>DOJ</strong>
            <p>{{ employee()?.doj | date }}</p>
          </div>

          <div>
            <strong>Status</strong>
            <p>{{ employee()?.status }}</p>
          </div>

        </div>

      </mat-card>
    }
  `,
  styles: [`
    .page-header{
      display:flex;
      justify-content:space-between;
      align-items:center;
      margin-bottom:20px;
    }

    .profile-card{
      padding:24px;
    }

    .profile-header{
      display:flex;
      gap:20px;
      align-items:center;
      margin-bottom:30px;
    }

    .avatar{
      width:80px;
      height:80px;
      border-radius:50%;
      background:#1565c0;
      color:white;
      display:flex;
      align-items:center;
      justify-content:center;
      font-size:28px;
      font-weight:700;
    }

    .grid{
      display:grid;
      grid-template-columns:repeat(auto-fit,minmax(220px,1fr));
      gap:20px;
    }
  `]
})
export class EmployeeProfileComponent implements OnInit {

  private route = inject(ActivatedRoute);
  private service = inject(EmployeeService);

  employee = signal<any>(null);

  ngOnInit(): void {

    const id = Number(
      this.route.snapshot.paramMap.get('id')
    );

    this.service.getById(id)
      .subscribe(res => {

        if(res.isSuccess){
          this.employee.set(res.data);
        }

      });
  }
}