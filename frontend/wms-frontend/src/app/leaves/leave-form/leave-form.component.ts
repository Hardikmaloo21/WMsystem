import {
  Component,
  inject
} from '@angular/core';

import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { Router } from '@angular/router';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';

import { LeaveService } from '../leave.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-leave-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule
  ],
  template: `
    <div class="page-header">
      <h1>Apply Leave</h1>
    </div>

    <mat-card>

      <form
        [formGroup]="form"
        (ngSubmit)="submit()">

        <mat-form-field appearance="outline">
          <mat-label>Leave Type</mat-label>

          <mat-select formControlName="leaveType">

            <mat-option value="Casual">
              Casual
            </mat-option>

            <mat-option value="Sick">
              Sick
            </mat-option>

            <mat-option value="Earned">
              Earned
            </mat-option>

          </mat-select>
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>From Date</mat-label>

          <input
            matInput
            type="date"
            formControlName="fromDate">
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>To Date</mat-label>

          <input
            matInput
            type="date"
            formControlName="toDate">
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Reason</mat-label>

          <textarea
            matInput
            rows="4"
            formControlName="reason">
          </textarea>
        </mat-form-field>

        <button
          mat-raised-button
          color="primary"
          [disabled]="form.invalid">

          Apply Leave

        </button>

      </form>

    </mat-card>
  `,
  styles:[`
    mat-card{
      padding:20px;
      max-width:700px;
    }

    form{
      display:flex;
      flex-direction:column;
      gap:16px;
    }

    .page-header{
      margin-bottom:20px;
    }
  `]
})
export class LeaveFormComponent {

  private fb = inject(FormBuilder);
  private service = inject(LeaveService);
  private auth = inject(AuthService);
  private router = inject(Router);

  form = this.fb.group({
    leaveType:['', Validators.required],
    fromDate:['', Validators.required],
    toDate:['', Validators.required],
    reason:['']
  });

  submit() {

    if (this.form.invalid)
      return;

    const empId =
      this.auth.currentUser()?.employeeId ?? 0;

    this.service
      .apply({
        empId,
        ...this.form.value
      })
      .subscribe(res => {

        if (res.isSuccess) {

          this.router.navigate([
            '/leaves/list'
          ]);

        }

      });

  }
}