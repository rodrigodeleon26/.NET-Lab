import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const toastr = inject(ToastrService);
  
  if (!authService.isLoggedIn()) {
    router.navigate(['/login']);
    return false;
  }

  const claims = authService.getClaims();
  const expectedRole = "Paciente";

  if (expectedRole && claims.role !== expectedRole) {
    authService.logout();
    authService.deleteToken();
    router.navigate(['/login']).then(() => {
      window.location.reload();
    });
    toastr.warning('No tiene permisos para acceder a este recurso', 'Acceso denegado');
    return false;
  }

  if (!authService.getEmailConfirmedStatus()) {
    if (state.url.startsWith('/confirmEmail')) {
      return true;
    }
    if (state.url !== '/resendEmailConfirmation') {
      router.navigate(['/resendEmailConfirmation']);
      return false;
    }
    return true;
  } else {
    if (state.url === '/resendEmailConfirmation') {
      router.navigate(['/inicio']);
      return false;
    }
  }

  if (authService.getTwoFactorEnabledStatus()) {
    if (authService.isTwoFactorAuthenticated()) {
      if (state.url === '/twoFactorAuth') {
        router.navigate(['/inicio']);
        return false;
      }
      return true;
    } else {
      if (state.url !== '/twoFactorAuth') {
        router.navigate(['/twoFactorAuth']);
        return false;
      }
      return true;
    }
  }


  return true;
};