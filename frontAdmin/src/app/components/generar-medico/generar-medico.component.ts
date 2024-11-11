import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EspecialidadesService } from '../../services/especialidades.service';
import { MedicosService } from '../../services/medicos.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ConsultoriosService } from '../../services/consultorios.service';
import { CalendariosService } from '../../services/calendarios.service';

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
  viendoCalendarios: boolean = true;

  especialidades: any[] = [];
  consultorios: any[] = [];
  calendariosDelMedico: any[] = [];
  medicoId: string | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private especialidadesService: EspecialidadesService,
    private medicosService: MedicosService,
    private consultoriosService: ConsultoriosService,
    private calendariosService: CalendariosService,
    private router: Router,
  ) {
    this.DatosMedicoForm = this.fb.group({
      nombres: ['', Validators.required],
      apellidos: ['', Validators.required],
      documento: ['', [Validators.required, Validators.pattern('^[0-9]*$')]],
      telefono: ['', [Validators.required, Validators.pattern('^[0-9]*$')]],
      email: ['', [Validators.required, Validators.email]],
      especialidades: this.fb.array([]),
    });
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.medicoId = params.get('id');
      if (this.medicoId) {
        // Lógica para cargar los datos del médico con la id proporcionada
        this.loadMedico(this.medicoId);
      }
    });

    this.especialidadesService.getEspecialidades()
      .subscribe({
        next: (data) => {
          this.especialidades = data;
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

  loadMedico(id: string) {
    // Implementa la lógica para cargar los datos del médico con la id proporcionada
    this.medicosService.getMedicoById(id).subscribe({
      next: (data) => {
        this.DatosMedicoForm.patchValue(data);
        console.log(data);
        data.especialidades.forEach((especialidad: any) => {
          this.onCheckboxChange({ target: { checked: true, value: especialidad.id } });
        });

      },
      error: (error) => {
        console.error(error);
      }
    });

    this.consultoriosService.getConsultorios().subscribe({
      next: (data) => {
        this.consultorios = data;
        console.log(data);
      },
      error: (error) => {
        console.error(error);
      }
    });

    this.calendariosService.getCalendariosByMedicoId(id).subscribe({
      next: (data) => {
        this.calendariosDelMedico = data;
        console.log(data);
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  onCheckboxChange(event: any) {
    const especialidadesArray: FormArray = this.DatosMedicoForm.get('especialidades') as FormArray;

    if (event.target.checked) {
      especialidadesArray.push(this.fb.control(parseInt((event.target.value), 10)));
    } else {
      
      const index = especialidadesArray.controls.findIndex(x => x.value === parseInt(event.target.value, 10));
      especialidadesArray.removeAt(index);
    }
  }

  agregarMedico(){
    if (this.DatosMedicoForm.valid) {
      const medico: any = {
        Nombres: this.DatosMedicoForm.value.nombres,
        Apellidos: this.DatosMedicoForm.value.apellidos,
        Documento: this.DatosMedicoForm.value.documento,
        Telefono: this.DatosMedicoForm.value.telefono,
        Email: this.DatosMedicoForm.value.email,
        //guardar especialidades como un arreglo de objetos con el id de la especialidad
        Especialidades: this.DatosMedicoForm.value.especialidades.map((id: string) => ({ id })),
      }

      if(this.medicoId){
        //editar medico
        medico['Id'] = this.medicoId;
        this.medicosService.updateMedico(this.medicoId, medico)
        .subscribe({
          next: (data) => {
            this.showSuccessMessage('Médico editado exitosamente');
            this.DatosMedicoForm.reset();
            this.medicoId = null;
            this.router.navigate(['/listMedicos']);
          },
          error: (error) => {
            console.error(error);
            const errorMessage = this.extractErrorMessage(error);
            this.showErrorMessage(errorMessage);
          }
        });
      }
      else{
        this.medicosService.addMedico(medico)
        .subscribe({
          next: (data) => {
            this.showSuccessMessage('Médico agregado exitosamente');
            this.DatosMedicoForm.reset();
          },
          error: (error) => {
            console.error(error);
            const errorMessage = this.extractErrorMessage(error);
            this.showErrorMessage(errorMessage);
          }
        });
      }

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
