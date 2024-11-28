import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PacienteService } from '../../../services/paciente.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-citas',
  templateUrl: './citas.component.html',
  styleUrl: './citas.component.css'
})
export class CitasComponent implements OnInit {
  cedula: string = '';

  citas: any[] = [];
  citaId: number = 0;

  modalCancelarCita: boolean = false;

  loading = false;

  constructor(
    private router: Router,
    private pacienteService: PacienteService,
    private toastr: ToastrService
  ) { 
    const navigation = this.router.getCurrentNavigation();
    this.cedula = navigation?.extras.state?.['cedula'] || '';
  }

  ngOnInit(): void {
    this.loading = true;
    this.pacienteService.obtenerCitas(this.cedula).subscribe(
      (response) => {
        this.citas = response;
        this.loading = false;
      },
      (error) => {
        if (
          error.error?.includes("No puedes acceder a las citas de otro paciente.") ||
          error.message?.includes("No puedes acceder a las citas de otro paciente.")
        ) {
          // Redirige a la ruta de inicio.
          this.toastr.error('No puedes acceder a las citas de otro paciente.', 'Error');
          this.router.navigate(['/inicio']);
        }
        else {
          this.loading = false;
          this.toastr.error(error.message, 'Error al obtener las citas');
        }
      }
    );
  }

  modalCancelar(citaId: number) {
    this.citaId = citaId;
    this.modalCancelarCita = true;
  }
  noCancelarCita() {
    console.log('No cancelar cita');
    this.citaId = 0;
    this.modalCancelarCita = false;
  }
  cancelarCita() {
    this.loading = true;
    this.pacienteService.cancelarCita(this.cedula, this.citaId).subscribe(
      (response) => {
        this.citas = this.citas.filter((cita) => cita.id !== this.citaId);
        this.modalCancelarCita = false;
        this.citaId = 0;
        this.loading = false;
        this.toastr.success('Cita cancelada correctamente', 'Cita cancelada');
      },
      (error) => {
        this.modalCancelarCita = false;
        this.citaId = 0;
        if (
          error.error?.includes("No puedes cancelar cita de otro paciente.") ||
          error.message?.includes("No puedes cancelar cita de otro paciente.")
        ) {
          this.loading = false;
          this.toastr.error('No puedes cancelar cita de otro paciente.', 'Error');
        } else {
          this.loading = false;
          this.toastr.error(error.message, 'Error al cancelar la cita');
        }
      }
    );
  }
}
