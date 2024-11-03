import { Component, OnInit } from '@angular/core';
import { SegurosMedicosService } from '../../services/seguros-medicos.service';

@Component({
  selector: 'app-copagos',
  templateUrl: './copagos.component.html',
  styleUrl: './copagos.component.css'
})
export class CopagosComponent implements OnInit {
  loading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';

  SegurosMedicos: any[] = [];

  constructor(
    private segurosMedicosService: SegurosMedicosService,
  ) { }

  ngOnInit(): void {
    this.loading = true;

    this.segurosMedicosService.getSegurosMedicos().subscribe({
      next: (data) => {
        this.SegurosMedicos = data;
      },
      error: (error) => {
        console.error(error);
        this.showErrorMessage('Error al obtener los seguros médicos');
        this.loading = false;
      },
      complete: () => {
        this.loading = false;
      }
    });
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
