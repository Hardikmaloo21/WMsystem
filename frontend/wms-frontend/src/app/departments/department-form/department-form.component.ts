import {
  Component,
  OnInit,
  inject
} from '@angular/core';

import {
  ReactiveFormsModule,
  FormBuilder,
  Validators
} from '@angular/forms';

import {
  ActivatedRoute,
  Router
} from '@angular/router';

import { CommonModule } from '@angular/common';

import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

import { DepartmentService } from '../department.service';

@Component({
  selector: 'app-department-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule
  ],
  template: `
    <mat-card>

      <h2>
        {{ isEdit
          ? 'Edit Department'
          : 'Add Department' }}
      </h2>

      <form
        [formGroup]="form"
        (ngSubmit)="save()">

        <mat-form-field
          appearance="outline">

          <mat-label>
            Department Name
          </mat-label>

          <input
            matInput
            formControlName="departmentName">
        </mat-form-field>

        <mat-form-field
          appearance="outline">

          <mat-label>
            Description
          </mat-label>

          <textarea
            matInput
            rows="4"
            formControlName="description">
          </textarea>

        </mat-form-field>

        <button
          mat-raised-button
          color="primary">

          Save

        </button>

      </form>

    </mat-card>
  `
})
export class DepartmentFormComponent
implements OnInit {

  private fb = inject(FormBuilder);
  private service =
    inject(DepartmentService);

  private route =
    inject(ActivatedRoute);

  private router =
    inject(Router);

  isEdit = false;

  departmentId = 0;

  form = this.fb.group({
    departmentName: [
      '',
      Validators.required
    ],
    description: ['']
  });

  ngOnInit(): void {

    const id =
      Number(
        this.route.snapshot
        .paramMap.get('id')
      );

    if(id){

      this.isEdit = true;
      this.departmentId = id;

      this.service
        .getById(id)
        .subscribe(res => {

          this.form.patchValue(
            res.data
          );

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
          this.departmentId,
          {
            departmentId:
              this.departmentId,
            ...dto
          }
        )
        .subscribe(() => {

          this.router.navigate([
            '/departments'
          ]);

        });

    } else {

      this.service
        .create(dto)
        .subscribe(() => {

          this.router.navigate([
            '/departments'
          ]);

        });

    }
  }
}