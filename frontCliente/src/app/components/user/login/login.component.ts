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
  isLoadingLogin: boolean = false;

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
        password: ['', Validators.required],
        role: ['Paciente', [Validators.required]] 
      });

      this.forgotPasswordForm = this.formBuilder.group({
        email: ['', [Validators.required, Validators.email]]
      });
    }

  onSubmit(): void {
    if (this.form.valid) {
      this.isLoadingLogin = true;
      const { email, password, role } = this.form.value;
      this.authService.loginUser(email, password, role)
      .subscribe({
        next: (res:any) => {
          this.authService.saveToken(res.token, res.refreshToken);
          if (this.authService.getEmailConfirmedStatus()) {
            this.router.navigateByUrl('/inicio');
          } else {
            this.router.navigateByUrl('/resendEmailConfirmation');
          }
          this.isLoadingLogin = false;
        },
        error: (err:any) => {
          this.isLoadingLogin = false
          if (err.status === 400) {
            this.toastr.error(err.error.message, 'Login fallido');
          } else if (err.status === 403) {
            this.toastr.error('No tienes permiso para acceder con este rol', 'Error de autenticación');
          } else {
            console.log('Error durante el login:\n', err);
          }
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