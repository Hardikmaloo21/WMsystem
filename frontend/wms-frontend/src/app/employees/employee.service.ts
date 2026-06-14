// ─────────────────────────────────────────────
// employee.service.ts
// frontend/src/app/employees/employee.service.ts
// ─────────────────────────────────────────────

import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class EmployeeService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/employees`;

  getAll(
    search = '', departmentId: any = null,
    roleId: any = null, status = '',
    pageNumber = 1, pageSize = 10
  ): Observable<any> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);
    if (search)       params = params.set('search', search);
    if (departmentId) params = params.set('departmentId', departmentId);
    if (roleId)       params = params.set('roleId', roleId);
    if (status)       params = params.set('status', status);
    return this.http.get<any>(this.baseUrl, { params });
  }

  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${id}`);
  }

  create(dto: any): Observable<any> {
    return this.http.post<any>(this.baseUrl, dto);
  }

  update(id: number, dto: any): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}/${id}`);
  }

  getDepartments(): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}/departments`);
  }
  getRoles() {
  return this.http.get<any>(
    `${environment.apiUrl}/roles`
  );
}

updateStatus(id: number, status: string) {
  return this.http.put(
    `${this.baseUrl}/${id}/status`,
    { status }
  );
}
}
