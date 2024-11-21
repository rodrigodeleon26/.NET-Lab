import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  form!: FormGroup; // Usar el operador de aserción no nulo
  isLoading: boolean = false;

  constructor(
    public formBuilder: FormBuilder, 
    private toastr: ToastrService, 
    private authService: AuthService,
    private router: Router) {}

  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      this.router.navigateByUrl('/elegir-especialidad');
    }

    this.form = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.authService.loginUser(this.form.value)
      .subscribe({
        next: (res:any) => {
          this.authService.saveToken(res.token, res.refreshToken);
          this.router.navigateByUrl('/elegir-especialidad');
          this.toastr.success('Inicio de sesión exitoso', 'Bienvenido');
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
}