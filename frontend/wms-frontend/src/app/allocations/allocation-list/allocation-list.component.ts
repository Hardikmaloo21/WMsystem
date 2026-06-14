import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AllocationService } from '../allocation.service';

import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-allocation-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    FormsModule,
    MatTableModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatTooltipModule
  ],
  templateUrl: './allocation-list.component.html',
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

    .filters{
      display:flex;
      gap:12px;
      margin-bottom:20px;
      align-items:center;
    }

    .pagination{
      display:flex;
      gap:10px;
      align-items:center;
      margin-top:20px;
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
export class AllocationListComponent
implements OnInit {

  private allocationService =
    inject(AllocationService);

  allocations = signal<any[]>([]);

  columns = [
    'employee',
    'department',
    'project',
    'assignedOn',
    'status',
    'actions'
  ];

  status: boolean | null = null;

  pageNumber = 1;

  pageSize = 10;

  totalCount = 0;

  ngOnInit(): void {
    this.load();
  }

  load() {

    this.allocationService
      .getAll(
        null,
        null,
        this.status,
        this.pageNumber,
        this.pageSize
      )
      .subscribe({
        next: (res) => {

          this.allocations.set(
            res.data.items
          );

          this.totalCount =
            res.data.pagination?.totalCount ?? 0;
        }
      });
  }

  filter() {

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

  delete(id:number){

    if(!confirm(
      'Remove allocation?'
    )) return;

    this.allocationService
      .delete(id)
      .subscribe({
        next: () => {

          if(
            this.allocations().length === 1 &&
            this.pageNumber > 1
          ){
            this.pageNumber--;
          }

          this.load();
        }
      });
  }
}