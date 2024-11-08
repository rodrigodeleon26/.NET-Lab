import { Component, OnInit } from '@angular/core';
import { EspecialidadesService } from '../../services/especialidades.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-especialidad',
  templateUrl: './add-especialidad.component.html',
  styleUrl: './add-especialidad.component.css'
})
export class AddEspecialidadComponent implements OnInit{
  DatosEspecialidadForm: FormGroup;

  loading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';

  especialidades: any[] = [];
  verDetallePara: number | null = null;
  especidadBorrarId: string | null = null;
  isModalVisible: boolean = false;
  editando: string | null = null;

  constructor( 
    private especialidadesService: EspecialidadesService,
    private fb: FormBuilder,
  ) { 
    this.DatosEspecialidadForm = this.fb.group({
      nombre: ['', [Validators.required, Validators.maxLength(50)]],
      descripcion: ['', [Validators.required, Validators.maxLength(200)]],
    });
  }

  ngOnInit(): void {
    this.especialidadesService.getEspecialidades().subscribe({
      next: (data) => {
        this.especialidades = data;
      },
      error: (error) => {
        console.error(error);
      }
    })
  }

  showSuccessMessage(message: string) {
    this.successMessage = message;
      setTimeout(() => {
        this.errorMessage = '';
      }, 3000);
  }

  showErrorMessage(message: string) {
    this.errorMessage = message;
    setTimeout(() => {
      this.errorMessage = '';
    }, 3000);
  }

  editarEspecialidad( id: string ){
    this.editando = id;
    const especilidad = this.especialidades.find( especialidad => especialidad.id === id);
    if(especilidad){
      this.DatosEspecialidadForm.patchValue(especilidad);
    }
  }

  onModalContainerClick(event: MouseEvent): void {
    if ((event.target as HTMLElement).classList.contains('fixed')) {
      this.isModalVisible = false;
      this.especidadBorrarId = null;
    }
  }

  borrarEspecialidad(){
    if(this.especidadBorrarId){
      this.especialidadesService.deleteEspecialidad(this.especidadBorrarId).subscribe({
        next: (data) => {
          this.showSuccessMessage('Especialidad eliminada correctamente');
          //reiniciar la lista de medicos
          this.especialidadesService.getEspecialidades()
            .subscribe({
              next: (data) => {
                this.especialidades = data;
              },
              error: (error) => {
                console.error(error);
              }
            });
        },
        error: (error) => {
          console.error(error);
          this.showErrorMessage('Error al eliminar la especialidad');
        }
      });
    }
    else{
      this.showErrorMessage('Error al eliminar la especialidad');
    }
    this.isModalVisible = false;
    this.especidadBorrarId = null;
  }

  agregarEspecialidad(){
    if(this.DatosEspecialidadForm.invalid){
      this.showErrorMessage('Ingrese todos los campos correctamente');
      return;
    }
    if(this.editando){
      this.editarEspecialidadSend();
      return;
    }
    this.loading = true;
    this.especialidadesService.addEspecialidad(this.DatosEspecialidadForm.value).subscribe({
      next: (data) => {
        this.showSuccessMessage('Especialidad agregada correctamente');
        //reiniciar la lista de especialidades
        this.especialidadesService.getEspecialidades()
          .subscribe({
            next: (data) => {
              this.especialidades = data;
            },
            error: (error) => {
              console.error(error);
            }
          });
      },
      error: (error) => {
        console.error(error);
        this.showErrorMessage('Error al agregar la especialidad');
      },
      complete: () => {
        this.DatosEspecialidadForm.reset();
        this.loading = false;
      }
    });
  }

  editarEspecialidadSend(){
    this.loading = true;
    if(this.editando){
      this.especialidadesService.updateEspecialidad(this.editando ,this.DatosEspecialidadForm.value).subscribe({
        next: (data) => {
          this.showSuccessMessage('Especialidad editada correctamente');
          //reiniciar la lista de especialidades
          this.especialidadesService.getEspecialidades()
            .subscribe({
              next: (data) => {
                this.especialidades = data;
              },
              error: (error) => {
                console.error(error);
              }
            });
        },
        error: (error) => {
          console.error(error);
          this.showErrorMessage('Error al editar la especialidad');
        },
        complete: () => {
          this.DatosEspecialidadForm.reset();
          this.loading = false;
          this.editando = null;
        }
      });
    }
  }
}
