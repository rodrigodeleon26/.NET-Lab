import { Component, OnInit } from '@angular/core';
import { ConsultaMedicaService } from '../../../services/consulta-medica.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-consulta-medica',
  templateUrl: './consulta-medica.component.html',
  styleUrl: './consulta-medica.component.css'
})
export class ConsultaMedicaComponent implements OnInit {
  consultaMedicaForm: FormGroup;
  consultaMedica: any = {};
  consultaMeciaDatos = false;

  medicamentos = [
    { id: 1, name: 'Medicamento 1' },
    { id: 2, name: 'Medicamento 2' },
    { id: 3, name: 'Medicamento 3' }
  ];

  recetaForm: FormGroup;
  modalReceta = false;
  modalEditarReceta = false;
  modalEliminarReceta = false;

  estudioForm: FormGroup;
  modalEstudio = false;
  modalEditarEstudio = false;
  modalEliminarEstudio = false;

  selectedTab: string = 'recetas'

  loading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';

  constructor(
    private consultaMedicaService: ConsultaMedicaService,
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
      id: [''],
      nombreMedicamento: ['', Validators.required],
      cantidad: ['', [Validators.required, Validators.min(1)]],
      frecuencia: ['', Validators.required],
      vencimiento: ['', Validators.required],
    });
    this.estudioForm = this.fb.group({
      id: [''],
      nombre: ['', Validators.required],
      descripcion: ['', Validators.required],
      fechaRealizado: [''],
      fechaResultado: [''],
      imagenUrl: [''],
    });
  }

  ngOnInit(): void {
    this.consultaMedicaService.obtenerConsultaMedica(3).subscribe(
      response => {
        this.consultaMedica = response;
        this.consultaMedicaForm.patchValue(this.consultaMedica);
        if (this.consultaMedica.descripcion && this.consultaMedica.diagnostico) {
          this.consultaMeciaDatos = true;
        } else {
          this.consultaMeciaDatos = false;
        }
      },
      error => {
        this.errorMessage = 'Error al obtener la consulta médica';
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
    this.recetaForm.patchValue(receta);
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

  agregarReceta() {
    this.modalReceta = false;
    if (this.recetaForm.invalid) {
      this.errorMessage = 'Debe completar todos los campos correctamente';
      setTimeout(() => {
          this.errorMessage = '';
      }, 3000);
      return;
    }
    this.loading = true;
    const recetaAgregar = {
      ...this.recetaForm.value,
      consultaMedicaId: this.consultaMedica.id 
    };
    this.consultaMedicaService.agregarReceta(this.consultaMedica.id, recetaAgregar).subscribe(
      response => {
        this.loading = false;
        this.consultaMedica = response; 
        this.recetaForm.reset();
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
    this.modalReceta = false;
    this.modalEditarReceta = false;
    if (this.recetaForm.invalid) {
        this.errorMessage = 'Debe completar todos los campos correctamente';
        setTimeout(() => {
            this.errorMessage = '';
        }, 3000);
        return;
    }
    this.loading = true;
    const recetaActualizada = this.recetaForm.value;
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
    this.modalEstudio = false;
    if (this.estudioForm.invalid) {
      this.errorMessage = 'Debe completar todos los campos correctamente';
      setTimeout(() => {
          this.errorMessage = '';
      }, 3000);
      return;
    }
    this.loading = true;
    const estudioAgregar = {
      ...this.estudioForm.value,
      consultaMedicaId: this.consultaMedica.id 
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
