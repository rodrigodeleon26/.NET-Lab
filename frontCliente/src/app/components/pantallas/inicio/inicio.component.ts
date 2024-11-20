import { Component, OnInit} from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';

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
    console.log(this.cedula);
  }

  verHistoriaClinica(): void {
    //redirigir al componente de historia clinica
    this.router.navigateByUrl(`/historia-clinica?cedula=${this.cedula}`);
  }
}
