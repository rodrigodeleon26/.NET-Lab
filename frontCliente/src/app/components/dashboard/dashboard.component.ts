import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';
import { PacienteService } from '../../services/paciente.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit{
  constructor(
    private router: Router,
    private authService: AuthService,
    private userService: UserService,
    private pacienteService: PacienteService
  ) {}

  pacientes: any[] = [];

  ngOnInit(): void {
    this.pacienteService.getPacientes().subscribe((data: any[]) => {
      this.pacientes = data;
      console.log(this.authService.getClaims());
    });
  }

  onLogout() {
    this.authService.deleteToken(); 
    this.authService.setTwoFactorAuthenticated(false); // Actualizar el estado de autenticación 2FA
    this.router.navigateByUrl('/login');
  }
}
