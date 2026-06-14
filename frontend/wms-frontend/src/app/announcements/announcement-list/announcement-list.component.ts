import {
  Component,
  OnInit,
  inject
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AnnouncementService } from '../announcement.service';

@Component({
  selector: 'app-announcement-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    FormsModule
  ],
  template: `

  <div class="page-header">

    <h2>Announcements</h2>

    <a
      routerLink="/announcements/add">

      Add Announcement

    </a>

  </div>

  <div class="search-bar">

    <select
      [(ngModel)]="isActive"
      (change)="searchAnnouncements()">

      <option [ngValue]="null">
        All
      </option>

      <option [ngValue]="true">
        Active
      </option>

      <option [ngValue]="false">
        Inactive
      </option>

    </select>

  </div>

  <table *ngIf="announcements.length">

    <thead>

      <tr>

        <th>Title</th>

        <th>Message</th>

        <th>Status</th>

        <th>Created</th>

        <th>Actions</th>

      </tr>

    </thead>

    <tbody>

      <tr
        *ngFor="let a of announcements">

        <td>
          {{ a.title }}
        </td>

        <td>
          {{ a.message }}
        </td>

        <td>
          {{ a.isActive ? 'Active' : 'Inactive' }}
        </td>

        <td>
          {{ a.createdOn | date:'dd-MM-yyyy' }}
        </td>

        <td>

          <button
            [routerLink]="[
              '/announcements/edit',
              a.announcementId
            ]">

            Edit

          </button>

          <button
            (click)="toggle(a.announcementId)">

            {{ a.isActive
              ? 'Deactivate'
              : 'Activate'
            }}

          </button>

          <button
            (click)="delete(a.announcementId)">

            Delete

          </button>

        </td>

      </tr>

    </tbody>

  </table>

  <div
    class="pagination">

    <button
      (click)="prevPage()">

      Previous

    </button>

    <span>
      Page {{ pageNumber }}
    </span>

    <button
      (click)="nextPage()">

      Next

    </button>

    <span>
      Total:
      {{ totalCount }}
    </span>

  </div>

  <p *ngIf="!announcements.length">

    No announcements found.

  </p>

  `,
  styles: [`

    .page-header{
      display:flex;
      justify-content:space-between;
      align-items:center;
      margin-bottom:20px;
    }

    .search-bar{
      margin-bottom:20px;
    }

    table{
      width:100%;
      border-collapse:collapse;
    }

    th,
    td{
      padding:10px;
      border-bottom:1px solid #ddd;
      text-align:left;
    }

    .pagination{
      margin-top:20px;
      display:flex;
      gap:10px;
      align-items:center;
    }

  `]
})
export class AnnouncementListComponent
implements OnInit {

  private service =
    inject(AnnouncementService);

  announcements:any[] = [];

  isActive:
    boolean | null = null;

  pageNumber = 1;

  pageSize = 10;

  totalCount = 0;

  ngOnInit(): void {
    this.load();
  }

  load(){

    this.service
      .getAll(
        this.isActive,
        this.pageNumber,
        this.pageSize
      )
      .subscribe(res => {

        this.announcements =
          res.data.items;

        this.totalCount =
          res.data.pagination?.totalCount ?? 0;

      });
  }

  searchAnnouncements(){

    this.pageNumber = 1;

    this.load();
  }

  nextPage(){

    const totalPages =
      Math.ceil(
        this.totalCount /
        this.pageSize
      );

    if(
      this.pageNumber <
      totalPages
    ){

      this.pageNumber++;

      this.load();
    }
  }

  prevPage(){

    if(
      this.pageNumber > 1
    ){

      this.pageNumber--;

      this.load();
    }
  }

  toggle(id:number){

    this.service
      .toggle(id)
      .subscribe(() => {

        this.load();

      });
  }

  delete(id:number){

    if(
      !confirm(
        'Delete announcement?'
      )
    ) return;

    this.service
      .delete(id)
      .subscribe(() => {

        if(
          this.announcements.length === 1 &&
          this.pageNumber > 1
        ){
          this.pageNumber--;
        }

        this.load();

      });
  }
}