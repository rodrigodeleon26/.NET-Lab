import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { PacienteService } from '../../services/paciente.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  pacientes: any[] = [];

  constructor(
    public authService: AuthService,
    private pacienteService: PacienteService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
  }

  onLogout() {
    this.authService.deleteToken();
    this.router.navigateByUrl('/login');
  }

  enableTwoFactorAuth() {
    const email = this.authService.getEmail();
    this.authService.enableTwoFactorAuth(email).subscribe({
      next: (response: any) => {
        this.toastr.success(response.message, 'Éxito');
        this.authService.saveToken(response.tokens.token, response.tokens.refreshToken);
      },
      error: (error: any) => {
        this.toastr.error(error.error.message, 'Error');
        console.log(error);
      }
    });
  }

  disableTwoFactorAuth() {
    const email = this.authService.getEmail();
    this.authService.disableTwoFactorAuth(email).subscribe({
      next: (response: any) => {
        this.toastr.success(response.message, 'Éxito');
        this.authService.saveToken(response.tokens.token, response.tokens.refreshToken);
        console.log(response);
      },
      error: (error: any) => {
        this.toastr.error(error.error.message, 'Error');
        console.log(error);
      }
    });
  }
}