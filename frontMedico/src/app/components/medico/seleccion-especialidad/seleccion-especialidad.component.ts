import { Component, AfterViewInit, OnInit } from '@angular/core';
import { PacienteService } from '../../../services/paciente.service';
import { CitasMedicasService } from '../../../services/cita-medica.service';
import { ConsultaMedicaService } from '../../../services/consulta-medica.service';
import { MedicoService } from '../../../services/medico.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { forkJoin, lastValueFrom } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

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
  idDelMedico: number = 1;

  constructor(
    private MedicoService: MedicoService,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    
  }

  ngOnInit(): void {
    const claims = this.authService.getClaims();
    this.idDelMedico = claims ? claims.idDelMedico : null;
    this.MedicoService.obtenerMedicoPorId(this.idDelMedico).subscribe(response => {
      console.log(response);
      this.especialidades = response.especialidades;
      this.loading = false;
    }); 
  }

  seleccionarEspecialidad(especialidad: any): void {
    const hoy = new Date();
    const year = hoy.getFullYear();
    const month = (hoy.getMonth() + 1).toString().padStart(2, '0'); // Mes en formato 2 dígitos
    const day = hoy.getDate().toString().padStart(2, '0'); // Día en formato 2 dígitos
  
    const fechaHoy = `${year}-${month}-${day}`; // Formato YYYY-MM-DD
  
    // Abre la URL en una nueva ventana con la especialidad y la fecha de hoy
    window.location.href = `/medico/cita-medica?especialidad=${especialidad}&fecha=${fechaHoy}`;
  }
}
