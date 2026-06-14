import {
  HttpInterceptorFn,
  HttpErrorResponse
} from '@angular/common/http';

import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

import { NotificationService }
from '../services/notification.service';

export const errorInterceptor: HttpInterceptorFn =
(req, next) => {

  const router = inject(Router);

  const notify =
    inject(NotificationService);

  return next(req).pipe(

    catchError(
      (error: HttpErrorResponse) => {

        if (error.status === 401) {

          notify.error(
            'Session expired. Please login again.'
          );

          router.navigate([
            '/auth/login'
          ]);
        }

        else if (error.status === 403) {

          notify.error(
            'You do not have permission to perform this action.'
          );

          router.navigate([
            '/unauthorized'
          ]);
        }

        else {

          notify.error(
            error?.error?.message ??
            'Something went wrong.'
          );
        }

        return throwError(
          () => error
        );
      }
    )
  );
};