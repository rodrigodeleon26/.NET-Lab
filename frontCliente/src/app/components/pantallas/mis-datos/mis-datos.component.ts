import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PacienteService } from '../../../services/paciente.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../services/auth.service';
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
  dobleFactorHabilitado: boolean = false;
  qrCodeImage: string | null = null; 
  isModalVisibleQrCode: boolean = false;

  maxDate: string;

  errorMessage: string = '';

  loading = false;

  constructor(
    private router: Router,
    private pacienteService: PacienteService,
    private fb: FormBuilder,
    private toastr: ToastrService,
    private authService: AuthService,
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
      fechaDeNacimiento: [''],
    });

    const today = new Date();
    this.maxDate = today.toISOString().split('T')[0];
    this.dobleFactorHabilitado = this.authService.getTwoFactorEnabledStatus();
  }

  ngOnInit(): void {
    this.loading = true;
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

    this.pacienteService.actualizarMisDatos(this.cedula, this.pacienteForm.getRawValue()).subscribe(
      (response) => {
        this.pacienteForm.patchValue(response);
        this.loading = false;
        this.toastr.success('Datos actualizados correctamente.');
      },
      (error) => {
        if (error.error?.includes("No puedes actualizar la informacion de otro paciente")) {
          this.toastr.error('No puedes actualizar la información de otro paciente', 'Error');
          this.router.navigate(['/inicio']);
        } else {
          this.loading = false;
          this.toastr.error('Ocurrió un error al actualizar los datos.');
        }
      }
    );
  }

  agregar2FA(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const isChecked = inputElement.checked;
    const email = this.authService.getEmail();
    
    if (isChecked) {
      this.loading = true;
      this.authService.enableTwoFactorAuth(email).subscribe(
        (response: any) => {
          this.dobleFactorHabilitado = true;
          this.authService.saveToken(response.tokens.token, response.tokens.refreshToken);
          this.qrCodeImage = response.qrCodeImage;
          this.loading = false;
          this.toastr.success('Doble factor de autenticación habilitado.');
          this.isModalVisibleQrCode = true; // Mostrar el modal
        },
        (error) => {
          this.loading = false;
          this.toastr.error('Ocurrió un error al habilitar el doble factor de autenticación.');
        }
      );
    } else {
      this.loading = true;
      this.authService.disableTwoFactorAuth(email).subscribe(
        (response: any) => {
          this.dobleFactorHabilitado = false;
          this.authService.saveToken(response.tokens.token, response.tokens.refreshToken);
          this.loading = false;
          this.toastr.success('Doble factor de autenticación deshabilitado.');
        },
        (error) => {
          this.loading = false;
          this.toastr.error('Ocurrió un error al deshabilitar el doble factor de autenticación.');
        }
      );
    }
  }

  vincularConGoogle(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const isChecked = inputElement.checked;
    const patientId = this.pacienteForm.get('id')?.value;

    if (isChecked) {
      // El checkbox estaba marcado
      const googleAuthUrl = "https://accounts.google.com/o/oauth2/v2/auth?" +
        "scope=https://www.googleapis.com/auth/calendar.events&" +
        "access_type=offline&" +
        "include_granted_scopes=true&" +
        "response_type=code&" +
        "client_id=48134233839-ikthbqdo5edbjju2s0k0c90aab40n7f1.apps.googleusercontent.com&" +
        "redirect_uri=https://localhost:5001/api/Pacientes/oauth2callback&" +
        "state=" + patientId;
      console.log(googleAuthUrl);
      window.location.href = googleAuthUrl;
    } else {
      this.loading = true;
      this.pacienteService.desvincularConGoogle(patientId).subscribe(
        (response) => {
          this.vinculadoConGoogle = false;
          this.loading = false;
          this.toastr.success('Desvinculación exitosa.');
        },
        (error) => {
          this.loading = false;
          this.toastr.error('Ocurrió un error al desvincular con Google.');
        }
      );
    }
  }
}
