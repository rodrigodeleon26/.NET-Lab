import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ConsultaMedicaService } from '../../../services/consulta-medica.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-consulta-medica',
  templateUrl: './consulta-medica.component.html',
  styleUrl: './consulta-medica.component.css'
})
export class ConsultaMedicaComponent implements OnInit {
  consultaMedicaForm: FormGroup;
  consultaMedica: any = {};
  consultaMeciaDatos = false;

  citaMedica: any = {};
  paciente: any = {};

  modalEliminarConsultaMedica = false;
  modalGuardarConsultaMedica = false;
  terminarConsulta = false;

  medicamentos: any = [];

  recetaForm: FormGroup;
  modalReceta = false;
  modalEditarReceta = false;
  modalEliminarReceta = false;

  estudioForm: FormGroup;
  modalEstudio = false;
  modalEditarEstudio = false;
  modalEliminarEstudio = false;

  today: string;

  selectedTab: string = 'recetas'

  loading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';

  consultaSeleccionadaUrl: any = null;

  constructor(
    private consultaMedicaService: ConsultaMedicaService,
    private route: ActivatedRoute,
    private fb: FormBuilder
  ) { 
    this.consultaMedicaForm = this.fb.group({
      id: [''],
      descripcion: ['', Validators.required],
      diagnostico: ['', Validators.required],
      citaMedicaId: [''],
      recetas: [''],
      estudios: [''],
    });
    this.recetaForm = this.fb.group({
      id: [0],
      nombreMedicamento: ['', Validators.required],
      cantidad: [
        '', 
        [Validators.required, Validators.pattern('^[1-9]\\d*$')] // Solo números positivos mayores que 0
      ],
      frecuencia: [
        '', 
        [Validators.required, Validators.pattern('^[1-9]\\d*$')] // Solo números positivos mayores que 0
      ],
      vencimiento: ['', Validators.required],
    });
    this.estudioForm = this.fb.group({
      id: [0],
      nombre: ['', Validators.required],
      descripcion: ['', Validators.required],
      fechaRealizado: ['', Validators.required],
      fechaResultado: [null],
      imagenUrl: [null],
    });
    const now = new Date();
    this.today = now.toISOString().split('T')[0]; // Formato yyyy-MM-dd
  }

  ngOnInit(): void {
    this.loading = true;
    this.route.queryParams.subscribe(params => {
      const consultaSeleccionada = params['consultaSeleccionada'];
      if (!consultaSeleccionada) return;
      this.consultaSeleccionadaUrl = consultaSeleccionada;
    });
    this.consultaMedicaService.obtenerConsultaMedica(this.consultaSeleccionadaUrl).subscribe(
      response => {
        this.consultaMedica = response.consultaMedica;
        this.consultaMedicaForm.patchValue(this.consultaMedica);
        if (this.consultaMedica.descripcion && this.consultaMedica.diagnostico) {
          this.consultaMeciaDatos = true;
        } else {
          this.consultaMeciaDatos = false;
        }
        this.citaMedica = response.citaMedica;    
        this.paciente = response.paciente;
        this.loading = false;
      },
      error => {
        this.errorMessage = 'Error al obtener la consulta médica';
        setTimeout(() => {
          this.errorMessage = '';
        }, 3000);
      }
    );
    this.consultaMedicaService.obtenerMedicamentos().subscribe(
      response => {
        this.medicamentos = response;
        console.log(this.medicamentos);
      },
      error => {
        this.errorMessage = 'Error al obtener los medicamentos';
        setTimeout(() => {
          this.errorMessage = '';
        }, 3000);
      }
    );
  }

  openModalReceta() {
    this.modalReceta = true;
  }
  closeModalReceta() {
    this.modalEditarReceta = false;
    this.modalReceta = false;
    this.recetaForm.reset();
  }
  openModalEditarReceta(receta: any) {
    const frecuencia = receta.frecuencia.match(/\d+/); 
    const frecuenciaNumero = frecuencia ? +frecuencia[0] : null; 
    this.recetaForm.patchValue({
        ...receta,
        frecuencia: frecuenciaNumero
    });
    this.modalEditarReceta = true;
    this.modalReceta = true;
  }
  openModalEliminarReceta(receta: any) {
    this.recetaForm.patchValue(receta);
    this.modalEliminarReceta = true;
  }
  closeModalEliminarReceta() {
    this.modalEliminarReceta = false;
    this.recetaForm.reset();
  }

