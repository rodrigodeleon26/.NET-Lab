import { Component, OnInit } from '@angular/core';
import { PacienteService } from '../../../services/paciente.service';
import { CitasMedicasService } from '../../../services/cita-medica.service';
import { ConsultorioService } from '../../../services/consultorio.service';
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
  modalVerCita: boolean = false; // Modal de eliminación de cita
  citaSeleccionada: any = null;

  constructor(
    private citasMedicasService: CitasMedicasService,
    private pacienteService: PacienteService,
    private consultorioService: ConsultorioService,
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
    this.citasMedicasService.obtenerCitasMedicas().subscribe(
      (data) => {
        this.citasMedicas = data;

        this.loading = true;
  
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

        this.loading = false;

        console.log('Datos cargados en citasMedicas:', data); // Imprime los datos en la consola
      },
      (error) => {
        this.errorMessage = 'Error al cargar las citas médicas';
        console.error('Error al cargar citas:', error);
      }
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

  abrirConsultaMedica(id: number) {
    console.log(id);
    this.citaSeleccionada = this.citasMedicas.find(cita => cita.id === id);
    window.open(`/medico/consulta-medica?consultaSeleccionada=${this.citaSeleccionada.consultaMedicaId}`, '_blank');
  }
}
