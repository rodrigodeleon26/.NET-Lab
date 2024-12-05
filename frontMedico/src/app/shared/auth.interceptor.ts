import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private isRefreshing = false;

  constructor(
    private authService: AuthService, 
    private router: Router, 
    private toastr: ToastrService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.getToken();

    if (token) {
      const clonedReq = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${token}`)
      });

      return next.handle(clonedReq).pipe(
        catchError((err: HttpErrorResponse) => {
          if (err.status === 401 && !this.isRefreshing) {
            this.isRefreshing = true;
            return this.authService.refreshToken().pipe(
              switchMap((res: any) => {
                this.isRefreshing = false;
                this.authService.saveToken(res.token, res.refreshToken);
                const newReq = req.clone({
                  headers: req.headers.set('Authorization', `Bearer ${res.token}`)
                });
                return next.handle(newReq);
              }),
              catchError((refreshErr) => {
                this.isRefreshing = false;
                this.authService.deleteToken();
                this.toastr.info('Su sesión ha expirado, por favor inicie sesión nuevamente', 'Sesión expirada');
                this.router.navigateByUrl('/login');
                return throwError(refreshErr);
              })
            );
          } else if (err.status === 403) {
            this.toastr.warning('No tiene permisos para acceder a este recurso', 'Acceso denegado');
          }

          return throwError(err);
        })
      );
    } else {
      return next.handle(req);
    }
  }
}