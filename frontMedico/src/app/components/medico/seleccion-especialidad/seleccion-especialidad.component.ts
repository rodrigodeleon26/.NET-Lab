import { Component, AfterViewInit, OnInit } from '@angular/core';
import { PacienteService } from '../../../services/paciente.service';
import { CitasMedicasService } from '../../../services/cita-medica.service';
import { ConsultaMedicaService } from '../../../services/consulta-medica.service';
import { MedicoService } from '../../../services/medico.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { forkJoin, lastValueFrom } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-seleccion-especialidad',
  templateUrl: './seleccion-especialidad.component.html',
  styleUrl: './seleccion-especialidad.component.css'
})
export class SeleccionEspecialidadComponent {
  loading: boolean = true; // Indicador de carga
  successMessage: string = ''; // Mensaje de éxito
  errorMessage: string = ''; // Mensaje de error
  especialidades: any[] = []; // Lista de especialidades

  constructor(
    private MedicoService: MedicoService,
    private fb: FormBuilder
  ) {
    
  }

  ngOnInit(): void {
    this.MedicoService.obtenerMedicoPorId(1).subscribe(medico => {
      console.log('Médico:', medico);
      this.especialidades = medico.especialidades;
      console.log('Especialidades:', this.especialidades);
      this.loading = false;
    }); 
  }

  seleccionarEspecialidad(especialidad: any): void {
    console.log("Especialidad seleccionada:", especialidad);
    // Aquí puedes añadir la lógica que necesites al seleccionar una especialidad
    window.open(`/medico/cita-medica?especialidad=${especialidad}&fecha=2024-11-06`, '_blank');
  }
}
