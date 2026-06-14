
// ─────────────────────────────────────────────
// employee-list.component.ts
// frontend/src/app/employees/employee-list/
// ─────────────────────────────────────────────

import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { EmployeeService } from '../employee.service';
import { AuthService } from '../../core/services/auth.service';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [
    CommonModule, RouterLink, ReactiveFormsModule,
    MatTableModule, MatPaginatorModule, MatSortModule,
    MatCardModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatButtonModule, MatIconModule,
    MatChipsModule, MatDialogModule, MatTooltipModule
  ],
  template: `
    <div class="page-container">
      <!-- Header -->
      <div class="page-header">
        <h1 class="page-title">Employees</h1>
        @if (authService.isAdmin()) {
          <button mat-raised-button color="primary"
                  [routerLink]="['/employees/add']">
            <mat-icon>add</mat-icon> Add Employee
          </button>
        }
      </div>

      <!-- Filters -->
      <mat-card class="filter-card">
        <form [formGroup]="filterForm" class="filter-form">
          <mat-form-field appearance="outline">
            <mat-label>Search</mat-label>
            <input matInput formControlName="search"
                   placeholder="Name, Email, Phone..." />
            <mat-icon matSuffix>search</mat-icon>
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Department</mat-label>
            <mat-select formControlName="departmentId">
              <mat-option [value]="null">All Departments</mat-option>
              @for (dept of departments(); track dept.departmentId) {
                <mat-option [value]="dept.departmentId">
                  {{ dept.departmentName }}
                </mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Status</mat-label>
            <mat-select formControlName="status">
              <mat-option value="">All</mat-option>
              <mat-option value="Active">Active</mat-option>
              <mat-option value="Inactive">Inactive</mat-option>
            </mat-select>
          </mat-form-field>

          <button mat-stroked-button type="button" (click)="resetFilters()">
            <mat-icon>clear</mat-icon> Clear
          </button>
        </form>
      </mat-card>

      <!-- Table -->
      <mat-card>
        <table mat-table [dataSource]="employees()" class="full-width">

          <ng-container matColumnDef="name">
            <th mat-header-cell *matHeaderCellDef>Name</th>
            <td mat-cell *matCellDef="let emp">
              <div class="emp-name-cell">
                <div class="avatar">
                  {{ emp.firstName[0] }}{{ emp.lastName[0] }}
                </div>
                <div>
                  <div class="emp-name">{{ emp.firstName }} {{ emp.lastName }}</div>
                  <div class="emp-email">{{ emp.email }}</div>
                </div>
              </div>
            </td>
          </ng-container>

          <ng-container matColumnDef="department">
            <th mat-header-cell *matHeaderCellDef>Department</th>
            <td mat-cell *matCellDef="let emp">{{ emp.departmentName }}</td>
          </ng-container>

          <ng-container matColumnDef="role">
            <th mat-header-cell *matHeaderCellDef>Role</th>
            <td mat-cell *matCellDef="let emp">{{ emp.roleName }}</td>
          </ng-container>

          <ng-container matColumnDef="phone">
            <th mat-header-cell *matHeaderCellDef>Phone</th>
            <td mat-cell *matCellDef="let emp">{{ emp.phoneNumber }}</td>
          </ng-container>

          <ng-container matColumnDef="doj">
            <th mat-header-cell *matHeaderCellDef>Join Date</th>
            <td mat-cell *matCellDef="let emp">{{ emp.doj | date:'mediumDate' }}</td>
          </ng-container>

          <ng-container matColumnDef="status">
            <th mat-header-cell *matHeaderCellDef>Status</th>
            <td mat-cell *matCellDef="let emp">
              <mat-chip [class]="emp.status === 'Active' ? 'chip-active' : 'chip-inactive'">
                {{ emp.status }}
              </mat-chip>
            </td>
          </ng-container>

          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let emp">
              <button mat-icon-button [routerLink]="['/employees/profile', emp.employeeId]"
                      matTooltip="View Profile">
                <mat-icon>visibility</mat-icon>
              </button>
              @if (authService.isAdmin()) {
                <button mat-icon-button color="primary"
                        [routerLink]="['/employees/edit', emp.employeeId]"
                        matTooltip="Edit">
                  <mat-icon>edit</mat-icon>
                </button>
                <button
  mat-icon-button
  [color]="emp.status === 'Active' ? 'warn' : 'primary'"
  (click)="confirmDelete(emp)"
  [matTooltip]="
    emp.status === 'Active'
      ? 'Deactivate'
      : 'Activate'
  ">

  <mat-icon>
    {{
      emp.status === 'Active'
        ? 'person_off'
        : 'person'
    }}
  </mat-icon>

                </button>
              }
            </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>

          <tr class="mat-row" *matNoDataRow>
            <td class="mat-cell no-data" [attr.colspan]="displayedColumns.length">
              No employees found.
            </td>
          </tr>

        </table>

        <mat-paginator
          [length]="totalCount()"
          [pageSize]="pageSize"
          [pageSizeOptions]="[10, 25, 50]"
          (page)="onPageChange($event)"
          showFirstLastButtons>
        </mat-paginator>
      </mat-card>
    </div>
  `,
  styles: [`
    .page-container { max-width: 1200px; }
    .page-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px; }
    .page-title { font-size: 22px; font-weight: 700; color: #1a237e; margin: 0; }
    .filter-card { margin-bottom: 20px; padding: 16px; }
    .filter-form { display: flex; gap: 16px; align-items: center; flex-wrap: wrap; }
    .filter-form mat-form-field { min-width: 180px; }
    .full-width { width: 100%; }
    .emp-name-cell { display: flex; align-items: center; gap: 12px; }
    .avatar {
      width: 36px; height: 36px; border-radius: 50%;
      background: #1565c0; color: white;
      display: flex; align-items: center; justify-content: center;
      font-weight: 600; font-size: 13px; flex-shrink: 0;
    }
    .emp-name { font-weight: 500; }
    .emp-email { font-size: 12px; color: #888; }
    .chip-active  { background: #e8f5e9 !important; color: #2e7d32 !important; }
    .chip-inactive{ background: #ffebee !important; color: #c62828 !important; }
    .no-data { text-align: center; padding: 32px; color: #888; }
  `]
})
export class EmployeeListComponent implements OnInit {
  private empService = inject(EmployeeService);
  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  authService = inject(AuthService);

