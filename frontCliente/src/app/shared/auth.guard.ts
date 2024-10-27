import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Si no está autenticado, redirige a /login
  if (!authService.isLoggedIn()) {
    router.navigate(['/login']);
    return false;
  }

  // Si el correo no está confirmado y la ruta actual no es /resendEmailConfirmation
  if (!authService.getEmailConfirmedStatus() && state.url !== '/resendEmailConfirmation') {
    router.navigate(['/resendEmailConfirmation']);
    return false;
  }

  return true;
};