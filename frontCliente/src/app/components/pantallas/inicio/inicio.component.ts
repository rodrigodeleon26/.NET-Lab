import { Component, OnInit} from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { NavigationExtras } from '@angular/router';

@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.component.html',
  styleUrl: './inicio.component.css'
})
export class InicioComponent implements OnInit {
  cedula: string = '';
  loading: boolean = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const claims = this.authService.getClaims();
    this.cedula = claims ? claims.cedula : null;
  }

  verHistoriaClinica(): void {
    const navigationExtras: NavigationExtras = {
      state: { cedula: this.cedula }
    };
    this.router.navigateByUrl('/historia-clinica', navigationExtras);
  }

  verMisDatos(): void {
    const navigationExtras: NavigationExtras = {
      state: { cedula: this.cedula }
    };
    this.router.navigateByUrl('/mis-datos', navigationExtras);
  }

  verNotificaciones(): void {
    const navigationExtras: NavigationExtras = {
      state: { cedula: this.cedula }
    };
    this.router.navigateByUrl('/notificaciones', navigationExtras);
  }

  agendarme(): void {
    const navigationExtras: NavigationExtras = {
      state: { cedula: this.cedula }
    };
    this.router.navigateByUrl('/agendarse', navigationExtras);
  }

}
