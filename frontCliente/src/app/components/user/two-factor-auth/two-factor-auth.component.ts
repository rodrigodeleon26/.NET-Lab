import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';


@Component({
  selector: 'app-two-factor-auth',
  templateUrl: './two-factor-auth.component.html',
  styleUrl: './two-factor-auth.component.css'
})
export class TwoFactorAuthComponent implements OnInit {
  qrCodeImage: string = '';
  authCode: string = '';
  isLoading: boolean = false;
  isSendingQrCode: boolean = false;

  constructor(
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
  }

  verifyAuthCode(): void {
    const email = this.authService.getEmail();
    this.isLoading = true;
    this.authService.validateTwoFactorCode(email, this.authCode).subscribe({
      next: (response: any) => {
        this.isLoading = false;
        this.authService.saveToken(response.tokens.token, response.tokens.refreshToken); // Guardar los nuevos tokens
        this.toastr.success(response.message, 'Éxito');
        this.router.navigateByUrl('/inicio');
      },
      error: (error: any) => {
        this.isLoading = false;
        this.toastr.error(error.error.message, 'Error');
      }
    });
  }
}