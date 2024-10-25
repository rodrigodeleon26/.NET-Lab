import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-resend-email-confirmation',
  templateUrl: './resend-email-confirmation.component.html',
  styleUrl: './resend-email-confirmation.component.css'
})
export class ResendEmailConfirmationComponent {
isLoading = false;

  constructor(
    private toastr: ToastrService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
  }

  resendEmailConfirmation(): void {
    this.isLoading = true; // Iniciar el spinner
    this.authService.resendEmailConfirmation(this.authService.getEmail()).subscribe(
      response => {
        this.toastr.success('Correo de confirmación reenviado exitosamente.', 'Correo reenviado');
        this.isLoading = false;
      },
      error => {
        this.toastr.error('Error al reenviar el correo de confirmación.', 'Error');
        this.isLoading = false;
      }
    );
  }
}
