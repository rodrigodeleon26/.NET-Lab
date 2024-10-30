import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
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
      nombres: [''],
      apellidos: [''],
      documento: [''],
      telefono: [''],
      email: [''],
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
        this.errorMessage = '';
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
    console.log(this.DatosMedicoForm.value);

  }
}
