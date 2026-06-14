import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  private http = inject(HttpClient);

  private apiUrl =
    `${environment.apiUrl}/projects`;

  getAll(
    search = '',
    status = '',
    clientId: number | null = null,
    pageNumber = 1,
    pageSize = 10
  ) {

    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (search) {
      params = params.set('search', search);
    }

    if (status) {
      params = params.set('status', status);
    }

    if (clientId) {
      params = params.set(
        'clientId',
        clientId
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

  delete(id: number) {
    return this.http.delete<any>(
      `${this.apiUrl}/${id}`
    );
  }
}