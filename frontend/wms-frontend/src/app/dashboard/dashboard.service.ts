import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

import { ApiResponse } from '../core/models/api-response.model';
import { DashboardSummaryDto } from './dashboard.model';


@Injectable({ providedIn: 'root' })
export class DashboardService {
  private readonly apiUrl = `${environment.apiUrl}/dashboard`;

  constructor(private http: HttpClient) {}

  getSummary(): Observable<ApiResponse<DashboardSummaryDto>> {
    return this.http.get<ApiResponse<DashboardSummaryDto>>(`${this.apiUrl}/summary`);
  }
}

