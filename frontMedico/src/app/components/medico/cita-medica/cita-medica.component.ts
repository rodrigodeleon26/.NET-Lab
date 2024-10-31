import { Component, OnInit } from '@angular/core';
import { PacienteService } from '../../../services/paciente.service';
import { CitasMedicasService } from '../../../services/cita-medica.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-cita-medica',
  templateUrl: './cita-medica.component.html',
  styleUrls: ['./cita-medica.component.css']
})
export class CitaMedicaComponent implements OnInit {
  citasMedicas: any[] = []; // Lista de citas
  nuevaCitaForm: FormGroup;  // Formulario reactivo para nueva cita
  loading: boolean = false; // Indicador de carga
  successMessage: string = ''; // Mensaje de éxito
  errorMessage: string = ''; // Mensaje de error
  modalEliminarCita: boolean = false; // Modal de eliminación de cita
  modalCrearCita: boolean = false; // Modal de creación de cita
  citaIdEliminar: number | null = null; // ID de cita para eliminar

  constructor(
    private citasMedicasService: CitasMedicasService,
    private pacienteService: PacienteService,
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
    this.cargarCitasMedicas();
  }

  // Cargar todas las citas médicas
  cargarCitasMedicas(): void {
    this.loading = true;
    this.citasMedicasService.obtenerCitasMedicas().subscribe(
      (data) => {
        this.citasMedicas = data;
        this.loading = false;
  
        // Cargar información del paciente para cada cita médica
        this.citasMedicas.forEach((cita) => {
          if (cita.pacienteId) { // Verifica que la cita tiene un pacienteId
            this.pacienteService.getPacienteGestion(cita.pacienteId).subscribe(
              (pacienteData) => {
                cita.paciente = pacienteData; // Asigna la información del paciente a la cita médica
              },
              (error) => {
                console.error(`Error al cargar datos del paciente para la cita con ID ${cita.id}:`, error);
              }
            );
          }
        });

        console.log('Datos cargados en citasMedicas:', data); // Imprime los datos en la consola
      },
      (error) => {
        this.errorMessage = 'Error al cargar las citas médicas';
        console.error('Error al cargar citas:', error);
        this.loading = false;
      }
    );
  }

  getHora(fecha: string): string {
    const date = new Date(fecha);
    const horas = date.getHours().toString().padStart(2, '0');
    const minutos = date.getMinutes().toString().padStart(2, '0');
    return `${horas}:${minutos}`;
  }

  // Abrir el modal para crear una nueva cita
  openModalCrearCita(): void {
    this.modalCrearCita = true;
    this.nuevaCitaForm.reset();
  }

  // Cerrar el modal de creación de cita
  closeModalCrearCita(): void {
    this.modalCrearCita = false;
    this.nuevaCitaForm.reset();
  }

  // Crear una nueva cita médica
  crearCita(): void {
    if (this.nuevaCitaForm.invalid) return;

    const nuevaCita = this.nuevaCitaForm.value;
    const calendarioId = 1; // Cambiar según el contexto
    const pacienteId = 1;   // Cambiar según el contexto

    this.citasMedicasService.crearCitaMedica(calendarioId, pacienteId, nuevaCita).subscribe(
      (data) => {
        this.citasMedicas.push(data);
        this.successMessage = 'Cita médica creada con éxito';
        this.closeModalCrearCita();
        setTimeout(() => (this.successMessage = ''), 3000);
      },
      (error) => {
        this.errorMessage = 'Error al crear la cita médica';
        console.error('Error al crear cita:', error);
        setTimeout(() => (this.errorMessage = ''), 3000);
      }
    );
  }

  // Abrir el modal de eliminación y guardar el ID de la cita a eliminar
  openModalEliminarCita(id: number): void {
    this.modalEliminarCita = true;
    this.citaIdEliminar = id;
  }

  // Cerrar el modal de eliminación
  closeModalEliminarCita(): void {
    this.modalEliminarCita = false;
    this.citaIdEliminar = null;
  }

  // Eliminar una cita médica
  eliminarCita(): void {
    if (!this.citaIdEliminar) return;

    this.citasMedicasService.eliminarCitaMedica(this.citaIdEliminar).subscribe(
      () => {
        this.citasMedicas = this.citasMedicas.filter(cita => cita.id !== this.citaIdEliminar);
        this.successMessage = 'Cita médica eliminada con éxito';
        this.closeModalEliminarCita();
        setTimeout(() => (this.successMessage = ''), 3000);
      },
      (error) => {
        this.errorMessage = 'Error al eliminar la cita médica';
        console.error('Error al eliminar cita:', error);
        setTimeout(() => (this.errorMessage = ''), 3000);
      }
    );
  }

  // Ver detalles de una cita médica (por implementar)
  verCita(id: number): void {
    console.log('Ver cita con ID:', id);
    // Aquí se puede redirigir o abrir modal con detalles
  }

  // Editar una cita médica (por implementar)
  editarCita(id: number): void {
    console.log('Editar cita con ID:', id);
    // Aquí se puede redirigir o abrir modal con formulario de edición
  }
}
