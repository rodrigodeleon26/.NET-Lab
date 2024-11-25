import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PacienteService } from '../../../services/paciente.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-mis-datos',
  templateUrl: './mis-datos.component.html',
  styleUrl: './mis-datos.component.css'
})
export class MisDatosComponent implements OnInit {
  cedula: string = '';  
  pacienteForm: FormGroup;
  vinculadoConGoogle: boolean = false;

  maxDate: string;

  errorMessage: string = '';

  loading = false;  

  constructor(
    private router: Router,
    private pacienteService: PacienteService,
    private fb: FormBuilder,
    private toastr: ToastrService,
    private route: ActivatedRoute
  ) {
    this.route.queryParams.subscribe(params => {
      this.cedula = params['cedula'] || '';
    });
    this.pacienteForm = this.fb.group({
      id: [''],
      documento: [{ value: '', disabled: true }, Validators.required],
      nombres: [{ value: '', disabled: true }, Validators.required],
      apellidos: [{ value: '', disabled: true }, Validators.required],
      email: ['', [Validators.required, Validators.email]], // Corregido: validadores síncronos
      telefono: [''],
      direccion: [''],
      fechaDeNacimiento: ['']
    });

    const today = new Date();
    this.maxDate = today.toISOString().split('T')[0];
  }

  ngOnInit(): void {
    this.loading = true;
    console.log('entre');
    this.pacienteService.obtenerMisDatos(this.cedula).subscribe(
      (response) => {
        this.pacienteForm.patchValue(response);
        if (response.googleToken !== null) {
          this.vinculadoConGoogle = true;
        }
        this.loading = false;
      },
      (error) => {
        if (
          error.error?.includes("No puedes ver la informacion de otro usuario") ||
          error.message?.includes("No puedes ver la informacion de otro usuario")
        ) {
          // Redirige a la ruta de inicio.
          this.router.navigate(['/inicio']);
        } else {
          // Manejo de otros errores (opcional).
          console.error("Ocurrió un error inesperado:", error);
        }
      }
    );
  }

  actualizarDatos(): void {
    if (this.pacienteForm.pristine) {
      return;
    }

    if (!this.pacienteForm.valid) {
      this.toastr.error('Por favor, completa los campos requeridos.');
      setTimeout(() => {
        this.errorMessage = '';
      }, 3000);
    } 
    
    this.loading = true;
    this.pacienteService.actualizarMisDatos(this.cedula, this.pacienteForm.getRawValue()).subscribe(
      (response) => {
        this.pacienteForm.patchValue(response);
        this.loading = false;
        this.toastr.success('Datos actualizados correctamente.');
      },
      (error) => {
        if (
          error.error?.includes("No puedes actualizar la informacion de otro paciente") ||
          error.message?.includes("No puedes actualizar la informacion de otro paciente")
        ) {
          // Redirige a la ruta de inicio.
          this.toastr.error('No puedes actualizar la informacion de otro paciente', 'Error');
          this.router.navigate(['/inicio']);
        } else {
          this.loading = false;
          this.toastr.error('Ocurrió un error al actualizar los datos.');
        }
      }
    );
  }

  vincularConGoogle(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const isChecked = inputElement.checked;
    const patientId = this.pacienteForm.get('id')?.value;
  
    if (isChecked) {
      // El checkbox estaba marcado
      const googleAuthUrl = `https://accounts.google.com/o/oauth2/auth?client_id=48134233839-ikthbqdo5edbjju2s0k0c90aab40n7f1.apps.googleusercontent.com&redirect_uri=https://localhost:5001/api/Pacientes/oauth2callback&response_type=code&scope=https://www.googleapis.com/auth/calendar.events&state=${patientId}`;
      window.location.href = googleAuthUrl;
    } else {
      
    }
  }
}
