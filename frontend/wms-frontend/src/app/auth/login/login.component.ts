// frontend/src/app/auth/login/login.component.ts
import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../../core/services/auth.service';
import { signal } from '@angular/core';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, RouterLink,
    MatCardModule, MatFormFieldModule, MatInputModule,
    MatButtonModule, MatIconModule, MatProgressSpinnerModule
  ],
  template: `
    <div class="login-container">
      <mat-card class="login-card">
        <mat-card-header>
          <div class="login-logo">
            <mat-icon>business_center</mat-icon>
            <h1>WMS</h1>
            <p>Workforce Management System</p>
          </div>
        </mat-card-header>

        <mat-card-content>
          <form [formGroup]="loginForm" (ngSubmit)="onSubmit()">

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Username</mat-label>
              <input matInput formControlName="username" autocomplete="username" />
              <mat-icon matSuffix>person</mat-icon>
              @if (loginForm.get('username')?.hasError('required') &&
                   loginForm.get('username')?.touched) {
                <mat-error>Username is required.</mat-error>
              }
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Password</mat-label>
              <input matInput [type]="hidePassword() ? 'password' : 'text'"
                     formControlName="password" autocomplete="current-password" />
              <button mat-icon-button matSuffix type="button"
                      (click)="hidePassword.set(!hidePassword())">
                <mat-icon>{{ hidePassword() ? 'visibility_off' : 'visibility' }}</mat-icon>
              </button>
              @if (loginForm.get('password')?.hasError('required') &&
                   loginForm.get('password')?.touched) {
                <mat-error>Password is required.</mat-error>
              }
            </mat-form-field>

            @if (errorMessage()) {
              <div class="error-banner">{{ errorMessage() }}</div>
            }

            <button mat-raised-button color="primary" type="submit"
                    class="full-width login-btn"
                    [disabled]="loginForm.invalid || loading()">
              @if (loading()) {
                <mat-spinner diameter="20"></mat-spinner>
              } @else {
                Sign In
              }
            </button>

          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .login-container {
      min-height: 100vh;
      display: flex;
      align-items: center;
      justify-content: center;
      background: linear-gradient(135deg, #1a237e 0%, #283593 50%, #1565c0 100%);
    }
    .login-card { width: 420px; padding: 24px; border-radius: 16px; }
    .login-logo { text-align: center; padding: 16px 0; }
    .login-logo mat-icon { font-size: 48px; height: 48px; width: 48px; color: #1565c0; }
    .login-logo h1 { margin: 8px 0 4px; font-size: 28px; font-weight: 700; color: #1a237e; }
    .login-logo p { margin: 0; color: #666; font-size: 13px; }
    .full-width { width: 100%; margin-bottom: 16px; }
    .login-btn { height: 48px; font-size: 16px; font-weight: 600; margin-top: 8px; }
    .error-banner {
      background: #ffebee; color: #c62828; padding: 12px 16px;
      border-radius: 8px; margin-bottom: 16px; font-size: 14px;
    }
  `]
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  loading = signal(false);
  hidePassword = signal(true);
  errorMessage = signal('');

  loginForm = this.fb.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  onSubmit(): void {
    if (this.loginForm.invalid) return;
    this.loading.set(true);
    this.errorMessage.set('');

    const { username, password } = this.loginForm.value;

    this.authService.login({ username: username!, password: password! }).subscribe({
      next: (res) => {
        this.loading.set(false);
        if (res.isSuccess) {
          const role = res.data.role;

if (role === 'Employee') {

  this.router.navigate(['/employee-dashboard']);

} else {

  this.router.navigate(['/dashboard']);

}
        }
      },
      error: (err) => {
        this.loading.set(false);
        this.errorMessage.set(err?.error?.message ?? 'Login failed. Please try again.');
      }
    });
  }
}