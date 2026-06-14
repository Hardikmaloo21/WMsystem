import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { EmployeeService } from '../employee.service';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatSnackBarModule
  ],
  template: `
    <mat-card class="employee-card">

      <div class="header">
        <h2>
          {{ isEditMode ? 'Edit Employee' : 'Add Employee' }}
        </h2>
      </div>

      <form [formGroup]="form" (ngSubmit)="save()">

        <div class="grid">

          <mat-form-field appearance="outline">
            <mat-label>First Name</mat-label>
            <input matInput formControlName="firstName">
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Last Name</mat-label>
            <input matInput formControlName="lastName">
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Email</mat-label>
            <input
              matInput
              formControlName="email"
              [readonly]="isEditMode">
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Phone Number</mat-label>
            <input matInput formControlName="phoneNumber">
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Gender</mat-label>
            <mat-select formControlName="gender">
              <mat-option value="Male">Male</mat-option>
              <mat-option value="Female">Female</mat-option>
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Date Of Birth</mat-label>
            <input
              matInput
              type="date"
              formControlName="dob">
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Date Of Joining</mat-label>
            <input
              matInput
              type="date"
              formControlName="doj">
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Department</mat-label>

            <mat-select formControlName="departmentId">

              <mat-option
                *ngFor="let dept of departments"
                [value]="dept.departmentId">

                {{ dept.departmentName }}

              </mat-option>

            </mat-select>

          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Role</mat-label>

            <mat-select formControlName="roleId">

              <mat-option
                *ngFor="let role of roles"
                [value]="role.roleId">

                {{ role.roleName }}

              </mat-option>

            </mat-select>

          </mat-form-field>

          <mat-form-field
            *ngIf="!isEditMode"
            appearance="outline">

            <mat-label>Username</mat-label>
            <input matInput formControlName="username">

          </mat-form-field>

          <mat-form-field
            *ngIf="!isEditMode"
            appearance="outline">

            <mat-label>Password</mat-label>
            <input
              matInput
              type="password"
              formControlName="password">

          </mat-form-field>

          <mat-form-field
            *ngIf="isEditMode"
            appearance="outline">

            <mat-label>Status</mat-label>

            <mat-select formControlName="status">

              <mat-option value="Active">
                Active
              </mat-option>

              <mat-option value="Inactive">
                Inactive
              </mat-option>

            </mat-select>

          </mat-form-field>

        </div>

        <div class="actions">

          <button
            mat-raised-button
            color="primary"
            type="submit"
            [disabled]="form.invalid">

            {{ isEditMode ? 'Update Employee' : 'Create Employee' }}

          </button>

        </div>

      </form>

    </mat-card>
  `,
  styles: [`
    .employee-card {
      padding: 24px;
    }

    .header {
      margin-bottom: 20px;
    }

    .grid {
      display: grid;
      grid-template-columns: repeat(auto-fit,minmax(250px,1fr));
      gap: 16px;
    }

    .actions {
      margin-top: 24px;
    }
  `]
})
export class EmployeeFormComponent implements OnInit {

  private fb = inject(FormBuilder);
  private employeeService = inject(EmployeeService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  form!: FormGroup;

  isEditMode = false;
  employeeId = 0;

  departments: any[] = [];
  roles: any[] = [];

  ngOnInit(): void {

    this.buildForm();

    this.loadDepartments();
    this.loadRoles();

    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.isEditMode = true;
      this.employeeId = Number(id);
      this.loadEmployee();
    }
  }

  buildForm(): void {

    this.form = this.fb.group({

      employeeId: [0],

      firstName: ['', Validators.required],
      lastName: ['', Validators.required],

      email: ['', [Validators.required, Validators.email]],

      phoneNumber: ['', Validators.required],

      gender: ['', Validators.required],

      dob: ['', Validators.required],
      doj: [''],

      departmentId: ['', Validators.required],
      roleId: ['', Validators.required],

      username: [''],
      password: [''],

      status: ['Active']
    });
  }

  loadDepartments(): void {

    this.employeeService
      .getDepartments()
      .subscribe(res => {

        this.departments = res.data || [];

      });
  }

  loadRoles(): void {

    this.employeeService
      .getRoles()
      .subscribe(res => {

        this.roles = res.data || [];

      });
  }

  loadEmployee(): void {

    this.employeeService
      .getById(this.employeeId)
      .subscribe(res => {

        if (res.isSuccess) {

          this.form.patchValue({
            employeeId: res.data.employeeId,
            firstName: res.data.firstName,
            lastName: res.data.lastName,
            email: res.data.email,
            phoneNumber: res.data.phoneNumber,
            gender: res.data.gender,
            dob: res.data.dob?.substring(0, 10),
            doj: res.data.doj?.substring(0, 10),
            departmentId: res.data.departmentId,
            roleId: res.data.roleId,
            status: res.data.status
          });

        }

      });
  }

  save(): void {

    if (this.form.invalid) {
      return;
    }

    const dto = this.form.getRawValue();

    if (this.isEditMode) {

      this.employeeService
        .update(this.employeeId, dto)
        .subscribe(() => {

          this.snackBar.open(
            'Employee updated successfully',
            'Close',
            { duration: 3000 }
          );

          this.router.navigate(['/employees']);
        });

    } else {

      this.employeeService
        .create(dto)
        .subscribe(() => {

          this.snackBar.open(
            'Employee created successfully',
            'Close',
            { duration: 3000 }
          );

          this.router.navigate(['/employees']);
        });

    }
  }
}