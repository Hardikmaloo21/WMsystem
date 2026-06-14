// frontend/src/app/core/services/auth.service.ts
import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap, catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface LoginRequest {
  username: string;
  password: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  username: string;
  role: string;
  employeeId: number | null;
}

export interface ApiResponse<T> {
  isSuccess: boolean;
  statusCode: number;
  message: string;
  data: T;
  errors: any;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
private readonly apiUrl = `${environment.apiUrl}/auth`;

  // Signals for reactive state
  private _currentUser = signal<AuthResponse | null>(this.loadFromStorage());
  readonly currentUser = this._currentUser.asReadonly();
  readonly isAuthenticated = computed(() => !!this._currentUser());
  readonly userRole = computed(() => this._currentUser()?.role ?? '');
  readonly isAdmin = computed(() => this.userRole() === 'Admin');
  readonly isManager = computed(() => ['Admin', 'Manager'].includes(this.userRole()));

  constructor(private http: HttpClient, private router: Router) {}

  login(credentials: LoginRequest): Observable<ApiResponse<AuthResponse>> {
    return this.http.post<ApiResponse<AuthResponse>>(`${this.apiUrl}/login`, credentials).pipe(
      tap(res => {
        if (res.isSuccess) {
          this.storeTokens(res.data);
          this._currentUser.set(res.data);
        }
      })
    );
  }

  logout(): Observable<any> {
    return this.http.post(`${this.apiUrl}/logout`, {}).pipe(
      tap(() => this.clearSession()),
      catchError(err => {
        this.clearSession();
        return throwError(() => err);
      })
    );
  }

  refreshToken(): Observable<ApiResponse<AuthResponse>> {
    const accessToken = localStorage.getItem('accessToken') ?? '';
    const refreshToken = localStorage.getItem('refreshToken') ?? '';

    return this.http.post<ApiResponse<AuthResponse>>(
      `${this.apiUrl}/refresh-token`,
      { accessToken, refreshToken }
    ).pipe(
      tap(res => {
        if (res.isSuccess) {
          this.storeTokens(res.data);
          this._currentUser.set(res.data);
        }
      })
    );
  }

  getAccessToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  private storeTokens(auth: AuthResponse): void {
    localStorage.setItem('accessToken', auth.accessToken);
    localStorage.setItem('refreshToken', auth.refreshToken);
    localStorage.setItem('currentUser', JSON.stringify(auth));
  }

  private clearSession(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('currentUser');
    this._currentUser.set(null);
    this.router.navigate(['/auth/login']);
  }

  private loadFromStorage(): AuthResponse | null {
    try {
      const data = localStorage.getItem('currentUser');
      return data ? JSON.parse(data) : null;
    } catch {
      return null;
    }
  }
}