  employees = signal<any[]>([]);
  totalCount = signal(0);
  departments = signal<any[]>([]);
  pageNumber = 1;
  pageSize = 10;

  displayedColumns = ['name','department','role','phone','doj','status','actions'];

  filterForm = this.fb.group({
    search: [''],
    departmentId: [null],
    status: ['Active']
  });

  ngOnInit(): void {
    this.loadDepartments();
    this.loadEmployees();

    this.filterForm.valueChanges.pipe(
      debounceTime(400),
      distinctUntilChanged()
    ).subscribe(() => {
      this.pageNumber = 1;
      this.loadEmployees();
    });
  }

  loadEmployees(): void {
    const { search, departmentId, status } = this.filterForm.value;
    this.empService.getAll(search ?? '', departmentId, null, status ?? '',
      this.pageNumber, this.pageSize).subscribe(res => {
      if (res.isSuccess) {
        this.employees.set(res.data.items);
        this.totalCount.set(res.data.pagination.totalCount);
      }
    });
  }

  loadDepartments(): void {
    this.empService.getDepartments().subscribe(res => {
      if (res.isSuccess) this.departments.set(res.data);
    });
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadEmployees();
  }

  resetFilters(): void {
    this.filterForm.reset({ search: '', departmentId: null, status: 'Active' });
  }

  confirmDelete(emp: any): void {

  const isActive = emp.status === 'Active';

  const ref = this.dialog.open(ConfirmDialogComponent, {
    data: {
      title: isActive
        ? 'Deactivate Employee'
        : 'Activate Employee',

      message: isActive
        ? `Are you sure you want to deactivate ${emp.firstName} ${emp.lastName}?`
        : `Are you sure you want to activate ${emp.firstName} ${emp.lastName}?`,

      confirmText: isActive
        ? 'Deactivate'
        : 'Activate'
    }
  });

  ref.afterClosed().subscribe(confirmed => {
    if (confirmed) {
      const newStatus =
  emp.status === 'Active'
    ? 'Inactive'
    : 'Active';

this.empService
  .updateStatus(emp.employeeId, newStatus)
  .subscribe(() => {
    this.loadEmployees();
  });
    }
  });
  }
}