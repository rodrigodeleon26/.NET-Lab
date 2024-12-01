import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.component.html',
  styleUrl: './inicio.component.css'
})
export class InicioComponent {

  loading: boolean = false;

  constructor(
    private router: Router
  ) {}

  ListMedicos(): void {
    // const navigationExtras: NavigationExtras = {
    //   state: { cedula: this.cedula }
    // };
    this.router.navigateByUrl('/listMedicos');
  }

  Especialidades(): void {
    this.router.navigateByUrl('/especialidades');
  }

  Consultorios(): void {
    this.router.navigateByUrl('/consultorios');
  }

  Copagos(): void {
    this.router.navigateByUrl('/copagos');
  }

  Pacientes(): void {
    this.router.navigateByUrl('/pacientes');
  }

  Contratos(): void {
    this.router.navigateByUrl('/contratos');
  }

  Facturas(): void {
    this.router.navigateByUrl('/facturas');
  }
}
