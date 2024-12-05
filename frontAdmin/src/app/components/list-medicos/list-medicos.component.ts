import { Component, OnInit } from '@angular/core';
import { MedicosService } from '../../services/medicos.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list-medicos',
  templateUrl: './list-medicos.component.html',
  styleUrl: './list-medicos.component.css'
})
export class ListMedicosComponent implements OnInit{

  loading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';
  verDetallePara: number | null = null;
  isModalVisible: boolean = false;
  isModalVisibleDetalle: boolean = false;
  medicoBorrarId: string | null = null;
  medicoSeleccionado: any | null = null;
  paginaActual: number = 1;

  busqueda: string = '';

  medicos: any[] = [];

  constructor(
    private medicosService: MedicosService,
    private router : Router,
  ) {}

  ngOnInit(): void {
    this.medicos = [];
    this.medicosService.getMedicosPaginadosYFiltrados(this.paginaActual, '')
      .subscribe({
        next: (data) => {
          console.log(data);
          this.medicos = data;
        },
        error: (error) => {
          console.error(error);
        }
      });
  }

  showSuccessMessage(message: string) {
    this.successMessage = message;
      setTimeout(() => {
        this.errorMessage = '';
      }, 3000);
  }

  showErrorMessage(message: string) {
    this.errorMessage = message;
    setTimeout(() => {
      this.errorMessage = '';
    }, 3000);
  }

  agregarMedico(){
    this.loading = true;
    //redireccionar a /generarMedico
    this.router.navigate(['/generarMedico']);
  }

  editarMedico(id: string){
    this.loading = true;
    //redireccionar a /generarMedico/id
    this.router.navigate(['/generarMedico', id]);
  }

  onModalContainerClick(event: MouseEvent): void {
    if ((event.target as HTMLElement).classList.contains('fixed')) {
      this.isModalVisible = false;
      this.isModalVisibleDetalle = false;
      this.medicoBorrarId = null;
      this.verDetallePara = null;
    }
  }

  borrarMedico(){
    console.log('Borrando Medico');
    console.log(this.medicoBorrarId);
    if(this.medicoBorrarId){
      this.medicosService.deleteMedico(this.medicoBorrarId)
      .subscribe({
        next: (data) => {
          console.log(data);
          this.showSuccessMessage('Medico eliminado correctamente');
          //reiniciar la lista de medicos
          this.medicosService.getMedicosPaginadosYFiltrados(this.paginaActual, '')
            .subscribe({
              next: (data) => {
                console.log(data);
                this.medicos = data;
              },
              error: (error) => {
                console.error(error);
              }
            });
        },
        error: (error) => {
          console.error(error);
          this.showErrorMessage('Error al eliminar el medico');
        }
      });
    }
    else{
      this.showErrorMessage('Error al eliminar el medico');
    }
    this.isModalVisible = false;
    this.medicoBorrarId = null;
  }

  seleccionarMedico(){
    this.medicoSeleccionado = this.medicos.find(medico => medico.id === this.verDetallePara);
  }

  buscar(){
    console.log(this.busqueda);
    this.medicosService.getMedicosPaginadosYFiltrados(1, this.busqueda).subscribe({
      next: (data) => {
        console.log(data);
        this.medicos = data;
        this.paginaActual = 1;
      },
      error: (error) => {
        console.error(error);
      },
      complete: () => {
      }
    });
  }

  cambiarPagina(pagina: number){
    if(pagina < 1){
      return;
    }
    this.medicosService.getMedicosPaginadosYFiltrados(pagina, this.busqueda).subscribe({
        next: (data) => {
          console.log(data);
          if(data.length === 0){
            return;
          }
          this.medicos = data;
          this.paginaActual = pagina;
        },
        error: (error) => {
          console.error(error);
        },
        complete: () => {
        }
      });
  }

  LimpiarBusqueda(){
    this.busqueda = '';
    this.medicosService.getMedicosPaginadosYFiltrados(1, '').subscribe({
      next: (data) => {
        console.log(data);
        this.medicos = data;
        this.paginaActual = 1;
      },
      error: (error) => {
        console.error(error);
      },
      complete: () => {
      }
    });
  }
}
