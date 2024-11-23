import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PacienteService } from '../../../services/paciente.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../services/auth.service';

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
    private authService: AuthService
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
      fechaDeNacimiento: [''],
      dobleFactor: [this.authService.getTwoFactorEnabledStatus()] // Inicializa el control de doble factor de autenticación
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
      console.log('Formulario no válido:', this.pacienteForm);
      console.log('Errores del formulario:', this.pacienteForm.errors);
      Object.keys(this.pacienteForm.controls).forEach(key => {
        const controlErrors = this.pacienteForm.get(key)?.errors;
        if (controlErrors != null) {
          console.log(`Errores en el control ${key}:`, controlErrors);
        }
      });
  
      this.toastr.error('Por favor, completa los campos requeridos.');
      setTimeout(() => {
        this.errorMessage = '';
      }, 3000);
      return; // Asegúrate de salir de la función si el formulario no es válido
    }
  
    this.loading = true;
    const email = this.authService.getEmail();
    const currentTwoFactorStatus = this.authService.getTwoFactorEnabledStatus();
    const newTwoFactorStatus = this.pacienteForm.get('dobleFactor')?.value;
  
    const updateTwoFactorAuth = new Promise<void>((resolve, reject) => {
      if (newTwoFactorStatus !== currentTwoFactorStatus) {
        if (newTwoFactorStatus) {
          this.authService.enableTwoFactorAuth(email).subscribe({
            next: (response: any) => {
              this.authService.setTwoFactorAuthenticated(true);
              this.authService.saveToken(response.tokens.token, response.tokens.refreshToken);
              this.toastr.success(response.message, 'Éxito');
              resolve();
            },
            error: (error) => {
              this.toastr.error('Error al habilitar el doble factor de autenticación', 'Error');
              console.error('Error al habilitar el doble factor de autenticación:', error);
              reject(error);
            }
          });
        } else {
          this.authService.disableTwoFactorAuth(email).subscribe({
            next: (response: any) => {
              this.authService.setTwoFactorAuthenticated(false);
              this.authService.saveToken(response.tokens.token, response.tokens.refreshToken);
              this.toastr.success(response.message, 'Éxito');
              resolve();
            },
            error: (error) => {
              this.toastr.error('Error al deshabilitar el doble factor de autenticación', 'Error');
              console.error('Error al deshabilitar el doble factor de autenticación:', error);
              reject(error);
            }
          });
        }
      } else {
        resolve();
      }
    });
  
    updateTwoFactorAuth.then(() => {
      this.pacienteService.actualizarMisDatos(this.cedula, this.pacienteForm.getRawValue()).subscribe(
        (response) => {
          this.pacienteForm.patchValue(response);
          this.loading = false;
          this.toastr.success('Datos actualizados correctamente.');
        },
        (error) => {
          this.loading = false;
          this.toastr.error('Ocurrió un error al actualizar los datos.');
        }
      );
    }).catch((error) => {
      this.loading = false;
      console.error('Error al actualizar el doble factor de autenticación:', error);
    });
  }
}