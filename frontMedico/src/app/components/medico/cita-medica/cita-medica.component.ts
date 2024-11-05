import { Component, AfterViewInit, OnInit } from '@angular/core';
import { PacienteService } from '../../../services/paciente.service';
import { CitasMedicasService } from '../../../services/cita-medica.service';
import { ConsultorioService } from '../../../services/consultorio.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { forkJoin, lastValueFrom } from 'rxjs';

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
    this.loading = true;
    this.cargarCitasMedicas().then(() => {
      this.loading = false; // Se ejecuta solo después de que los datos hayan sido cargados
    });
  }
  
  // Cargar todas las citas médicas
  async cargarCitasMedicas(): Promise<void> {
    try {
      const data = await lastValueFrom(this.citasMedicasService.obtenerCitasMedicas()); // Convertir Observable a Promesa
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

  abrirConsultaMedica(id: number) {
    console.log(id);
    this.citaSeleccionada = this.citasMedicas.find(cita => cita.id === id);
    window.open(`/medico/consulta-medica?consultaSeleccionada=${this.citaSeleccionada.consultaMedicaId}`, '_blank');
  }
}
