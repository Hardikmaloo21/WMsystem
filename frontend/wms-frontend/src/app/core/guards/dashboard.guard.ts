import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const dashboardGuard: CanActivateFn = () => {

  const authService = inject(AuthService);
  const router = inject(Router);

  const role = authService.userRole();

  if (role === 'Employee') {

    router.navigate(['/employee-dashboard']);
    return false;

  }

  return true;
};