  openModalEstudio() {
    this.modalEstudio = true;
  }
  closeModalEstudio() {
    this.modalEditarEstudio = false;
    this.modalEstudio = false;
    this.estudioForm.reset();
  }
  openModalEditarEstudio(estudio: any) {
    this.estudioForm.patchValue(estudio);
    this.modalEditarEstudio = true;
    this.modalEstudio = true
  }
  openModalEliminarEstudio(estudio: any) {
    this.estudioForm.patchValue(estudio);
    this.modalEliminarEstudio = true;
  }
  closeModalEliminarEstudio() {
    this.modalEliminarEstudio = false;
    this.estudioForm.reset();
  }

  openModalEliminarConsultaMedica() {
    this.modalEliminarConsultaMedica = true;
  }
  closeModalEliminarConsultaMedica() { 
    this.modalEliminarConsultaMedica = false;
  }

  openModalGuardarConsultaMedica() {
    this.modalGuardarConsultaMedica = true;
  }
  closeModalGuardarConsultaMedica() {
    this.modalGuardarConsultaMedica = false;
  }

  abrirHistorialClinico() {
    const documento = this.paciente.documento;
    console.log(documento);
    window.open(`/medico/historia-clinica?documento=${documento}`, '_blank');
  }

  guardarConsultaMedica() {
    if (this.consultaMedicaForm.invalid) {
      this.errorMessage = 'Debe completar todos los campos';
      setTimeout(() => {
        this.errorMessage = '';
      }, 3000);
      return;
    }
    this.loading = true;
    const consultaMedicaGuardar = this.consultaMedicaForm.value;
    this.consultaMedicaService.guardarConsultaMedica(consultaMedicaGuardar.id).subscribe(
      response => {
        this.loading = false;
        this.consultaMedica = response;
        this.consultaMedicaForm.patchValue(this.consultaMedica);
        this.consultaMeciaDatos = true;
        this.modalGuardarConsultaMedica = false;
        this.successMessage = 'Consulta médica guardada correctamente';
        setTimeout(() => {
          this.successMessage = '';
        }, 3000);
      },
      error => {
        this.loading = false;
        this.errorMessage = 'Error al guardar la consulta médica';
        setTimeout(() => {
          this.errorMessage = '';
        }, 3000);
      }
    );
  }

  actualizarCosultaMedica() {
    if (this.consultaMedicaForm.invalid) {
      this.errorMessage = 'Debe completar todos los campos';
      setTimeout(() => {
        this.errorMessage = '';
      }, 3000);
      return;
    }
    this.loading = true;
    const consultaMedicaActualizada = this.consultaMedicaForm.value;
    this.consultaMedicaService.actualizarCosultaMedica(consultaMedicaActualizada).subscribe(
      response => {
        this.loading = false;
        this.terminarConsulta = true;
        this.consultaMedica = response;
        this.consultaMedicaForm.patchValue(this.consultaMedica);
        this.consultaMeciaDatos = true;
        this.successMessage = 'Consulta médica actualizada correctamente';
        setTimeout(() => {
          this.successMessage = '';
        }, 3000);
      },
      error => {
        this.loading = false;
        this.errorMessage = 'Error al actualizar la consulta médica';
        setTimeout(() => {
          this.errorMessage = '';
        }, 3000);
      }
    );
  }

  eliminarConsultaMedica() {
    this.loading = true;
    this.consultaMedicaService.eliminarConsultaMedica(this.consultaMedica.id).subscribe(
      response => {
        this.loading = false;
        this.successMessage = 'Consulta médica eliminada correctamente';
        setTimeout(() => {
          this.successMessage = '';
        }, 3000);
      },
      error => {
        this.loading = false;
        this.errorMessage = 'Error al eliminar la consulta médica';
        setTimeout(() => {
          this.errorMessage = '';
        }, 3000);
      }
    );
  }

  agregarReceta() {
    if (this.recetaForm.invalid) {
      this.recetaForm.markAllAsTouched(); 
      return;
    }
    this.modalReceta = false;
    this.loading = true;
    const frecuencia = this.recetaForm.get('frecuencia')?.value;
    const frecuenciaString = `Cada ${frecuencia} horas`;
    const recetaAgregar = {
      ...this.recetaForm.value,
      consultaMedicaId: this.consultaMedica.id,
      frecuencia: frecuenciaString,
      id: 0
    };
    this.consultaMedicaService.agregarReceta(this.consultaMedica.id, recetaAgregar).subscribe(
      response => {
        this.loading = false;
        this.consultaMedica = response; 
        this.recetaForm.reset();
        console.log(this.recetaForm);
        this.successMessage = 'Receta agregada correctamente';
        setTimeout(() => {
          this.successMessage = '';
        }, 3000);
      },
      error => {
        this.loading = false;
        this.errorMessage = 'Error al agregar la receta';
        setTimeout(() => {
          this.errorMessage = '';
        }, 3000);
      }
    );
  }

