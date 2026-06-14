import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { ProjectService } from '../project.service';
import { ClientService } from '../../clients/client.service';

@Component({
  selector: 'app-project-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  template: `
  <h2>
    {{ isEdit ? 'Edit Project' : 'Add Project' }}
  </h2>

  <form [formGroup]="form" (ngSubmit)="save()">

    <div>
      <label>Project Name</label>
      <input
        type="text"
        formControlName="projectName">
    </div>

    <div>
      <label>Client</label>

      <select formControlName="clientId">

        <option [ngValue]="null">
          Select Client
        </option>

        <option
          *ngFor="let c of clients"
          [ngValue]="c.clientId">

          {{ c.clientName }}

        </option>

      </select>
    </div>

    <div>
      <label>Start Date</label>
      <input
        type="date"
        formControlName="startDate">
    </div>

    <div>
      <label>End Date</label>
      <input
        type="date"
        formControlName="endDate">
    </div>

    <div *ngIf="isEdit">

      <label>Status</label>

      <select formControlName="status">
        <option value="Active">Active</option>
        <option value="Completed">Completed</option>
        <option value="OnHold">OnHold</option>
      </select>

    </div>

    <button type="submit">
      Save
    </button>

  </form>
  `
})
export class ProjectFormComponent implements OnInit {

  private fb = inject(FormBuilder);
  private service = inject(ProjectService);
  private clientService = inject(ClientService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  isEdit = false;

  clients: any[] = [];

  form = this.fb.group({
    projectName: ['', Validators.required],
    clientId: [null as number | null],
    startDate: [''],
    endDate: [''],
    status: ['Active']
  });

  ngOnInit(): void {

    this.loadClients();

    const id =
      this.route.snapshot.paramMap.get('id');

    if (id) {

      this.isEdit = true;

      this.service
        .getById(+id)
        .subscribe(res => {

          const p = res.data;

          this.form.patchValue({
            projectName: p.projectName,
            clientId: p.clientId,
            startDate: p.startDate?.substring(0,10),
            endDate: p.endDate?.substring(0,10),
            status: p.status
          });

        });
    }
  }

  loadClients() {

    this.clientService
      .getAll('',1,100)
      .subscribe(res => {

        this.clients =
          res.data.items;

      });
  }

  save() {

    if (this.form.invalid) {
      return;
    }

    const dto = this.form.getRawValue();

    const id =
      this.route.snapshot.paramMap.get('id');

    if (this.isEdit && id) {

      this.service
        .update(+id, dto)
        .subscribe({
          next: () =>
            this.router.navigate(['/projects/list'])
        });

    } else {

      this.service
        .create(dto)
        .subscribe({
          next: () =>
            this.router.navigate(['/projects/list'])
        });
    }
  }
}