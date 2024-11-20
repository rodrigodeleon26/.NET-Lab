import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ConsultaMedicaService } from '../../../services/consulta-medica.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-historia-clinica',
  templateUrl: './historia-clinica.component.html',
  styleUrl: './historia-clinica.component.css'
})
export class HistoriaClinicaComponent implements OnInit {
  paciente: any = {};
  historiaClinica: any = {};

  recetas: any[] = [];
  modalRecetas: boolean = false;

  estudios: any[] = [];
  modalEstudios: boolean = false;

  imagenSeleccionada: any = '';
  modalImagen: boolean = false;

  modalFiltro: boolean = false;
  filtroActivo: boolean = false;

  search: string = '';
  disableSearch: boolean = false;

  pageNumber = 1;
  pageSize = 2;
  pages: number[] = [];
  totalItems = 0;
  totalPages = 0;

  fechaInicio: string = '';
  fechaFin: string = '';
  ordenAscendente: boolean = false;
  ordenDescendente: boolean = true;
  orden: string = 'desc';

  especialidades: any[] = [];
  disabledEspe: boolean = false;

  loading: boolean = false; 
  errorMessage: string = '';

  hayPaciente: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private consultaMedicaService: ConsultaMedicaService,
    private sanitizer: DomSanitizer
  ) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const documento = params['documento'];
      if (!documento) return;
      this.search = documento;
      this.disableSearch = true;
      this.obtenerDatos(); 
    });
    this.getEspecialidades();
  }

  obtenerDatos(): void {
    this.getEspecialidades().then(() => {
      this.obtenerHistoriaClinica(this.search);
    }).catch(error => {
      console.error('Error al obtener especialidades', error);
    });
  }

  obtenerHistoriaClinica(documento: string): void {
    this.loading = true;
    this.consultaMedicaService.obtenerHistoriaClinica(documento, this.pageNumber, this.pageSize, this.orden, this.fechaInicio, this.fechaFin, this.especialidades).subscribe(
      response => {
        this.loading = false;
        this.paciente = response.paciente;
        this.historiaClinica = response.consultasMedicasConCitas;
        this.hayPaciente = true;
        this.totalPages = Math.ceil(response.totalItems / this.pageSize);  
        this.updatePages();
      },
      error => {
        this.loading = false;
        this.errorMessage = error.error.message;
        setTimeout(() => {
          this.errorMessage = '';
        }, 3000);
      }
    );
  }

  changePage(event: any) {
    this.pageNumber = event;
    this.obtenerHistoriaClinica(this.paciente.documento);
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

  aplicarFiltros() {
    this.modalFiltro = false;
    this.filtroActivo = true;
    this.obtenerHistoriaClinica(this.paciente.documento);
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
    this.obtenerHistoriaClinica(this.paciente.documento);
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
    this.obtenerHistoriaClinica(this.paciente.documento); 
  }
}
