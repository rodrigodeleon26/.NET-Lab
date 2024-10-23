import { HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { tap } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const toastr = inject(ToastrService);
  const router = inject(Router);
  
  if (authService.isLoggedIn()) {
    const clonedReq = req.clone({
      headers: req.headers.set('Authorization', 'Bearer ' + authService.getToken())
    })
    return next(clonedReq).pipe(
      tap({
        error: (err: any) => {
          if (err.status === 401) {
            authService.deleteToken();
            setTimeout(() => {
              toastr.info('Su sesión ha expirado, por favor inicie sesión nuevamente', 'Sesión expirada');
              router.navigateByUrl('/login');
            }, 1500);
          } else if (err.status === 403) {
            toastr.error('No tiene permisos para acceder a este recurso', 'Acceso denegado');
          }
        }
      }),
    );
  }
  else
    return next(req);
};
