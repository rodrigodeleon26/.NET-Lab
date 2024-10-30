import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EspecialidadesService } from '../../services/especialidades.service';
import { MedicosService } from '../../services/medicos.service';

@Component({
  selector: 'app-generar-medico',
  templateUrl: './generar-medico.component.html',
  styleUrl: './generar-medico.component.css'
})
export class GenerarMedicoComponent implements OnInit {
  DatosMedicoForm: FormGroup;

  loading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';

  especialidades: any[] = [];

  constructor(
    private fb: FormBuilder,
    private especialidadesService: EspecialidadesService,
    private medicosService: MedicosService,
  ) {
    this.DatosMedicoForm = this.fb.group({
      nombres: ['', Validators.required],
      apellidos: ['', Validators.required],
      documento: ['', Validators.required],
      telefono: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      especialidades: this.fb.array([]),
    });
  }

  ngOnInit(): void {
    
    this.especialidadesService.getEspecialidades()
      .subscribe({
        next: (data) => {
          console.log(data);
          this.especialidades = data;
        },
        error: (error) => {
          console.error(error);
        }
      });
    
    this.medicosService.getMedicos()
      .subscribe({
        next: (data) => {
          console.log(data);
        },
        error: (error) => {
          console.error(error);
        }
      });
  }

  showSuccessMessage(message: string) {
    this.successMessage = message;
      setTimeout(() => {
        this.successMessage = '';
      }, 3000);
  }

  showErrorMessage(message: string) {
    this.errorMessage = message;
    setTimeout(() => {
      this.errorMessage = '';
    }, 3000);
  }

  onCheckboxChange(event: any) {
    const especialidadesArray: FormArray = this.DatosMedicoForm.get('especialidades') as FormArray;

    if (event.target.checked) {
      especialidadesArray.push(this.fb.control(event.target.value));
    } else {
      const index = especialidadesArray.controls.findIndex(x => x.value === event.target.value);
      especialidadesArray.removeAt(index);
    }
  }

  agregarMedico(){
    if (this.DatosMedicoForm.valid) {
      const medico = {
        Nombres: this.DatosMedicoForm.value.nombres,
        Apellidos: this.DatosMedicoForm.value.apellidos,
        Documento: this.DatosMedicoForm.value.documento,
        Telefono: this.DatosMedicoForm.value.telefono,
        Email: this.DatosMedicoForm.value.email,
        //guardar especialidades como un arreglo de objetos con el id de la especialidad
        Especialidades: this.DatosMedicoForm.value.especialidades.map((id: string) => ({ id })),
      }
      console.log(medico);
      this.medicosService.addMedico(medico)
        .subscribe({
          next: (data) => {
            console.log(data);
            this.showSuccessMessage('Médico agregado exitosamente');
            this.DatosMedicoForm.reset();
          },
          error: (error) => {
            console.error(error);
            const errorMessage = this.extractErrorMessage(error);
                    this.showErrorMessage(errorMessage);
          }
        });
    } else {
      this.showErrorMessage('Por favor, complete todos los campos');
    }
  }

  private extractErrorMessage(error: any): string {
    if (error && error.error && typeof error.error === 'string') {
        const match = error.error.match(/System\.Exception: (.+?)\n/);
        if (match && match[1]) {
            return match[1];
        }
    }
    return 'Ocurrió un error inesperado';
  }

}
