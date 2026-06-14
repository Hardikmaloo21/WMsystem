
// ─────────────────────────────────────────────
// leave-list.component.ts (Employee view)
// frontend/src/app/leaves/leave-list/
// ─────────────────────────────────────────────

import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatTabsModule } from '@angular/material/tabs';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { LeaveService } from '../leave.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-leave-list',
  standalone: true,
  imports: [
    CommonModule, RouterLink, MatCardModule, MatTableModule,
    MatButtonModule, MatIconModule, MatChipsModule,
    MatTabsModule, MatPaginatorModule, MatDialogModule
  ],
  template: `
    <div class="page-container">
      <div class="page-header">
        <h1 class="page-title">Leave Management</h1>
        <button mat-raised-button color="primary"
                [routerLink]="['/leaves/apply']">
          <mat-icon>add</mat-icon> Apply Leave
        </button>
      </div>

      <mat-tab-group>

        <mat-tab label="My Leaves">
          <mat-card class="tab-card">
            <table mat-table [dataSource]="myLeaves()" class="full-width">

              <ng-container matColumnDef="type">
                <th mat-header-cell *matHeaderCellDef>Type</th>
                <td mat-cell *matCellDef="let l">{{ l.leaveType }}</td>
              </ng-container>

              <ng-container matColumnDef="from">
                <th mat-header-cell *matHeaderCellDef>From</th>
                <td mat-cell *matCellDef="let l">{{ l.fromDate | date:'mediumDate' }}</td>
              </ng-container>

              <ng-container matColumnDef="to">
                <th mat-header-cell *matHeaderCellDef>To</th>
                <td mat-cell *matCellDef="let l">{{ l.toDate | date:'mediumDate' }}</td>
              </ng-container>

              <ng-container matColumnDef="days">
                <th mat-header-cell *matHeaderCellDef>Days</th>
                <td mat-cell *matCellDef="let l">{{ l.totalDays }}</td>
              </ng-container>

              <ng-container matColumnDef="reason">
                <th mat-header-cell *matHeaderCellDef>Reason</th>
                <td mat-cell *matCellDef="let l">{{ l.reason || '—' }}</td>
              </ng-container>

              <ng-container matColumnDef="status">
                <th mat-header-cell *matHeaderCellDef>Status</th>
                <td mat-cell *matCellDef="let l">
                  <mat-chip [class]="statusClass(l.status)">{{ l.status }}</mat-chip>
                </td>
              </ng-container>

              <ng-container matColumnDef="actions">
                <th mat-header-cell *matHeaderCellDef></th>
                <td mat-cell *matCellDef="let l">
                  @if (l.status === 'Pending') {
                    <button mat-stroked-button color="warn"
                            (click)="cancelLeave(l.leaveId)">
                      Cancel
                    </button>
                  }
                </td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="myColumns"></tr>
              <tr mat-row *matRowDef="let row; columns: myColumns;"></tr>
            </table>

            <mat-paginator [length]="myTotal()" [pageSize]="10"
                           (page)="onMyPage($event)" showFirstLastButtons>
            </mat-paginator>
          </mat-card>
        </mat-tab>

        @if (authService.isManager()) {
          <mat-tab label="Pending Approvals">
            <mat-card class="tab-card">
              <table mat-table [dataSource]="pendingLeaves()" class="full-width">

                <ng-container matColumnDef="employee">
                  <th mat-header-cell *matHeaderCellDef>Employee</th>
                  <td mat-cell *matCellDef="let l">{{ l.employeeName }}</td>
                </ng-container>

                <ng-container matColumnDef="type">
                  <th mat-header-cell *matHeaderCellDef>Type</th>
                  <td mat-cell *matCellDef="let l">{{ l.leaveType }}</td>
                </ng-container>

                <ng-container matColumnDef="from">
                  <th mat-header-cell *matHeaderCellDef>From</th>
                  <td mat-cell *matCellDef="let l">{{ l.fromDate | date:'mediumDate' }}</td>
                </ng-container>

                <ng-container matColumnDef="to">
                  <th mat-header-cell *matHeaderCellDef>To</th>
                  <td mat-cell *matCellDef="let l">{{ l.toDate | date:'mediumDate' }}</td>
                </ng-container>

                <ng-container matColumnDef="actions">
                  <th mat-header-cell *matHeaderCellDef>Action</th>
                  <td mat-cell *matCellDef="let l">
                    <button mat-raised-button color="primary"
                            (click)="actionLeave(l.leaveId, 'Approve')">
                      Approve
                    </button>
                    <button mat-raised-button color="warn" class="ml-8"
                            (click)="actionLeave(l.leaveId, 'Reject')">
                      Reject
                    </button>
                  </td>
                </ng-container>

                <tr mat-header-row *matHeaderRowDef="pendingColumns"></tr>
                <tr mat-row *matRowDef="let row; columns: pendingColumns;"></tr>
              </table>
            </mat-card>
          </mat-tab>
        }

      </mat-tab-group>
    </div>
  `,
  styles: [`
    .page-container { max-width: 1200px; }
    .page-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px; }
    .page-title { font-size: 22px; font-weight: 700; color: #1a237e; margin: 0; }
    .tab-card { margin-top: 16px; }
    .full-width { width: 100%; }
    .chip-Pending  { background: #fff8e1 !important; color: #f57f17 !important; }
    .chip-Approved { background: #e8f5e9 !important; color: #2e7d32 !important; }
    .chip-Rejected { background: #ffebee !important; color: #c62828 !important; }
    .chip-Cancelled{ background: #f5f5f5 !important; color: #757575 !important; }
    .ml-8 { margin-left: 8px; }
  `]
})
export class LeaveListComponent implements OnInit {
  private leaveService = inject(LeaveService);
  authService = inject(AuthService);

  myLeaves = signal<any[]>([]);
  myTotal = signal(0);
  pendingLeaves = signal<any[]>([]);

  myColumns = ['type','from','to','days','reason','status','actions'];
  pendingColumns = ['employee','type','from','to','actions'];

  ngOnInit(): void {
    const empId = this.authService.currentUser()?.employeeId;
    if (empId) this.loadMyLeaves(empId);
    if (this.authService.isManager()) this.loadPending();
  }

  loadMyLeaves(empId: number, page = 1): void {
    this.leaveService.getByEmployee(empId, page).subscribe(res => {
      if (res.isSuccess) {
        this.myLeaves.set(res.data.items);
        this.myTotal.set(res.data.totalCount);
      }
    });
  }

  loadPending(): void {
    this.leaveService.getPending().subscribe(res => {
      if (res.isSuccess) this.pendingLeaves.set(res.data);
    });
  }

  onMyPage(e: PageEvent): void {
    const empId = this.authService.currentUser()?.employeeId;
    if (empId) this.loadMyLeaves(empId, e.pageIndex + 1);
  }

  cancelLeave(id: number): void {
    this.leaveService.cancel(id).subscribe(() => {
      const empId = this.authService.currentUser()?.employeeId;
      if (empId) this.loadMyLeaves(empId);
    });
  }

  actionLeave(id: number, action: string): void {
    const approverId = this.authService.currentUser()?.employeeId ?? 0;
    this.leaveService.approveReject({ leaveId: id, approverId, action }).subscribe(
      () => this.loadPending()
    );
  }

  statusClass(status: string): string {
    return `chip-${status}`;
  }
}
