import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { DepartmentService } from '../department.service';

@Component({
  selector: 'app-department-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatTableModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule
  ],
  template: `
    <div class="page-header">

      <h1>Departments</h1>

      <button
        mat-raised-button
        color="primary"
        routerLink="/departments/add">

        <mat-icon>add</mat-icon>
        Add Department

      </button>

    </div>

    <mat-card>

      <table
        mat-table
        [dataSource]="departments()">

        <ng-container matColumnDef="name">
          <th mat-header-cell *matHeaderCellDef>
            Department
          </th>

          <td mat-cell *matCellDef="let d">
            {{ d.departmentName }}
          </td>
        </ng-container>

        <ng-container matColumnDef="description">
          <th mat-header-cell *matHeaderCellDef>
            Description
          </th>

          <td mat-cell *matCellDef="let d">
            {{ d.description || '-' }}
          </td>
        </ng-container>

        <ng-container matColumnDef="employees">
          <th mat-header-cell *matHeaderCellDef>
            Employees
          </th>

          <td mat-cell *matCellDef="let d">
            {{ d.employeeCount }}
          </td>
        </ng-container>

        <ng-container matColumnDef="actions">
          <th mat-header-cell *matHeaderCellDef>
            Actions
          </th>

          <td mat-cell *matCellDef="let d">

            <button
              mat-icon-button
              color="primary"
              [routerLink]="[
                '/departments/edit',
                d.departmentId
              ]">

              <mat-icon>edit</mat-icon>

            </button>

            <button
              mat-icon-button
              color="warn"
              (click)="delete(d.departmentId)">

              <mat-icon>delete</mat-icon>

            </button>

          </td>
        </ng-container>

        <tr
          mat-header-row
          *matHeaderRowDef="columns">
        </tr>

        <tr
          mat-row
          *matRowDef="
            let row;
            columns: columns
          ">
        </tr>

      </table>

    </mat-card>
  `,
  styles: [`
    .page-header{
      display:flex;
      justify-content:space-between;
      align-items:center;
      margin-bottom:20px;
    }

    table{
      width:100%;
    }
  `]
})
export class DepartmentListComponent
implements OnInit {

  private service =
    inject(DepartmentService);

  departments = signal<any[]>([]);

  columns = [
    'name',
    'description',
    'employees',
    'actions'
  ];

  ngOnInit(): void {
    this.load();
  }

  load() {

    this.service
      .getAll()
      .subscribe(res => {

        if(res.isSuccess){
          this.departments.set(
            res.data
          );
        }

      });
  }

  delete(id:number){

    if(!confirm(
      'Delete department?'
    )) return;

    this.service
      .delete(id)
      .subscribe(() => {

        this.load();

      });
  }
}