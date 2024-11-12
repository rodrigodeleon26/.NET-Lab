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
  DatosCalendarioForm: FormGroup;

  loading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';
  errorModalMessage: string = '';
  viendoCalendarios: boolean = true;
  isModalNuevoCalendarioVisible: boolean = false;

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

    this.DatosCalendarioForm = this.fb.group({
      especialidadId: ['', Validators.required],
      consultorioId: ['', Validators.required],
      horaInicio: ['', Validators.required],
      horaFin: ['', Validators.required],
      tiempo: ['', [Validators.required, Validators.pattern('^[0-9]*$')]],
      cantidad: ['', [Validators.required, Validators.pattern('^[0-9]*$')]],
      dias: this.fb.array([]),
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

  showErrorModalMessage(message: string) {
    this.errorModalMessage = message;
    setTimeout(() => {
      this.errorModalMessage = '';
    }, 6000);
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

  onCheckboxDiasChange(event: any) {
    const diasArray: FormArray = this.DatosCalendarioForm.get('dias') as FormArray;

    if (event.target.checked) {
      diasArray.push(this.fb.control(event.target.value));
    } else {
      const index = diasArray.controls.findIndex(x => x.value === event.target.value);
      diasArray.removeAt(index);
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

  agregarCalendario(){
    this.errorModalMessage = '';
    console.log(this.DatosCalendarioForm.value);
    if(!this.DatosMedicoForm.value.especialidades.includes(parseInt(this.DatosCalendarioForm.value.especialidadId))){
      this.showErrorModalMessage('El médico no tiene la especialidad seleccionada');
      return;
    }
    if (!this.medicoId) {
      this.showErrorModalMessage('No se ha seleccionado un médico');
      return;
    }
    if (this.DatosCalendarioForm.value.horaInicio >= this.DatosCalendarioForm.value.horaFin){
      this.showErrorModalMessage('La hora de inicio debe ser menor a la hora de fin');
      return;
    }
    if (this.DatosCalendarioForm.value.tiempo <= 0) {
      this.showErrorModalMessage('El tiempo debe ser mayor a 0');
      return;
    }
    if (this.DatosCalendarioForm.value.cantidad <= 0) {
      this.showErrorModalMessage('La cantidad debe ser mayor a 0');
      return;
    }
    if (this.DatosCalendarioForm.value.dias.length === 0) {
      this.showErrorModalMessage('Seleccione al menos un día');
      return;
    }
    if (!this.DatosCalendarioForm.valid) {
      this.showErrorModalMessage('Por favor, complete todos los campos');
      return;
    }

    const Calendario: any = {
      MedicoId: this.medicoId,
      EspecialidadId: this.DatosCalendarioForm.value.especialidadId,
      ConsultorioId: this.DatosCalendarioForm.value.consultorioId,
      HoraInicio: this.DatosCalendarioForm.value.horaInicio,
      HoraFin: this.DatosCalendarioForm.value.horaFin,
      Tiempo: this.DatosCalendarioForm.value.tiempo,
      Cantidad: this.DatosCalendarioForm.value.cantidad,
      Dias: this.DatosCalendarioForm.value.dias,
    }

    //chequear conflictos con otros calendarios del medico
    const horaInicio = this.DatosCalendarioForm.value.horaInicio;
    const horaFin = this.DatosCalendarioForm.value.horaFin;
    const dias = this.DatosCalendarioForm.value.dias;
    const conflictos = this.calendariosDelMedico.filter((calendario: any) => {
      if (calendario.diasSemana.some((dia: string) => dias.includes(dia))) {
        console.log('conflicto de dias');
        if (horaInicio >= calendario.horaInicio && horaInicio < calendario.horaFin) {
          return true;
        }
        if (horaFin > calendario.horaInicio && horaFin <= calendario.horaFin) {
          return true;
        }
      }
      return false;
    });

    if (conflictos.length > 0) {
      this.showErrorModalMessage('El calendario se superpone con otro calendario');
      return;
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
