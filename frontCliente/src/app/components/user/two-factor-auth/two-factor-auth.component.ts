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
    
    // this.generateQrCode();
    console.log(this.authService.getClaims());
    console.log(this.authService.isTwoFactorAuthenticated());
    console.log(this.authService.getTwoFactorEnabledStatus());
  }

  sendQrCodeByEmail(): void {
    const email = this.authService.getEmail();
    this.isSendingQrCode = true;
    this.authService.sendQrCodeByEmail(email).subscribe({
      next: (response: any) => {
        this.toastr.success('Código QR enviado por email', 'Éxito');
        this.isSendingQrCode = false;
      },
      error: (error: any) => {
        this.toastr.error('Error al enviar el código QR', 'Error');
        this.isSendingQrCode = false;
      }
    });
  }

  verifyAuthCode(): void {
    const email = this.authService.getEmail();
    this.isLoading = true;
    this.authService.validateTwoFactorCode(email, this.authCode).subscribe({
      next: (response: any) => {
        this.isLoading = false;
        this.authService.setTwoFactorAuthenticated(true); // Actualizar el estado de autenticación 2FA
        this.toastr.success(response.message, 'Éxito');
        this.router.navigateByUrl('/inicio');
        console.log(this.authService.getTwoFactorEnabledStatus());
      },
      error: (error: any) => {
        this.isLoading = false;
        this.toastr.error(error.error.message, 'Error');
      }
    });
  }
}