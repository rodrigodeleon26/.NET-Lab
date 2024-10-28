import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.isLoggedIn()) {
    router.navigate(['/login']);
    return false;
  }

  if (!authService.getEmailConfirmedStatus()) {
    if (state.url !== '/resendEmailConfirmation') {
      router.navigate(['/resendEmailConfirmation']);
      return false;
    }
    return true;
  }

  if (authService.getTwoFactorEnabledStatus() && !authService.isTwoFactorAuthenticated()) {
    if (state.url !== '/twoFactorAuth') {
      router.navigate(['/twoFactorAuth']);
      return false;
    }
    return true;
  }

  return true;
};
