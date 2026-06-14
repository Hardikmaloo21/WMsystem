import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  private http = inject(HttpClient);

  private apiUrl =
    `${environment.apiUrl}/departments`;

  getAll() {
    return this.http.get<any>(this.apiUrl);
  }

  getById(id: number) {
    return this.http.get<any>(
      `${this.apiUrl}/${id}`
    );
  }

  create(dto: any) {
    return this.http.post<any>(
      this.apiUrl,
      dto
    );
  }

  update(id: number, dto: any) {
    return this.http.put<any>(
      `${this.apiUrl}/${id}`,
      dto
    );
  }

  delete(id: number) {
    return this.http.delete<any>(
      `${this.apiUrl}/${id}`
    );
  }
}