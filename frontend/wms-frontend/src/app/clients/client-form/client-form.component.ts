import {
  Component,
  OnInit,
  inject
} from '@angular/core';

import { CommonModule } from '@angular/common';

import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  ActivatedRoute,
  Router
} from '@angular/router';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

import { ClientService } from '../client.service';

@Component({
  selector: 'app-client-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  template: `
    <h1>
      {{ isEdit ? 'Edit Client' : 'Add Client' }}
    </h1>

    <mat-card>

      <form
        [formGroup]="form"
        (ngSubmit)="save()">

        <mat-form-field appearance="outline">
          <mat-label>Client Name</mat-label>
          <input
            matInput
            formControlName="clientName">
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Address</mat-label>
          <input
            matInput
            formControlName="clientAddress">
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Phone</mat-label>
          <input
            matInput
            formControlName="clientPhoneNumber">
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Location</mat-label>
          <input
            matInput
            formControlName="clientLocation">
        </mat-form-field>

        <button
          mat-raised-button
          color="primary">

          Save

        </button>

      </form>

    </mat-card>
  `,
  styles:[`
    form{
      display:flex;
      flex-direction:column;
      gap:16px;
    }
  `]
})
export class ClientFormComponent
implements OnInit {

  private fb =
    inject(FormBuilder);

  private service =
    inject(ClientService);

  private route =
    inject(ActivatedRoute);

  private router =
    inject(Router);

  isEdit = false;

  clientId = 0;

  form = this.fb.group({
    clientName:['',Validators.required],
    clientAddress:[''],
    clientPhoneNumber:[''],
    clientLocation:['']
  });

  ngOnInit(): void {

    const id =
      this.route.snapshot.paramMap.get('id');

    if(id){

      this.isEdit = true;

      this.clientId = +id;

      this.service
        .getById(this.clientId)
        .subscribe(res => {

          if(res.isSuccess){

            this.form.patchValue(
              res.data
            );

          }

        });

    }

  }

  save(){

    if(this.form.invalid)
      return;

    const dto =
      this.form.getRawValue();

    if(this.isEdit){

      this.service
        .update(
          this.clientId,
          dto
        )
        .subscribe(() => {

          this.router.navigate(
            ['/clients/list']
          );

        });

      return;
    }

    this.service
      .create(dto)
      .subscribe(() => {

        this.router.navigate(
          ['/clients/list']
        );

      });

  }
}