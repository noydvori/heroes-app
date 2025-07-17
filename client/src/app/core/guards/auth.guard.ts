import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { isTokenValid } from '../utils/token-utils';

export const authGuard: CanActivateFn = (route, state) => {
  const token = localStorage.getItem('token');
  const router = inject(Router);

  if (!token || !isTokenValid(token)) {
    localStorage.removeItem('token');
    router.navigate(['/login']);
    return false;
  }

  return true;
};
