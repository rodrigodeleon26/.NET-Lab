import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { ConsultaMedicaService } from '../../../services/consulta-medica.service';
import { PacienteService } from '../../../services/paciente.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-historia-clinica',
  templateUrl: './historia-clinica.component.html',
  styleUrl: './historia-clinica.component.css'
})
export class HistoriaClinicaComponent implements OnInit {
  cedula: string = '';

  paciente: any = {};
  historiaClinica: any = {};

  consulta: any = {};
  modalConsulta: boolean = false;

  recetas: any[] = [];
  modalRecetas: boolean = false;

  estudios: any[] = [];
  modalEstudios: boolean = false;

  pageNumber = 1;
  pageSize = 5;
  pages: number[] = [];
  totalItems = 0;
  totalPages = 0;

  fechaInicio: string = '';
  fechaFin: string = '';
  ordenAscendente: boolean = false;
  ordenDescendente: boolean = true;
  orden: string = 'desc';

  especialidades: any[] = [];

  modalFiltro: boolean = false;
  filtroActivo: boolean = false;

  loading: boolean = false;
  
  hayDatos: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private consultaMedicaService: ConsultaMedicaService,
    private pacienteService: PacienteService,
    private router: Router,
    private toastr: ToastrService
  ) { 
    const navigation = this.router.getCurrentNavigation();
    this.cedula = navigation?.extras.state?.['cedula'] || '';
  }

  ngOnInit(): void {
    this.loading = true;  
    if (!this.cedula) {
      window.location.href = '/cliente/inicio';
    } else {
      this.obtenerDatos();
    }
  }

  obtenerDatos(): void {
    this.getEspecialidades().then(() => {
      this.obtenerHistoriaClinica();
    }).catch(error => {
      this.toastr.error('Error al obtener la historia clinica', 'Error');
    });
  }

  obtenerHistoriaClinica(): void {
    this.loading = true; 
    this.pacienteService.obtenerMiHistoriaClinica(this.cedula, this.pageNumber, this.pageSize, this.orden, this.fechaInicio, this.fechaFin, this.especialidades)
      .subscribe(
        response => {
          this.paciente = response.paciente;
          this.historiaClinica = response.consultasMedicasConCitas;
          this.totalPages = Math.ceil(response.totalItems / this.pageSize);  
          this.hayDatos = true;
          this.loading = false;
          this.updatePages();
        },
        error => {
          if (
            error.error?.includes("No puedes acceder a la historia clínica de otro paciente.") ||
            error.message?.includes("No puedes acceder a la historia clínica de otro paciente.")
          ) {
            // Redirige a la ruta de inicio.
            this.toastr.error('No puedes acceder a la historia clínica de otro paciente.', 'Error');
            this.router.navigate(['/inicio']);
          } else {          
            this.loading = false;
            this.toastr.error(error.message, 'Error al cancelar la cita');
          }
        }
      );
  }
  changePage(event: any) {
    this.pageNumber = event;
    this.obtenerHistoriaClinica();
  }
  updatePages() {
    const visiblePages = 7; // Cuántas páginas mostrar en total (3 a cada lado + actual)
    let startPage = Math.max(1, this.pageNumber - 3);
    let endPage = Math.min(this.totalPages, this.pageNumber + 3);

    // Ajuste en caso de estar al inicio o al final del rango de páginas
    if (this.pageNumber <= 3) {
      endPage = Math.min(visiblePages, this.totalPages);
    } else if (this.pageNumber + 3 >= this.totalPages) {
      startPage = Math.max(this.totalPages - visiblePages + 1, 1);
    }

    // Generar el array de páginas a mostrar
    this.pages = Array.from({ length: (endPage - startPage + 1) }, (_, i) => startPage + i);
  }
  

  getEspecialidades(): Promise<any> {
    return new Promise((resolve, reject) => {
      this.consultaMedicaService.getEspecialidades().subscribe(
        response => {
          this.especialidades = response.map((especialidad: any) => {
            return {
              id: especialidad.id,
              nombre: especialidad.nombre,
              IsChecked: true
            };
          });
          resolve(this.especialidades);
        },
        error => {
          console.log(error);
          reject(error);
        }
      );
    });
  }
  isCheckboxDisabled(especialidad: any): boolean {
    const selectedCount = this.especialidades.filter(e => e.IsChecked).length;
    return selectedCount === 1 && especialidad.IsChecked;
  }
  mostrarEliminarEspecialidadesButton(): boolean {
    return this.filtroActivo && this.especialidades.some(e => !e.IsChecked);
  }
  eliminarEspecialidades() {
    this.especialidades.forEach(e => e.IsChecked = true);
    if (!this.fechaFin && !this.fechaInicio) {
      this.filtroActivo = false;
    }
    this.obtenerHistoriaClinica(); 
  }

  aplicarFiltros() {
    this.modalFiltro = false;
    this.filtroActivo = true;
    this.obtenerHistoriaClinica();
  }

  verConsulta(consulta: any) {
    this.consulta = consulta;
    this.modalConsulta = true;
  }
  closeModalConsulta() {
    this.consulta = {};
    this.modalConsulta = false;
  }

  verReceta(recetas: any[]) {
    this.recetas = recetas;
    this.modalRecetas = true;
  }
  closeModalRecetas() {
    this.recetas = [];
    this.modalRecetas = false;
  }

  verEstudios(estudios: any[]) {
    this.estudios = estudios;
    this.modalEstudios = true;
  }
  closeModalEstudios() {
    this.estudios = [];
    this.modalEstudios = false;
  }

  descargarPdf(url: string): void {
    const link = document.createElement('a');
    link.href = url;
    link.download = url.split('/').pop() || 'download';
    link.target = '_blank';
    link.click();
  }

  abrirModalFiltro() {
    this.modalFiltro = true;
  }
  cerrarModalFiltro() {
    this.modalFiltro = false;
  }

  toggleOrden(tipo: string) {
    if (tipo === 'ascendente') {
      this.orden = 'asc';
      this.ordenAscendente = true;
      this.ordenDescendente = false;
    } else {
      this.orden = 'desc';
      this.ordenAscendente = false;
      this.ordenDescendente = true;
    }
  }

  eliminarRango() {
    this.fechaInicio = '';
    this.fechaFin = '';
    if (this.especialidades.every(e => e.IsChecked)) {
      this.filtroActivo = false;
    }
    this.obtenerHistoriaClinica();
  }
}
