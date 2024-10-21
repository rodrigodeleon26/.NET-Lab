import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  form!: FormGroup; // Usar el operador de aserción no nulo
  isLoading = false; // Variable de estado para el spinner

  constructor(
    public formBuilder: FormBuilder, 
    private toastr: ToastrService, 
    private authService: AuthService,
    private router: Router) {}

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
    if(this.authService.isLoggedIn()){
      this.router.navigateByUrl('/dashboard');
    }
    this.form = this.formBuilder.group({
      nombres: ['', Validators.required],
      apellidos: ['', Validators.required],
      documento: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(8), Validators.pattern('^[0-9]*$')]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.isLoading = true; // Mostrar el spinner
      const { nombres, apellidos, documento, email, password } = this.form.value;
      const requestPayload = { nombres, apellidos, documento, email, password };

      this.authService.registerUser(requestPayload)
      .subscribe({
        next: (res: any) => {
          this.isLoading = false; // Ocultar el spinner
          if (res.succeeded) {
            this.form.reset();
            this.toastr.success('Nuevo usuario creado', 'Registro exitoso');
            this.router.navigate(['/login']);
          } else {
            res.errors.forEach((x: any) => {
              switch (x.code) {
                case "DuplicateUserName":
                  this.toastr.error(x.description, 'Registro fallido');
                  break;

                case "DuplicateEmail":
                  this.toastr.error(x.description, 'Registro fallido');
                  break;

                case "DuplicateDocumento":
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
          this.isLoading = false; // Ocultar el spinner
          if (err.status === 400) {
            if (Array.isArray(err.error)) {
              err.error.forEach((x: any) => {
                this.toastr.error(x.description, 'Registro fallido');
              });
            } else if (err.error && err.error.code && err.error.description) {
              this.toastr.error(err.error.description, 'Registro fallido');
            } else {
              this.toastr.error('Ocurrió un error inesperado. Por favor, inténtelo de nuevo más tarde.', 'Error');
            }
          } else {
            console.log('error', err);
            this.toastr.error('Ocurrió un error inesperado. Por favor, inténtelo de nuevo más tarde.', 'Error');
          }
        }
      });
    }
  }
}