import { Injectable, inject } from '@angular/core';
import {
  HttpClient,
  HttpParams
} from '@angular/common/http';

import { environment }
from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AllocationService {

  private http =
    inject(HttpClient);

  private apiUrl =
    `${environment.apiUrl}/allocations`;

  getAll(
    empId: number | null = null,
    projectId: number | null = null,
    status: boolean | null = null,
    pageNumber = 1,
    pageSize = 10
  ) {

    let params =
      new HttpParams()
        .set(
          'pageNumber',
          pageNumber
        )
        .set(
          'pageSize',
          pageSize
        );

    if(empId){
      params =
        params.set(
          'empId',
          empId
        );
    }

    if(projectId){
      params =
        params.set(
          'projectId',
          projectId
        );
    }

    if(status !== null){
      params =
        params.set(
          'status',
          status
        );
    }

    return this.http.get<any>(
      this.apiUrl,
      { params }
    );
  }

  assign(dto:any){
    return this.http.post<any>(
      `${this.apiUrl}/assign`,
      dto
    );
  }

  update(
    id:number,
    dto:any
  ){
    return this.http.put<any>(
      `${this.apiUrl}/${id}`,
      dto
    );
  }

  delete(id:number){
    return this.http.delete<any>(
      `${this.apiUrl}/${id}`
    );
  }

  getById(id:number){
    return this.http.get<any>(
      `${this.apiUrl}/${id}`
    );
  }
}