  editarReceta() {
    if (this.recetaForm.invalid) {
      this.recetaForm.markAllAsTouched(); 
      return;
    }
    this.modalEditarReceta = false;
    this.modalReceta = false;
    this.loading = true;
    const frecuencia = this.recetaForm.get('frecuencia')?.value;
    const frecuenciaString = `Cada ${frecuencia} horas`;
    const recetaActualizada = {
        ...this.recetaForm.value,
        frecuencia: frecuenciaString 
    };
    this.consultaMedicaService.editarReceta(this.consultaMedica.id, recetaActualizada).subscribe(
        response => {
            this.loading = false;
            this.consultaMedica = response;
            this.recetaForm.reset();
            this.successMessage = 'Receta editada correctamente';
            setTimeout(() => {
                this.successMessage = '';
            }, 3000);
        },
        error => {
            this.loading = false;
            this.errorMessage = 'Error al editar la receta';
            setTimeout(() => {
                this.errorMessage = '';
            }, 3000);
        }
    );
  }

  eliminarReceta() {
    this.modalEliminarReceta = false;
    this.loading = true;
    const recetaId = this.recetaForm.value.id;
    this.consultaMedicaService.eliminarReceta(this.consultaMedica.id, recetaId).subscribe(
      response => {
        this.loading = false;
        this.consultaMedica = response;
        this.recetaForm.reset();
        this.successMessage = 'Receta eliminada correctamente';
        setTimeout(() => {
          this.successMessage = '';
        }, 3000);
      },
      error => {
        this.loading = false;
        this.errorMessage = 'Error al eliminar la receta';
        setTimeout(() => {
          this.errorMessage = '';
        }, 3000);
      }
    );
  }

  agregarEstudio() {
    if (this.estudioForm.invalid) {
      this.recetaForm.markAllAsTouched(); 
      return;
    }
    this.modalEstudio = false;
    this.loading = true;
    const estudioAgregar = {
      ...this.estudioForm.value,
      consultaMedicaId: this.consultaMedica.id,
      id: 0 
    };
    this.consultaMedicaService.agregarEstudio(this.consultaMedica.id, estudioAgregar).subscribe(
      response => {
        console.log(response);
        this.loading = false;
        this.consultaMedica = response;
        this.estudioForm.reset();    
        this.successMessage = 'Estudio agregado correctamente';
        setTimeout(() => {
          this.successMessage = '';
        }, 3000);
      },
      error => {
        console.log(error);
        this.loading = false;
        this.errorMessage = 'Error al agregar el estudio';
        setTimeout(() => {
          this.errorMessage = '';
        }, 3000);
      }
    );
  }

  editarEstudio() {
    this.modalEstudio = false;
    this.modalEditarEstudio = false;
    if (this.estudioForm.invalid) {
      this.errorMessage = 'Debe completar todos los campos correctamente';
      setTimeout(() => {
          this.errorMessage = '';
      }, 3000);
      return;
    }
    this.loading = true;
    const estudioActualizado = this.estudioForm.value;
    this.consultaMedicaService.editarEstudio(this.consultaMedica.id, estudioActualizado).subscribe(
      response => {
        console.log(response);
        this.loading = false;
        this.consultaMedica = response;
        this.estudioForm.reset();    
        this.successMessage = 'Estudio editado correctamente';
        setTimeout(() => {
          this.successMessage = '';
        }, 3000);
      },
      error => {
        console.log(error);
        this.loading = false;
        this.errorMessage = 'Error al editar el estudio';
        setTimeout(() => {
          this.errorMessage = '';
        }, 3000);
      }
    );
  }


  eliminarEstudio() {
    this.modalEliminarEstudio = false;
    this.loading = true;
    const estudioId = this.estudioForm.value.id;
    this.consultaMedicaService.eliminarEstudio(this.consultaMedica.id, estudioId).subscribe(
      response => {
        this.loading = false;
        this.consultaMedica = response;
        this.estudioForm.reset();    
        this.successMessage = 'Estudio eliminado correctamente';
        setTimeout(() => {
          this.successMessage = '';
        }, 3000);
      },
      error => {
        this.loading = false;
        this.errorMessage = 'Error al eliminar el estudio';
        setTimeout(() => {
          this.errorMessage = '';
        }, 3000);
      }
    );
  }
}
