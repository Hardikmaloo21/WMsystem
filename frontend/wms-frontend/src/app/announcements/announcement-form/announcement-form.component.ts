import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import {
  ActivatedRoute,
  Router
} from '@angular/router';

import { AnnouncementService }
from '../announcement.service';
import { NotificationService } from '../../core/services/notification.service';

@Component({
  selector: 'app-announcement-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './announcement-form.component.html'
})
export class AnnouncementFormComponent
implements OnInit {

  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
private notify = inject(NotificationService);
  private service =
    inject(AnnouncementService);

  announcementId = 0;
  isEdit = false;

  form = this.fb.group({
    title: ['', Validators.required],
    message: ['', Validators.required]
  });

  ngOnInit(): void {

    const id =
      Number(
        this.route.snapshot.paramMap.get('id')
      );

    if(id){

      this.isEdit = true;
      this.announcementId = id;

      this.service
        .getById(id)
        .subscribe(res => {

          const a = res.data;

          this.form.patchValue({
            title: a.title,
            message: a.message
          });

        });
    }
  }

  save(){

    if(this.form.invalid){
      this.form.markAllAsTouched();
      return;
    }

    if(this.isEdit){

      this.service
        .update(
          this.announcementId,
          {
            announcementId:
              this.announcementId,
            ...this.form.value
          }
        )
        .subscribe(() => {

          this.router.navigate([
            '/announcements/list'
          ]);

        });

      return;
    }

    this.service
      .create({
        ...this.form.value,

        // change later when JWT user data is used
        createdBy: 1
      })
      .subscribe(() => {

        this.router.navigate([
          '/announcements/list'
        ]);

      });
  }
}