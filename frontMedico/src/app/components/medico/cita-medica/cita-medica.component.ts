import { Component, AfterViewInit, OnInit } from '@angular/core';
import { PacienteService } from '../../../services/paciente.service';
import { CitasMedicasService } from '../../../services/cita-medica.service';
import { ConsultaMedicaService } from '../../../services/consulta-medica.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { forkJoin, lastValueFrom } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-cita-medica',
  templateUrl: './cita-medica.component.html',
  styleUrls: ['./cita-medica.component.css']
})
export class CitaMedicaComponent implements OnInit {
  citasMedicas: any[] = []; // Lista de citas
  nuevaCitaForm: FormGroup;  // Formulario reactivo para nueva cita
  loading: boolean = true; // Indicador de carga
  successMessage: string = ''; // Mensaje de éxito
  errorMessage: string = ''; // Mensaje de error
  modalVerCita: boolean = false; // Modal de eliminación de cita
  citaSeleccionada: any = null;
  especialidadSeleccionada: string = ''; // Especialidad seleccionada
  fechaVisible: Date = new Date();
  paginaActual: number = 1;
  hayMasResultados: boolean = true;

  constructor(
    private citasMedicasService: CitasMedicasService,
    private ConsultaMedicaService: ConsultaMedicaService,
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private location: Location
  ) {
    // Inicializa el formulario de nueva cita
    this.nuevaCitaForm = this.fb.group({
      fecha: ['', Validators.required],
      hora: ['', Validators.required],
      paciente: ['', Validators.required],
      estado: ['PENDIENTE', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loading = true;
    this.route.queryParams.subscribe(params => {
      const especialidad = params['especialidad'];
      this.especialidadSeleccionada = especialidad;
      const fechaVisible = params['fecha'];
      if (fechaVisible) {
        const [year, month, day] = fechaVisible.split('-').map(Number);
        this.fechaVisible = new Date(year, month - 1, day);
      }
      else {
        this.fechaVisible = new Date();
      }
    });
    this.cargarCitasMedicas(this.especialidadSeleccionada, this.fechaVisible).then(() => {
      this.loading = false; // Se ejecuta solo después de que los datos hayan sido cargados
    });
  }
  
  // Cargar todas las citas médicas
  async cargarCitasMedicas(espec: string, fechaVisible: Date): Promise<void> {
    try {
      const data = await lastValueFrom(this.citasMedicasService.obtenerCitasMedicasPorEspecialidad(espec, this.paginaActual, fechaVisible)); // Convertir Observable a Promesa
      this.citasMedicas = data;
      console.log('Datos cargados en citasMedicas:', data); // Imprime los datos en la consola
      const paginadoSigue = await lastValueFrom(this.citasMedicasService.chequearPaginaCita(espec, this.paginaActual + 1, fechaVisible));
      if (paginadoSigue === true) {
        this.hayMasResultados = true;
      }
      else {
        this.hayMasResultados = false;
      }
    } catch (error) {
      this.errorMessage = 'Error al cargar las citas médicas';
      console.error('Error al cargar citas:', error);
    }
  }

  cambiarPagina(incremento: number): void {
    this.paginaActual += incremento; // Incrementa o decrementa la página
    this.cargarCitasMedicas(this.especialidadSeleccionada, this.fechaVisible);
  }

  esFechaHoy(): boolean {
    const hoy = new Date();
    return (
        this.fechaVisible &&
        this.fechaVisible.getDate() === hoy.getDate() &&
        this.fechaVisible.getMonth() === hoy.getMonth() &&
        this.fechaVisible.getFullYear() === hoy.getFullYear()
      );
  }

  esFechaHoyOMenor(): boolean {
    const hoy = new Date();
    
    // Asegurarse de que `fechaVisible` esté definido y sea una fecha válida
    return (
      this.fechaVisible &&
      (
        this.fechaVisible.getFullYear() < hoy.getFullYear() ||
        (this.fechaVisible.getFullYear() === hoy.getFullYear() &&
         this.fechaVisible.getMonth() < hoy.getMonth()) ||
        (this.fechaVisible.getFullYear() === hoy.getFullYear() &&
         this.fechaVisible.getMonth() === hoy.getMonth() &&
         this.fechaVisible.getDate() <= hoy.getDate())
      )
    );
  }

  getHora(fecha: string): string {
    const date = new Date(fecha);
    const horas = date.getHours().toString().padStart(2, '0');
    const minutos = date.getMinutes().toString().padStart(2, '0');
    return `${horas}:${minutos}`;
  }

  verCita(id: number): void {
    this.citaSeleccionada = this.citasMedicas.find(cita => cita.id === id);
    console.log(this.citaSeleccionada.consultaMedicaId);
    this.modalVerCita = true;
  }

  desverCita(): void {
    this.modalVerCita = false;
    this.citaSeleccionada = null;
  }

  generarConsultaMedica(id: number): void {
    this.modalVerCita = false;
    this.citaSeleccionada = null;
    this.loading = true;
    this.citaSeleccionada = this.citasMedicas.find(cita => cita.id === id);
    this.ConsultaMedicaService.crearConsultaSinDatos(this.citaSeleccionada.id).subscribe(
      (response) => {
        console.log('Consulta creada exitosamente:', response);

        // Asigna la ID de la nueva consulta a citaSeleccionada.consultaMedicaId
        this.citaSeleccionada.consultaMedicaId = response.id;  // Asegúrate de que la respuesta contiene la ID  // Cambia el estado de la cita a 'Completada'
        this.citasMedicasService.actualizarCitaMedica(this.citaSeleccionada.id, this.citaSeleccionada).subscribe(() => {
          this.route.queryParams.subscribe(params => {
            const especialidad = params['especialidad'];
            this.especialidadSeleccionada = especialidad;
            const fechaVisible = params['fecha'];
            if (fechaVisible) {
              const [year, month, day] = fechaVisible.split('-').map(Number);
              this.fechaVisible = new Date(year, month - 1, day);
            }
            else {
              this.fechaVisible = new Date();
            }
          });
          this.cargarCitasMedicas(this.especialidadSeleccionada, this.fechaVisible).then(() => {
            this.loading = false;
          });
        });
        
        
        /* this.citasMedicasService.editarEstado(id, 'Completada').subscribe(() => {
        }); */
        const currentUrl = this.router.url;
        localStorage.setItem('previousUrl', currentUrl);
        // Abre una nueva ventana con la ID de la consulta
        window.location.href = `/medico/consulta-medica?consultaSeleccionada=${this.citaSeleccionada.consultaMedicaId}`;

        this.loading = false;
        // Puedes actualizar el estado o mostrar una notificación de éxito aquí
      },
      (error) => {
        console.error('Error al crear la consulta:', error);
        this.loading = false;
        // Puedes manejar el error aquí, mostrando un mensaje de error
      }
    );
  }

  abrirConsultaMedica(id: number) {
    console.log(id);
    this.citaSeleccionada = this.citasMedicas.find(cita => cita.id === id);
    const documento = this.citaSeleccionada.paciente.documento; 
    window.open(`/medico/historia-clinica?documento=${documento}`, '_blank');
  }

  asignarEstado(id: number, estado: string): void {
    this.modalVerCita = false;
    this.citaSeleccionada = null;
    this.loading = true;
    this.citasMedicasService.editarEstado(id, estado).subscribe(() => {
      this.route.queryParams.subscribe(params => {
        const especialidad = params['especialidad'];
        this.especialidadSeleccionada = especialidad;
        const fechaVisible = params['fecha'];
        if (fechaVisible) {
          const [year, month, day] = fechaVisible.split('-').map(Number);
          this.fechaVisible = new Date(year, month - 1, day);
        }
        else {
          this.fechaVisible = new Date();
        }
      });
      this.cargarCitasMedicas(this.especialidadSeleccionada, this.fechaVisible).then(() => {
        this.loading = false;
      });
    }, error => {
      this.errorMessage = 'Error al actualizar el estado de la cita';
      console.error('Error al actualizar estado:', error);
      this.loading = false;
    });
  }

  actualizarFecha(event: Event): void {
    this.loading = true;
    const input = event.target as HTMLInputElement;
    const fechaSeleccionada = input.value;
    
    // Actualiza la URL con la fecha seleccionada
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { fecha: fechaSeleccionada, especialidad: this.especialidadSeleccionada },
      queryParamsHandling: 'merge' // Para mantener otros parámetros de la URL
    });

    this.route.queryParams.subscribe(params => {
      const especialidad = params['especialidad'];
      this.especialidadSeleccionada = especialidad;
      const fechaVisible = fechaSeleccionada;
      if (fechaVisible) {
        const [year, month, day] = fechaVisible.split('-').map(Number);
        this.fechaVisible = new Date(year, month - 1, day);
      }
      else {
        this.fechaVisible = new Date();
      }
    });
    this.cargarCitasMedicas(this.especialidadSeleccionada, this.fechaVisible).then(() => {
      this.loading = false;
    });
  }
}
