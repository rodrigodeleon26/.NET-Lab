import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../services/auth.service';


@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.css']
})
export class ConfirmEmailComponent implements OnInit {
  token!: string;
  isLoading = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      this.route.queryParams.subscribe(params => {
        this.token = decodeURIComponent(params['token']?.trim());

        if (!this.token) {
          this.toastr.error('Token no proporcionado en la URL', 'Error');
          this.router.navigate(['/resendEmailConfirmation']);
        } else {
          this.confirmEmail();
        }
      });
    } else {
      this.router.navigate(['/login']);
    }
  }

  confirmEmail(): void {
    console.log(this.token);
    console.log(this.authService.getEmail());
    this.isLoading = true;
    this.authService.confirmEmail(this.authService.getEmail(), this.token)
      .subscribe({
        next: (res: any) => {
          this.toastr.success('Correo electrónico confirmado exitosamente.', 'Correo confirmado');
          this.isLoading = false;
          this.authService.saveToken(res.tokens.token, res.tokens.refreshToken);
          this.router.navigate(['/inicio']);
        },
        error: (err: any) => {
          this.toastr.error('Error al confirmar el correo electrónico.', 'Error');
          this.isLoading = false;
          this.authService.deleteToken();
          this.router.navigate(['/resendEmailConfirmation']);
        }
      });
  }
}