import { Component, OnInit } from '@angular/core';
import { PacientesService } from '../../services/pacientes.service';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { SegurosMedicosService } from '../../services/seguros-medicos.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-pacientes',
  templateUrl: './pacientes.component.html',
  styleUrl: './pacientes.component.css'
})
export class PacientesComponent implements OnInit {
  pacientes: any[] = [];
  segurosMedicos: any[] = []; // Lista de seguros médicos
  loading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';
  paginaActual: number = 1;
  busqueda: string = '';
  pacienteBorrarId: string | null = null;
  pacienteSeleccionado: any | null = null;
  pacienteForm!: FormGroup;
  modalTitle: string = '';
  isModalVisibleCrear: boolean = false;
  isModalVisibleActualizar: boolean = false;
  isModalVisibleVer: boolean = false;
  isModalVisibleConfirmarBorrado: boolean = false;
  isViewMode: boolean = false;
  pacienteParaBorrar: any | null = null;
  loadingModal: boolean = false;
  pacienteId: number | null = null; // Propiedad para almacenar el ID del paciente seleccionado

  constructor(
    private pacienteService: PacientesService,
    private fb: FormBuilder,
    private segurosMedicosService: SegurosMedicosService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.pacienteForm = this.fb.group({
      nombres: ['', Validators.required],
      apellidos: ['', Validators.required],
      documento: ['', [Validators.required, Validators.pattern(/^\d{8}$/)]],
      fechaDeNacimiento: [null, [Validators.required, this.fechaNacimientoValidator]],
      direccion: ['', Validators.required],
      telefono: ['', [Validators.required, Validators.pattern(/^\d{8,9}$/)]],
      email: ['', [Validators.required, Validators.email]],
      seguroMedicoId: ['', Validators.required]
    });

    this.getPacientes();
    this.getSegurosMedicos();
  }

  getPacientes(): void {
    this.loading = true;
    this.pacienteService.getPacientesFiltradosPaginados(1, '')
      .subscribe({
        next: (data) => {
          console.log(data);
          this.pacientes = data;
        },
        error: (error) => {
          console.error(error);
        },
        complete: () => {
          this.loading = false;
        }
      });
  }

  getSegurosMedicos(): void {
    this.segurosMedicosService.getSegurosMedicos().subscribe({
      next: (data) => {
        this.segurosMedicos = data;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  buscar(): void {
    console.log(this.busqueda);
    this.pacienteService.getPacientesFiltradosPaginados(1, this.busqueda).subscribe({
      next: (data) => {
        console.log(data);
        this.pacientes = data;
        this.paginaActual = 1;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  cambiarPagina(pagina: number): void {
    if (pagina < 1) {
      return;
    }
    this.pacienteService.getPacientesFiltradosPaginados(pagina, this.busqueda).subscribe({
      next: (data) => {
        if (!data || data.length === 0) {
          return;
        }
        this.pacientes = data;
        this.paginaActual = pagina;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  LimpiarBusqueda(): void {
    this.busqueda = '';
    this.pacienteService.getPacientesFiltradosPaginados(1, '').subscribe({
      next: (data) => {
        console.log(data);
        this.pacientes = data;
        this.paginaActual = 1;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  fechaNacimientoValidator(control: AbstractControl): ValidationErrors | null {
    const fecha = new Date(control.value);
    const hoy = new Date();
    const minFecha = new Date(1920, 0, 1);
    if (fecha > hoy || fecha < minFecha) {
      return { fechaInvalida: true };
    }
    return null;
  }

  abrirModalCrear(): void {
    this.pacienteForm.reset();
    this.modalTitle = 'Crear Paciente';
    this.isViewMode = false;
    this.pacienteForm.reset();
    this.enableFormControls();
    this.isModalVisibleCrear = true;
  }

  abrirModalActualizar(paciente: any): void {
    this.pacienteForm.reset();
    this.modalTitle = 'Actualizar Paciente';
    this.isViewMode = false;
    this.pacienteId = paciente.id;
    this.pacienteService.getPaciente(paciente.id).subscribe({
      next: (data: any) => {
        this.pacienteForm.patchValue({
          ...data,
          seguroMedicoId: data.contrato?.seguroMedico.id
        });
        this.enableFormControls();
        this.pacienteForm.get('seguroMedicoId')?.disable();
        this.isModalVisibleActualizar = true;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  abrirModalVer(paciente: any): void {
    this.pacienteForm.reset();
    this.modalTitle = 'Ver Paciente';
    this.isViewMode = true;
    this.pacienteService.getPaciente(paciente.id).subscribe({
      next: (data: any) => {
        this.pacienteForm.patchValue({
          ...data,
          seguroMedicoId: data.contrato?.seguroMedico.id
        });
        this.disableFormControls();
        this.isModalVisibleVer = true;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  abrirModalConfirmarBorrado(paciente: any): void {
    this.pacienteParaBorrar = paciente;
    this.isModalVisibleConfirmarBorrado = true;
  }

  enableFormControls(): void {
    Object.keys(this.pacienteForm.controls).forEach(controlName => {
      this.pacienteForm.get(controlName)?.enable();
    });
  }

  disableFormControls(): void {
    Object.keys(this.pacienteForm.controls).forEach(controlName => {
      this.pacienteForm.get(controlName)?.disable();
    });
  }

  onSubmit(): void {
    if (this.pacienteForm.valid){
      this.loadingModal = true;
      if (this.isModalVisibleCrear) {
        this.crearPaciente();
      }
      if (this.isModalVisibleActualizar) {
        this.editarPaciente();
      }
    }
  }

  crearPaciente(): void {
    const nuevoPaciente = this.pacienteForm.value;
    this.pacienteService.addPaciente(nuevoPaciente).subscribe({
      next: () => {
        this.getPacientes();
        this.isModalVisibleCrear = false;
        this.toastr.success('Paciente creado exitosamente');
        this.loadingModal = false;
      },
      error: (error) => {
        this.loadingModal = false;
        console.error(error);
        if (error.error && error.error.description) {
          this.toastr.error(error.error.description, error.error.code);
        } else {
          this.toastr.error('Error al crear el paciente');
        }
      }
    });
  }

  editarPaciente(): void {
    console.log(this.pacienteForm.value);
    const pacienteActualizado = this.pacienteForm.value;
    if (this.pacienteId !== null && pacienteActualizado) {
      this.pacienteService.updatePaciente(this.pacienteId, pacienteActualizado).subscribe({
        next: () => {
          this.getPacientes();
          this.isModalVisibleActualizar = false;
          this.toastr.success('Paciente actualizado exitosamente');
          this.loadingModal = false;
        },
        error: (error) => {
          this.loadingModal = false;
          console.error(error);
          if (error.error && error.error.description) {
            this.toastr.error(error.error.description, error.error.code);
          } else {
            this.toastr.error('Error al actualizar el paciente');
          }
        }
      });
    }
  }

  confirmarBorrado(): void {
    if (this.pacienteParaBorrar && this.pacienteParaBorrar.id) {
      this.loadingModal = true;
      this.pacienteService.deletePaciente(this.pacienteParaBorrar.id).subscribe({
        next: () => {
          this.getPacientes();
          this.isModalVisibleConfirmarBorrado = false;
          this.pacienteParaBorrar = null;
          this.toastr.success('Paciente eliminado exitosamente');
          this.loadingModal = false;
        },
        error: (error) => {
          this.loadingModal = false;
          console.error(error);
          if (error.error && error.error.description) {
            this.toastr.error(error.error.description, error.error.code);
          } else {
            this.toastr.error('Error al eliminar el paciente');
          }
        }
      });
    }
  }
}