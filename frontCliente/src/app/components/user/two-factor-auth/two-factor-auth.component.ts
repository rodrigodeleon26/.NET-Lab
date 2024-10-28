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
  isImageLoading: boolean = true; // Variable para controlar el estado de carga de la imagen

  constructor(
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.generateQrCode();
    console.log(this.authService.getClaims());
    console.log(this.authService.getTwoFactorEnabledStatus());
  }

  generateQrCode(): void {
    const email = this.authService.getEmail();
    this.authService.generateQrCode(email).subscribe({
      next: (response: any) => {
        this.qrCodeImage = response.qrCodeImageUrl;
        this.isImageLoading = false; 
        console.log(response);
      },
      error: (error: any) => {
        this.toastr.error('Error al generar el código QR', 'Error');
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
        this.router.navigateByUrl('/dashboard');
        console.log(this.authService.getTwoFactorEnabledStatus());
      },
      error: (error: any) => {
        this.isLoading = false;
        this.toastr.error(error.error.message, 'Error');
      }
    });
  }
}