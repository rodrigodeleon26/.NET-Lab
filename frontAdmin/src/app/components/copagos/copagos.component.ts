import { Component, OnInit } from '@angular/core';
import { CopagosService } from '../../services/copagos.service';

@Component({
  selector: 'app-copagos',
  templateUrl: './copagos.component.html',
  styleUrl: './copagos.component.css'
})
export class CopagosComponent implements OnInit {
  loading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';

  selectedSeguroMedico: any = null;
  selectedSeguroArticulos: any[] = [];

  constructor(
    private CopagosService: CopagosService
  ) { }

  ngOnInit(): void {
    this.CopagosService.selectedSeguroMedico$.subscribe({
      next: (data) => {
        this.selectedSeguroMedico = data;
      },
      error: (error) => {
        console.error(error);
      },
    });

    this.CopagosService.articulosDeSeguroMedico$.subscribe({
      next: (data) => {
        this.selectedSeguroArticulos = data;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  getPrecioActual(precios: any[]): number | null {
    const today = new Date();
    const preciosValidos = precios.filter(precio => new Date(precio.fechaInicio) <= today);
    
    if (preciosValidos.length === 0) {
      return null; // no hay precios válidos para la fecha actual
    }
  
    const precioActual = preciosValidos
      .sort((a, b) => new Date(b.fechaInicio).getTime() - new Date(a.fechaInicio).getTime())[0];
    
    return precioActual ? precioActual.precioBase : null;
  }

  showSuccessMessage(message: string) {
    this.successMessage = message;
      setTimeout(() => {
        this.successMessage = '';
      }, 3000);
  }

  showErrorMessage(message: string) {
    this.errorMessage = message;
    setTimeout(() => {
      this.errorMessage = '';
    }, 3000);
  }

  showModalSM(){
    
  }
}
