import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  form!: FormGroup; // Usar el operador de aserción no nulo
  forgotPasswordForm!: FormGroup;
  showModal: boolean = false;
  isLoading: boolean = false;

  constructor(
    public formBuilder: FormBuilder, 
    private toastr: ToastrService, 
    private authService: AuthService,
    private router: Router) {}

    ngOnInit(): void {
      if (this.authService.isLoggedIn()) {
        if (this.authService.getEmailConfirmedStatus()) {
          this.router.navigateByUrl('/inicio');
        } else {
          this.toastr.warning('Por favor, confirma tu correo electrónico para continuar.', 'Correo no confirmado');
          this.router.navigateByUrl('/resendEmailConfirmation');
        }
      }
  
      this.form = this.formBuilder.group({
        email: ['', Validators.required],
        password: ['', Validators.required]
      });

      this.forgotPasswordForm = this.formBuilder.group({
        email: ['', [Validators.required, Validators.email]]
      });
    }

  onSubmit(): void {
    if (this.form.valid) {
      console.log(this.form.value);
      this.authService.loginUser(this.form.value)
      .subscribe({
        next: (res:any) => {
          this.authService.saveToken(res.token, res.refreshToken);
          if (this.authService.getEmailConfirmedStatus()) {
            this.router.navigateByUrl('/inicio');
          } else {
            this.router.navigateByUrl('/resendEmailConfirmation');
          }
          this.isLoading = false;
        },
        error: (err:any) => {
          if (err.status === 400) {
            this.toastr.error(err.error.message, 'Login fallido');
          }
          else
            console.log('error during login:\n', err);
        }
      });

    }
  }

  openForgotPasswordModal(): void {
    this.showModal = true;
  }

  closeForgotPasswordModal(): void {
    this.showModal = false;
    this.forgotPasswordForm.reset();
  }

  onForgotPasswordSubmit(): void {
    if (this.forgotPasswordForm.valid) {
      this.isLoading = true; // Iniciar el spinner
      const email = this.forgotPasswordForm.value.email;
      this.authService.forgotPassword(email)
        .subscribe({
          next: (res: any) => {
            this.toastr.success('Correo de restablecimiento de contraseña enviado', '¡Correo enviado!');
            this.closeForgotPasswordModal(); // Cerrar el modal después de enviar el correo
            this.isLoading = false; // Detener el spinner
            this.forgotPasswordForm.reset(); // Restablecer el formulario
          },
          error: (err: any) => {
            this.toastr.error(err.error.message, 'Error');
            this.isLoading = false; // Detener el spinner
          }
        });
    }
  }
}