import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ProjectService } from '../project.service';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FormsModule } from '@angular/forms';
import { ConfirmDialogComponent }
from '../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-project-list',
  standalone: true,
  imports: [
  CommonModule,
  RouterModule,
  MatTableModule,
  MatCardModule,
  MatButtonModule,
  MatIconModule,
  MatChipsModule,
  MatDialogModule,
  MatTooltipModule,
  FormsModule
],
  template: `
  <div class="page-header">
    <h2>Projects</h2>

    <button
      class="add-btn"
      routerLink="/projects/add">
      + Add Project
    </button>
  </div>

  <div style="margin-bottom:20px;">

  <input
  type="text"
  [(ngModel)]="search"
  (keyup.enter)="searchProjects()"
  placeholder="Search project">

  <button
    (click)="searchProjects()">

    Search

  </button>

</div>

  <table *ngIf="projects.length">
    <thead>
      <tr>
        <th>Name</th>
        <th>Client</th>
        <th>Status</th>
        <th>Employees</th>
        <th>Start</th>
        <th>End</th>
        <th>Actions</th>
      </tr>
    </thead>

    <tbody>
      <tr *ngFor="let p of projects">

        <td>{{ p.projectName }}</td>

        <td>{{ p.clientName }}</td>

        <td>

  <mat-chip
    [class]="
      p.status === 'Active'
        ? 'active-chip'
        : 'inactive-chip'
    ">

    {{ p.status }}

  </mat-chip>

</td>

        <td>{{ p.allocatedEmployeeCount }}</td>

        <td>{{ p.startDate | date:'dd-MM-yyyy' }}</td>

        <td>{{ p.endDate | date:'dd-MM-yyyy' }}</td>

        <td>
          <button
  mat-icon-button
  color="primary"
  matTooltip="Edit Project"
  (click)="edit(p.projectId)">

  <mat-icon>edit</mat-icon>

</button>

<button
  mat-icon-button
  color="warn"
  matTooltip="Delete Project"
  (click)="delete(p.projectId)">

  <mat-icon>delete</mat-icon>

</button>
        </td>

      </tr>
    </tbody>
  </table>

  <div
  style="
    margin-top:20px;
    display:flex;
    gap:10px;
    align-items:center;
  ">

  <button
    (click)="prevPage()">

    Previous

  </button>

  <span>

    Page {{pageNumber}}

  </span>

  <button
    (click)="nextPage()">

    Next

  </button>

  <span>

    Total:
    {{totalCount}}

  </span>

</div>
  <p *ngIf="!projects.length">
    No projects found.
  </p>
  `,
  styles: [`
    .page-header{
      display:flex;
      justify-content:space-between;
      margin-bottom:20px;
    }

    table{
      width:100%;
      border-collapse:collapse;
    }

    th,td{
      padding:10px;
      border-bottom:1px solid #ddd;
      text-align:left;
    }

    button{
      margin-right:6px;
      cursor:pointer;
    }

    .active-chip{
  background:#e8f5e9 !important;
  color:#2e7d32 !important;
}

.inactive-chip{
  background:#ffebee !important;
  color:#c62828 !important;
}
  `]
})
export class ProjectListComponent implements OnInit {

  private service = inject(ProjectService);
  private router = inject(Router);
  private dialog =
  inject(MatDialog);
  projects: any[] = [];

search = '';
pageNumber = 1;
pageSize = 10;

totalCount = 0;

  ngOnInit(): void {
    this.load();
  }

  load() {

  this.service
    .getAll(
      this.search,
      '',
      null,
      this.pageNumber,
      this.pageSize
    )
    .subscribe(res => {

      this.projects =
        res.data.items;

      this.totalCount =
  res.data.pagination?.totalCount ?? 0;
    });
}

  edit(id: number) {
    this.router.navigate([
      '/projects/edit',
      id
    ]);
  }

  searchProjects() {

  this.pageNumber = 1;

  this.load();
}

nextPage() {

  const totalPages =
    Math.ceil(
      this.totalCount /
      this.pageSize
    );

  if(this.pageNumber < totalPages){

    this.pageNumber++;

    this.load();
  }
}

prevPage() {

  if(this.pageNumber > 1){

    this.pageNumber--;

    this.load();
  }
}

  delete(id: number) {

  const ref =
    this.dialog.open(
      ConfirmDialogComponent,
      {
        data: {
          title: 'Delete Project',
          message:
            'Are you sure you want to delete this project?',
          confirmText: 'Delete'
        }
      }
    );

  ref.afterClosed()
    .subscribe(result => {

      if (!result) return;

      this.service
        .delete(id)
        .subscribe({
          next: () => {

  if (
    this.projects.length === 1 &&
    this.pageNumber > 1
  ) {
    this.pageNumber--;
  }

  this.load();
}
        });
    });
}
}