import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm!: FormGroup;
  token!: string;
  email!: string;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  passwordMatchValidator: ValidatorFn = (control: AbstractControl): null => {
    const password = control.get('newPassword');
    const confirmPassword = control.get('confirmPassword');

    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword?.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword?.setErrors(null);
    }

    return null;
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.token = decodeURIComponent(params['token']?.trim());
      this.email = decodeURIComponent(params['email']?.trim());

      // Verificar y registrar los parámetros
      console.log('Token:', this.token);
      console.log('Email:', this.email);

      if (!this.token || !this.email) {
        this.toastr.error('Token o email no proporcionado en la URL', 'Error');
        this.router.navigate(['/login']);
      }
    });

    this.resetPasswordForm = this.fb.group({
      newPassword: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  onResetPasswordSubmit(): void {
    if (this.resetPasswordForm.valid) {
      this.isLoading = true;
      const newPassword = this.resetPasswordForm.value.newPassword;

      // Imprimir el request antes de enviarlo
      console.log('Request:', {
        email: this.email,
        token: this.token,
        newPassword: newPassword
      });

      this.authService.resetPassword(this.email, this.token, newPassword).subscribe(
        response => {
          this.toastr.success('Contraseña restablecida exitosamente', '¡Éxito!');
          this.router.navigate(['/login']);
          this.isLoading = false;
        },
        error => {
          const errorMessage = error.error?.message || 'Error al restablecer la contraseña';
          this.toastr.error(errorMessage, 'Error');
          this.isLoading = false;
        }
      );
    }
  }
}