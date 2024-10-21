import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  form!: FormGroup; // Usar el operador de aserción no nulo

  constructor(public formBuilder: FormBuilder, private toastr: ToastrService, private authService: AuthService) {}

  passwordMatchValidator: ValidatorFn = (control: AbstractControl): null => {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword?.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword?.setErrors(null);
    }

    return null;
  }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  onSubmit(): void {
    if (this.form.valid) {
      const { email, password, fullName } = this.form.value;
      const requestPayload = { email, password, fullName };

      this.authService.registerUser(requestPayload)
      .subscribe({
        next: (res: any) => {
          if (res.succeeded) {
            this.form.reset();
            this.toastr.success('Nuevo usuario creado', 'Registro exitoso');
          } else {
            res.errors.forEach((x: any) => {
              switch (x.code) {
                case "DuplicateUserName":
                  this.toastr.error(x.description, 'Registro fallido');
                  break;

                case "DuplicateEmail":
                  this.toastr.error(x.description, 'Registro fallido');
                  break;

                default:
                  this.toastr.error('Contacte al desarrollador', 'Registro fallido');
                  console.log(x);
                  break;
              }
            });
          }
          console.log('response:', res);
        },
        error: (err: any) => {
          if (err.status === 400 && err.error) {
            err.error.forEach((x: any) => {
              this.toastr.error(x.description, 'Registro fallido');
            });
          } else {
            console.log('error', err);
            this.toastr.error('Ocurrió un error inesperado. Por favor, inténtelo de nuevo más tarde.', 'Error');
          }
        }
      });
    }
  }
}