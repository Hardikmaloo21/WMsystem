import {
  Component,
  OnInit,
  inject,
  signal
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { ClientService } from '../client.service';
import { MatDialog } from '@angular/material/dialog';
import { MatDialogModule } from '@angular/material/dialog';

import { ConfirmDialogComponent }
from '../../shared/components/confirm-dialog/confirm-dialog.component';

import { MatTooltipModule }
from '@angular/material/tooltip';

@Component({
  selector: 'app-client-list',
  standalone: true,
  imports: [
    MatTooltipModule,
    MatDialogModule,
    CommonModule,
    RouterLink,
    MatTableModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    FormsModule
  ],
  template: `
    <div class="page-header">

      <h1>Clients</h1>
    <div style="margin-bottom:20px;">

  <input
    type="text"
    [(ngModel)]="search"
    (keyup.enter)="searchClients()"
    placeholder="Search client">

  <button
    mat-raised-button
    color="primary"
    (click)="searchClients()">

    Search

  </button>

</div>
      <button
        mat-raised-button
        color="primary"
        routerLink="/clients/add">

        <mat-icon>add</mat-icon>
        Add Client

      </button>

    </div>

    <mat-card>

      <table
        mat-table
        [dataSource]="clients()">

        <ng-container matColumnDef="name">
          <th mat-header-cell *matHeaderCellDef>
            Client Name
          </th>

          <td mat-cell *matCellDef="let c">
            {{ c.clientName }}
          </td>
        </ng-container>

        <ng-container matColumnDef="phone">
          <th mat-header-cell *matHeaderCellDef>
            Phone
          </th>

          <td mat-cell *matCellDef="let c">
            {{ c.clientPhoneNumber || '-' }}
          </td>
        </ng-container>

        <ng-container matColumnDef="location">
          <th mat-header-cell *matHeaderCellDef>
            Location
          </th>

          <td mat-cell *matCellDef="let c">
            {{ c.clientLocation || '-' }}
          </td>
        </ng-container>

        <ng-container matColumnDef="status">
          <th mat-header-cell *matHeaderCellDef>
            Status
          </th>

          <td mat-cell *matCellDef="let c">
            {{ c.status ? 'Active' : 'Inactive' }}
          </td>
        </ng-container>

        <ng-container matColumnDef="actions">
          <th mat-header-cell *matHeaderCellDef>
            Actions
          </th>

          <td mat-cell *matCellDef="let c">

            <button
  mat-icon-button
  color="primary"
  matTooltip="Edit Client"
  [routerLink]="[
    '/clients/edit',
    c.clientId
  ]">

  <mat-icon>edit</mat-icon>

</button>

<button
  mat-icon-button
  color="warn"
  matTooltip="Delete Client"
  (click)="delete(c.clientId)">

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
export class ClientListComponent
implements OnInit {

  private service =
    inject(ClientService);

  private dialog =
    inject(MatDialog);

  clients = signal<any[]>([]);

  search = '';

  pageNumber = 1;

  pageSize = 10;

  totalCount = 0;

  columns = [
    'name',
    'phone',
    'location',
    'status',
    'actions'
  ];

  ngOnInit(): void {
    this.load();
  }

  load() {

    this.service
      .getAll(
        this.search,
        this.pageNumber,
        this.pageSize
      )
      .subscribe(res => {

        if(res.isSuccess){

          this.clients.set(
            res.data.items
          );

          this.totalCount =
            res.data.pagination?.totalCount ?? 0;
        }

      });

  }

  searchClients() {

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
            title: 'Delete Client',
            message:
              'Are you sure you want to delete this client?',
            confirmText: 'Delete'
          }
        }
      );

    ref.afterClosed()
      .subscribe(result => {

        if (!result) return;

        this.service
          .delete(id)
          .subscribe(() => {

            if(
              this.clients().length === 1 &&
              this.pageNumber > 1
            ){
              this.pageNumber--;
            }

            this.load();

          });

      });
  }
}