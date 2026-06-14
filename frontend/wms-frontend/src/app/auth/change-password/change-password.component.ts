import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { HttpClient } from '@angular/common/http';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';


import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  template: `
    <div class="page-header">
      <h1>Change Password</h1>
    </div>

    <mat-card>

      <form
        [formGroup]="form"
        (ngSubmit)="submit()">

        <mat-form-field appearance="outline">

          <mat-label>
            Current Password
          </mat-label>

          <input
            matInput
            type="password"
            formControlName="currentPassword">

        </mat-form-field>

        <mat-form-field appearance="outline">

          <mat-label>
            New Password
          </mat-label>

          <input
            matInput
            type="password"
            formControlName="newPassword">

        </mat-form-field>

        <mat-form-field appearance="outline">

          <mat-label>
            Confirm Password
          </mat-label>

          <input
            matInput
            type="password"
            formControlName="confirmPassword">

        </mat-form-field>

        <button
          mat-raised-button
          color="primary">

          Change Password

        </button>

      </form>

      <div
        *ngIf="message"
        class="success">

        {{ message }}

      </div>
      <div
  *ngIf="errorMessage"
  class="error">

  {{ errorMessage }}

</div>

    </mat-card>
  `,
  styles: [`
    .page-header{
      margin-bottom:20px;
    }

    mat-card{
      max-width:600px;
      padding:20px;
    }

    form{
      display:flex;
      flex-direction:column;
      gap:16px;
    }

    .success{
      margin-top:20px;
      color:green;
      font-weight:600;
    }
    .error{
  margin-top:20px;
  color:#c62828;
  font-weight:600;
}

.success{
  margin-top:20px;
  color:#2e7d32;
  font-weight:600;
}
  `]
})
export class ChangePasswordComponent {

  private fb = inject(FormBuilder);
  private http = inject(HttpClient);

  message = '';
  errorMessage = 'Passwords do not match';

  form = this.fb.group({
    currentPassword: ['', Validators.required],
    newPassword: [
      '',
      [
        Validators.required,
        Validators.minLength(6)
      ]
    ],
    confirmPassword: ['', Validators.required]
  });

  submit() {

    if (this.form.invalid)
      return;

    const value = this.form.getRawValue();

    if (
      value.newPassword !==
      value.confirmPassword
    ) {

      this.message = '';
      this.errorMessage = 'Passwords do not match';

      return;
    }

    this.http.post<any>(
      `${environment.apiUrl}/auth/change-password`,
      {
        currentPassword:
          value.currentPassword,

        newPassword:
          value.newPassword
      }
    )
    .subscribe({
      next: () => {

  this.errorMessage = '';

  this.message =
    'Password changed successfully';

  this.form.reset();

}
    });

  }
}