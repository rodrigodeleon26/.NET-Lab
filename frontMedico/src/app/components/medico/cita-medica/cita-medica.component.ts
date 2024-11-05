import { Component, AfterViewInit, OnInit } from '@angular/core';
import { PacienteService } from '../../../services/paciente.service';
import { CitasMedicasService } from '../../../services/cita-medica.service';
import { ConsultaMedicaService } from '../../../services/consulta-medica.service';
import { ConsultorioService } from '../../../services/consultorio.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { forkJoin, lastValueFrom } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

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

  constructor(
    private citasMedicasService: CitasMedicasService,
    private ConsultaMedicaService: ConsultaMedicaService,
    private pacienteService: PacienteService,
    private consultorioService: ConsultorioService,
    private route: ActivatedRoute,
    private fb: FormBuilder
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
    });
    this.cargarCitasMedicas(this.especialidadSeleccionada).then(() => {
      this.loading = false; // Se ejecuta solo después de que los datos hayan sido cargados
    });
  }
  
  // Cargar todas las citas médicas
  async cargarCitasMedicas(espec: string): Promise<void> {
    try {
      this.especialidadSeleccionada = 'Odontologia';
      const data = await lastValueFrom(this.citasMedicasService.obtenerCitasMedicasPorEspecialidad(espec)); // Convertir Observable a Promesa
      this.citasMedicas = data;
      console.log('Datos cargados en citasMedicas:', data); // Imprime los datos en la consola
    } catch (error) {
      this.errorMessage = 'Error al cargar las citas médicas';
      console.error('Error al cargar citas:', error);
    }
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
    const consultaData = {
      descripcion: " ",  // Puedes establecer valores iniciales o vacíos aquí
      diagnostico: " ",  // o incluso permitir que el usuario los complete
      citaMedicaId: this.citaSeleccionada.id
    };
    this.ConsultaMedicaService.crearConsulta(consultaData).subscribe(
      (response) => {
        console.log('Consulta creada exitosamente:', response);

        // Asigna la ID de la nueva consulta a citaSeleccionada.consultaMedicaId
        this.citaSeleccionada.consultaMedicaId = response.id;  // Asegúrate de que la respuesta contiene la ID
        this.citaSeleccionada.estado = 'Completada';  // Cambia el estado de la cita a 'Completada'
        this.citasMedicasService.actualizarCitaMedica(this.citaSeleccionada.id, this.citaSeleccionada).subscribe(() => {
        });

        /* this.citasMedicasService.editarEstado(id, 'Completada').subscribe(() => {
        }); */
        
        // Abre una nueva ventana con la ID de la consulta
        window.open(`/medico/consulta-medica?consultaSeleccionada=${this.citaSeleccionada.consultaMedicaId}`, '_blank');

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
    window.open(`/medico/consulta-medica?consultaSeleccionada=${this.citaSeleccionada.consultaMedicaId}`, '_blank');
  }

  asignarEstado(id: number, estado: string): void {
    this.modalVerCita = false;
    this.citaSeleccionada = null;
    this.loading = true;
    this.citasMedicasService.editarEstado(id, estado).subscribe(() => {
      this.route.queryParams.subscribe(params => {
        const especialidad = params['especialidad'];
        this.especialidadSeleccionada = especialidad;
      });
      this.cargarCitasMedicas(this.especialidadSeleccionada).then(() => {
        this.loading = false;
      });
    }, error => {
      this.errorMessage = 'Error al actualizar el estado de la cita';
      console.error('Error al actualizar estado:', error);
      this.loading = false;
    });
  }
}
