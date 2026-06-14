import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AnnouncementService {

  private http = inject(HttpClient);

  private apiUrl =
    `${environment.apiUrl}/announcements`;

  getAll(
    isActive: boolean | null = null,
    pageNumber = 1,
    pageSize = 10
  ) {

    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (isActive !== null) {
      params = params.set(
        'isActive',
        isActive
      );
    }

    return this.http.get<any>(
      this.apiUrl,
      { params }
    );
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

  toggle(id: number) {
    return this.http.patch<any>(
      `${this.apiUrl}/${id}/toggle`,
      {}
    );
  }

  delete(id: number) {
    return this.http.delete<any>(
      `${this.apiUrl}/${id}`
    );
  }
}