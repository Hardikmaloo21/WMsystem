import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { EmployeeService } from '../../employees/employee.service';
import { ProjectService } from '../../projects/project.service';
import { AllocationService } from '../allocation.service';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from '../../core/services/notification.service';

@Component({
  selector: 'app-allocation-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './allocation-form.component.html'
})
export class AllocationFormComponent implements OnInit {

  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private employeeService = inject(EmployeeService);
  private projectService = inject(ProjectService);
  private allocationService = inject(AllocationService);
  private notify = inject(NotificationService);

  employees:any[] = [];
  projects:any[] = [];

  allocationId = 0;
isEdit = false;

  form = this.fb.group({
    empId:[null, Validators.required],
    projectId:[null, Validators.required],
    assignedOn:[null, Validators.required]
  });

ngOnInit(): void {

  this.employeeService
    .getAll('', null, null, '', 1, 100)
    .subscribe(res => {

     

      this.employees =
        res.data.items;

      
    });

  this.projectService
    .getAll('', '', null, 1, 100)
    .subscribe(res => {

     

      this.projects =
        res.data.items;

      
    });

    const id =
  Number(
    this.route.snapshot.paramMap.get('id')
  );

if(id){

  this.isEdit = true;
  this.allocationId = id;

  this.allocationService
    .getById(id)
    .subscribe(res => {

      const allocation =
        res.data;

      this.form.patchValue({

        empId:
          allocation.empId,

        projectId:
          allocation.projectId,

        assignedOn:
          allocation.assignedOn
      });

    });
}
}
  save() {

  if (this.form.invalid) {

    this.form.markAllAsTouched();

    this.notify.error(
      'Please fill all required fields.'
    );

    return;
  }

  if (this.isEdit) {

    this.allocationService
      .update(
        this.allocationId,
        {
          allocationId: this.allocationId,
          status: true,
          updatedBy: 'admin'
        }
      )
      .subscribe({
        next: () => {

          this.notify.success(
            'Allocation updated successfully.'
          );

          this.router.navigate([
            '/allocations/list'
          ]);
        },

        error: (err) => {

          this.notify.error(
            err?.error?.message ??
            'Failed to update allocation.'
          );
        }
      });

    return;
  }

  this.allocationService
    .assign(this.form.value)
    .subscribe({

      next: () => {

        this.notify.success(
          'Employee assigned successfully.'
        );

        this.router.navigate([
          '/allocations/list'
        ]);
      },

      error: (err) => {

        this.notify.error(
          err?.error?.message ??
          'Failed to assign employee.'
        );
      }
    });
}
}