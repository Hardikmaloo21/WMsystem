import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../core/models/api-response.model';

export interface LeaveDto {
  leaveId: number;
  employeeId?: number;
  employeeName?: string;
  leaveType?: string;
  fromDate?: string;
  toDate?: string;
  totalDays?: number;
  reason?: string;
  status: string;
}

export interface Pagination<T> {
  items: T;
  totalCount: number;
}

export interface LeaveApproveRejectDto {
  leaveId: number;
  approverId: number;
  action: 'Approve' | 'Reject' | string;
}

@Injectable({ providedIn: 'root' })
export class LeaveService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/leaves`;

  getByEmployee(employeeId: number, page: number): Observable<ApiResponse<Pagination<LeaveDto[]>>> {
    const params = new HttpParams().set('page', page);
    return this.http.get<ApiResponse<Pagination<LeaveDto[]>>>(`${this.apiUrl}/employee/${employeeId}`, { params });
  }

  getPending(): Observable<ApiResponse<LeaveDto[]>> {
    return this.http.get<ApiResponse<LeaveDto[]>>(`${this.apiUrl}/pending`);
  }

  cancel(leaveId: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${leaveId}/cancel`, {});
  }

  approveReject(payload: LeaveApproveRejectDto): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/approve-reject`, payload);
  }
  apply(dto:any){
  return this.http.post<any>(
    `${this.apiUrl}/apply`,
    dto
  );
}


}

