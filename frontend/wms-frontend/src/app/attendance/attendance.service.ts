import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {

  private http = inject(HttpClient);

  private apiUrl =
    `${environment.apiUrl}/attendance`;

  getToday() {
    return this.http.get<any>(
      `${this.apiUrl}/today`
    );
  }

  getMonthly(
    employeeId: number,
    year: number,
    month: number
  ) {
    return this.http.get<any>(
      `${this.apiUrl}/employee/${employeeId}/monthly`,
      {
        params: {
          year,
          month
        }
      }
    );
  }

  checkIn(empId:number, workMode:string){
  return this.http.post<any>(
    `${this.apiUrl}/check-in`,
    {
      empId,
      workMode
    }
  );
}

checkOut(empId:number){
  return this.http.post<any>(
    `${this.apiUrl}/check-out`,
    {
      empId
    }
  );
}
getEmployeeAttendance(
  employeeId:number,
  year:number,
  month:number
){
  return this.getMonthly(
    employeeId,
    year,
    month
  );
}
}