import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class TokenStorageService {
  getAccessToken(): string | null { return localStorage.getItem('accessToken'); }
  setTokens(accessToken: string, refreshToken: string): void { localStorage.setItem('accessToken', accessToken); localStorage.setItem('refreshToken', refreshToken); }
  clear(): void { localStorage.removeItem('accessToken'); localStorage.removeItem('refreshToken'); }
}
