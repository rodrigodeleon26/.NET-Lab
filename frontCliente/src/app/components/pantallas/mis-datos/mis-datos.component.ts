import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PacienteService } from '../../../services/paciente.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-mis-datos',
  templateUrl: './mis-datos.component.html',
  styleUrl: './mis-datos.component.css'
})
export class MisDatosComponent implements OnInit {
  cedula: string = '';  
  pacienteForm: FormGroup;

  maxDate: string;

  errorMessage: string = '';

  loading = false;  

  constructor(
    private router: Router,
    private pacienteService: PacienteService,
    private fb: FormBuilder,
    private toastr: ToastrService,
  ) {
    const navigation = this.router.getCurrentNavigation();
    this.cedula = navigation?.extras.state?.['cedula'] || '';
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
    this.pacienteService.obtenerMisDatos(this.cedula).subscribe(
      (response) => {
        this.pacienteForm.patchValue(response);
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
}
