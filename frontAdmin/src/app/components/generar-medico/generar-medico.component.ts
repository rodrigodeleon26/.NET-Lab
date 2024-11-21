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
  viendoCalendarios: boolean = false;
  isModalNuevoCalendarioVisible: boolean = false;
  isModalBorrarVisible: boolean = false;
  isModalAlertaEspVisible: boolean = false;
  mostrarEspecialidadSelect: boolean = false;
  mostrarDiaSelect: boolean = false;
  mostrarHoraSelect: boolean = false;
  ordenActualDeCalendariosEspecialidad: string = 'PorDefecto';
  ordenActualDeCalendariosDia: string = 'PorDefecto';
  ordenActualDeCalendariosHora: string = 'PorDefecto';

  especialidades: any[] = [];
  consultorios: any[] = [];
  calendariosDelMedico: any[] = [];
  calendariosShow: any[] = [];
  medicoId: string | null = null;
  calendarioABorrarId: number | null = null;
  calendarioEditId: number | null = null;
  medicoAlerta: any = null;

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
      horaFin: [{ value: '', disabled: true }, Validators.required],
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

    // update hora fin segun el tiempo, la cantidad y la hora de inicio
    this.DatosCalendarioForm.get('tiempo')?.valueChanges.subscribe(() => {
      this.updateHoraFin();
    });

    this.DatosCalendarioForm.get('cantidad')?.valueChanges.subscribe(() => {
      this.updateHoraFin();
    });

    this.DatosCalendarioForm.get('horaInicio')?.valueChanges.subscribe(() => {
      this.updateHoraFin();
    });
  }

  updateHoraFin() {
    const horaInicio = this.DatosCalendarioForm.get('horaInicio')?.value;
    const tiempo = this.DatosCalendarioForm.get('tiempo')?.value;
    const cantidad = this.DatosCalendarioForm.get('cantidad')?.value;

    if (horaInicio && tiempo && cantidad) {
      const [hours, minutes] = horaInicio.split(':').map(Number);
      const totalMinutes = (parseInt(tiempo, 10) * parseInt(cantidad, 10));
      const endDate = new Date();
      endDate.setHours(hours);
      endDate.setMinutes(minutes + totalMinutes);
      //no permitir combinaciones que permitan que endDate pase al siguiente dia
      if(endDate.getDate() > new Date().getDate()){
        this.DatosCalendarioForm.get('horaFin')?.setValue('', { emitEvent: false });
        this.showErrorModalMessage('La hora de fin no puede ser en el día siguiente');
        return;
      }

      const horaFin = `${String(endDate.getHours()).padStart(2, '0')}:${String(endDate.getMinutes()).padStart(2, '0')}`;
      this.DatosCalendarioForm.get('horaFin')?.setValue(horaFin, { emitEvent: false });
    }
    else{
      this.DatosCalendarioForm.get('horaFin')?.setValue('', { emitEvent: false });
    }
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

    let filtrosActuales = [
      'PorDefecto', //especialidad
      'PorDefecto', //dia
      'PorDefecto' //hora
    ]
    //this.calendariosService.getCalendariosByMedicoId(id).subscribe({
    this.calendariosService.getCalendariosFiltrados(id, filtrosActuales).subscribe({
      next: (data) => {
        this.calendariosDelMedico = data;
        this.calendariosShow = data;
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

      if(this.medicoId !== null){
        //editar medico
        medico['Id'] = this.medicoId;
        //verificar las especialidades para no sacar una que ya tenga calendarios
        this.calendariosService.validarEspecialidadesParaBorrar(this.medicoId, medico.Especialidades).subscribe({
          next: (data) => {
            if(data === true){
              console.log('Especialidades validadas, es seguro eliminar');
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
              console.log('Especialidades no validadas, no se puede eliminar');
              this.showErrorMessage('Hay calendarios para este medico que dependen de especialidades que serán eliminadas');
              this.isModalAlertaEspVisible = true;
              this.medicoAlerta = medico;
            }
          },
          error: (error) => {
            console.error(error);
            this.showErrorModalMessage(error);
          }
        });
        // this.medicosService.updateMedico(this.medicoId, medico)
        // .subscribe({
        //   next: (data) => {
        //     this.showSuccessMessage('Médico editado exitosamente');
        //     this.DatosMedicoForm.reset();
        //     this.medicoId = null;
        //     this.router.navigate(['/listMedicos']);
        //   },
        //   error: (error) => {
        //     console.error(error);
        //     const errorMessage = this.extractErrorMessage(error);
        //     this.showErrorMessage(errorMessage);
        //   }
        // });
      }
      else{
        //agregar medico
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

    this.DatosCalendarioForm.get('horaFin')?.enable();
    if (this.DatosCalendarioForm.value.horaInicio >= this.DatosCalendarioForm.value.horaFin){
      this.showErrorModalMessage('La hora de inicio debe ser menor a la hora de fin');
      return;
    }

    const Calendario: any = {
      Medico: {
        Id: this.medicoId
      },
      Especialidad: {
        Id: this.DatosCalendarioForm.value.especialidadId
      },
      Consultorio:{
        Id: this.DatosCalendarioForm.value.consultorioId
      },
      HoraInicio: this.DatosCalendarioForm.value.horaInicio,
      HoraFin: this.DatosCalendarioForm.value.horaFin,
      TiempoCita: this.DatosCalendarioForm.value.tiempo,
      CantidadCitas: this.DatosCalendarioForm.value.cantidad,
      DiasSemana: this.DatosCalendarioForm.value.dias,
    }
    this.DatosCalendarioForm.get('horaFin')?.disable();

    //si es editar le pongo la id
    if(this.calendarioEditId !== null){
      Calendario['Id'] = this.calendarioEditId;
    }

    //chequear conflictos con otros calendarios del medico
    const horaInicio = this.DatosCalendarioForm.value.horaInicio;
    const horaFin = this.DatosCalendarioForm.value.horaFin;
    const dias = this.DatosCalendarioForm.value.dias;
    let conflictos = [];
    
    if(this.calendarioEditId !== null){
      //en caso de editar, solo chequear los dias nuevos en caso de tener
      const calendarioComparacion = this.calendariosDelMedico.find((calendario) => calendario.id === this.calendarioEditId);
      if (calendarioComparacion) {
        const diasNuevos = dias.filter((dia: string) => !calendarioComparacion.diasSemana.includes(dia));

        conflictos = this.calendariosDelMedico.filter((calendario: any) => {
          if (diasNuevos.some((dia: string) => calendario.diasSemana.includes(dia))) {
            console.log('conflicto con calendario en los dias ' + calendario.diasSemana.join(', ') + ' para los dias ' + dias.join(', '));
            const horaInicioCalendario = new Date(`1970-01-01T${calendario.horaInicio}`).getTime();
            const horaFinCalendario = new Date(`1970-01-01T${calendario.horaFin}`).getTime();
            const horaInicioComparar = new Date(`1970-01-01T${horaInicio}`).getTime();
            const horaFinComparar = new Date(`1970-01-01T${horaFin}`).getTime();
        
            if ((horaInicioComparar > horaInicioCalendario && horaInicioComparar < horaFinCalendario) ||
                (horaFinComparar > horaInicioCalendario && horaFinComparar < horaFinCalendario) ||
                (horaInicioComparar <= horaInicioCalendario && horaFinComparar >= horaFinCalendario) ||
                (horaInicioComparar >= horaInicioCalendario && horaFinComparar <= horaFinCalendario)) {
              console.log('conflicto con calendario en las horas ' + calendario.horaInicio + ' - ' + calendario.horaFin + ' para la hora de inicio ' + horaInicio + ' y hora de fin ' + horaFin);
              return true;
            }
          }
        
          return false;
        });
      }
    }
    else{
      conflictos = this.calendariosDelMedico.filter((calendario: any) => {
        //el calendario es nuevo asi que chequear todos los dias
        if (calendario.diasSemana.some((dia: string) => dias.includes(dia))) {
          console.log('conflicto con calendario en los dias ' + calendario.diasSemana.join(', ') + ' para los dias ' + dias.join(', '));
          const horaInicioCalendario = new Date(`1970-01-01T${calendario.horaInicio}`).getTime();
          const horaFinCalendario = new Date(`1970-01-01T${calendario.horaFin}`).getTime();
          const horaInicioComparar = new Date(`1970-01-01T${horaInicio}`).getTime();
          const horaFinComparar = new Date(`1970-01-01T${horaFin}`).getTime();
      
          if ((horaInicioComparar > horaInicioCalendario && horaInicioComparar < horaFinCalendario) ||
              (horaFinComparar > horaInicioCalendario && horaFinComparar < horaFinCalendario) ||
              (horaInicioComparar <= horaInicioCalendario && horaFinComparar >= horaFinCalendario) ||
              (horaInicioComparar >= horaInicioCalendario && horaFinComparar <= horaFinCalendario)) {
            console.log('conflicto con calendario en las horas ' + calendario.horaInicio + ' - ' + calendario.horaFin + ' para la hora de inicio ' + horaInicio + ' y hora de fin ' + horaFin);
            return true;
          }
        }
      
        return false;
      });
    }

    if (conflictos.length > 0) {
      this.showErrorModalMessage('El calendario se superpone con otro calendario');
      return;
    }

    //chequear disponibilidad del consultorio
    //agregar :00 a las horas por compativilidad
    if(Calendario.HoraInicio.length === 5) Calendario.HoraInicio += ':00';
    if(Calendario.HoraFin.length === 5) Calendario.HoraFin += ':00';

    console.log(Calendario);
    this.calendariosService.checkCalendarioOcupado(Calendario).subscribe({
      next: (data) => {
        if(data == true){
          console.log('Consultorio ocupado');
          this.showErrorModalMessage('El consultorio está ocupado en el horario seleccionado');
        }
        else{
          console.log('Consultorio disponible');
          //divido segun si es agregar o editar
          if(this.calendarioEditId !== null){

            //editar el calendario
            Calendario['Id'] = this.calendarioEditId;
            this.calendariosService.updateCalendario(Calendario).subscribe({
              next: (data) => {
                this.showSuccessMessage('Calendario editado exitosamente');
                this.closeEditarModal();
                if (this.medicoId) {
                  let filtrosActuales = [
                    'PorDefecto', //especialidad
                    'PorDefecto', //dia
                    'PorDefecto' //hora
                  ]
                  //this.calendariosService.getCalendariosByMedicoId(this.medicoId).subscribe({
                  this.calendariosService.getCalendariosFiltrados(this.medicoId, filtrosActuales).subscribe({
                    next: (data) => {
                      this.calendariosDelMedico = data;
                      this.calendariosShow = data;
                      this.ordenarCalnedariosPorEspecialidad(this.ordenActualDeCalendariosEspecialidad);
                    },
                    error: (error) => {
                      console.error(error);
                    }
                  });
                }
              },
              error: (error) => {
                console.error(error);
                const errorMessage = this.extractErrorMessage(error);
                this.showErrorModalMessage(errorMessage);
              }
            });
          }
          else{

            //agregar el calendario
            this.calendariosService.addCalendario(Calendario).subscribe({
              next: (data) => {
                this.showSuccessMessage('Calendario agregado exitosamente');
                this.isModalNuevoCalendarioVisible = false;
                //remover todo del arreglo de dias
                const diasArray: FormArray = this.DatosCalendarioForm.get('dias') as FormArray;
                while (diasArray.length !== 0) {
                  diasArray.removeAt(0);
                }
                this.DatosCalendarioForm.reset();
                if (this.medicoId) {

                  let filtrosActuales = [
                    'PorDefecto', //especialidad
                    'PorDefecto', //dia
                    'PorDefecto' //hora
                  ]
                  //this.calendariosService.getCalendariosByMedicoId(this.medicoId).subscribe({
                  this.calendariosService.getCalendariosFiltrados(this.medicoId, filtrosActuales).subscribe({
                    next: (data) => {
                      this.calendariosDelMedico = data;
                      this.calendariosShow = data;
                      this.ordenarCalnedariosPorEspecialidad(this.ordenActualDeCalendariosEspecialidad);
                    },
                    error: (error) => {
                      console.error(error);
                    }
                  });
                }
                
              },
              error: (error) => {
                console.error(error);
                const errorMessage = this.extractErrorMessage(error);
                this.showErrorModalMessage(errorMessage);
              }
            });

          }
        }
      },
      error: (error) => {
        console.error(error);
        this.showErrorModalMessage(error);
        return;
      }
    })

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

  borrarCalendario(){
    console.log(this.calendarioABorrarId);
    if (this.calendarioABorrarId) {
      this.calendariosService.removeCalendario(this.calendarioABorrarId).subscribe({
        next: (data) => {
          this.showSuccessMessage('Calendario eliminado exitosamente');
          this.isModalBorrarVisible = false;
          this.calendariosDelMedico = this.calendariosDelMedico.filter((calendario) => calendario.id !== this.calendarioABorrarId);
          this.calendariosShow = this.calendariosShow.filter((calendario) => calendario.id !== this.calendarioABorrarId);
          this.calendarioABorrarId = null;
        },
        error: (error) => {
          console.error(error);
          const errorMessage = this.extractErrorMessage(error);
          this.showErrorModalMessage(errorMessage);
        }
      });
    }
  }

  selectCalendarioEdit(id: number){
    this.calendarioEditId = id;
    const calendario = this.calendariosDelMedico.find((calendario) => calendario.id === id);
    if (calendario) {
      this.DatosCalendarioForm.patchValue({
        especialidadId: calendario.especialidad.id,
        consultorioId: calendario.consultorio.id,
        horaInicio: calendario.horaInicio,
        horaFin: calendario.horaFin,
        tiempo: calendario.tiempoCita,
        cantidad: calendario.cantidadCitas,
        dias: calendario.diasSemana,
      });
    }

    //chequear los checkboxes de dias
    calendario.diasSemana.forEach((dia: any) => {
      this.onCheckboxDiasChange({ target: { checked: true, value: dia } });
    });

    this.isModalNuevoCalendarioVisible = true;


  }

  closeEditarModal(){
    this.calendarioEditId = null;
    this.DatosCalendarioForm.reset();
    this.isModalNuevoCalendarioVisible = false;
    //remover todo del arreglo de dias
    const diasArray: FormArray = this.DatosCalendarioForm.get('dias') as FormArray;
    while (diasArray.length !== 0) {
      diasArray.removeAt(0);
    }
  }

  ordenarCalnedariosPorEspecialidad(orden: any){
    console.log('Ordenar por:', orden);
    this.ordenActualDeCalendariosEspecialidad = orden;
    this.mostrarEspecialidadSelect = false;

    this.aplicarOrdenCalendarios();
  }

  ordenarCalendariosPorDia(dia: string){
    console.log('Ordenar por:', dia);
    this.ordenActualDeCalendariosDia = dia;
    this.mostrarDiaSelect = false;

    this.aplicarOrdenCalendarios();
  }
  
  ordenarCalendariosPorHora(orden: any){
    console.log('Ordenar por:', orden);
    this.ordenActualDeCalendariosHora = orden;
    this.mostrarHoraSelect = false;

    this.aplicarOrdenCalendarios();
  }

  aplicarOrdenCalendarios(){
    let calendarios = [...this.calendariosDelMedico];

    if(this.ordenActualDeCalendariosEspecialidad !== 'PorDefecto'){
      if(this.ordenActualDeCalendariosEspecialidad === 'Agrupar'){
        calendarios = calendarios.sort((a, b) => a.especialidad.nombre.localeCompare(b.especialidad.nombre));
      }
      else{
        calendarios = calendarios.filter((calendario) => calendario.especialidad.nombre === this.ordenActualDeCalendariosEspecialidad);  
      }
    }

    if(this.ordenActualDeCalendariosDia !== 'PorDefecto'){
      calendarios = calendarios.filter((calendario) => calendario.diasSemana.includes(this.ordenActualDeCalendariosDia));
    }

    if(this.ordenActualDeCalendariosHora !== 'PorDefecto'){
      if(this.ordenActualDeCalendariosHora === 'Ascendente'){
        calendarios = calendarios.sort((a, b) => a.horaInicio.localeCompare(b.horaInicio));
      }
      else{
        calendarios = calendarios.sort((a, b) => b.horaInicio.localeCompare(a.horaInicio));
      }
    }

    if(this.ordenActualDeCalendariosEspecialidad === 'PorDefecto' && this.ordenActualDeCalendariosDia === 'PorDefecto' && this.ordenActualDeCalendariosHora === 'PorDefecto'){
      console.log('Sin filtros');
      this.calendariosShow = this.calendariosDelMedico;
    }
    else{
      console.log('Filtrando por dia y especialidad');
      this.calendariosShow = calendarios;
    }
  }

  confirmarAlertaEspecialidad(){
    //primero borrar los calendarios que dependen de las especialidades que se borraran y luego actualizar
    if(this.medicoId === null || this.medicoAlerta === null){
      this.showErrorModalMessage('Error al resolver los conflictos con las especialidades');
      return;
    }
    this.calendariosService.BorrarCalendariosIncompatibles(this.medicoId, this.medicoAlerta.Especialidades).subscribe({
      next: (data) => {
        console.log('Calendarios borrados exitosamente' + data);


        //Actualizar Medico
        this.medicosService.updateMedico(this.medicoId, this.medicoAlerta)
        .subscribe({
          next: (data) => {
            this.showSuccessMessage('Médico editado exitosamente');
            this.DatosMedicoForm.reset();
            this.medicoId = null;
            this.medicoAlerta = null;
            this.isModalAlertaEspVisible = false;
            this.router.navigate(['/listMedicos']);
          },
          error: (error) => {
            console.error(error);
            const errorMessage = this.extractErrorMessage(error);
            this.showErrorMessage(errorMessage);
          }
        });

      },
      error: (error) => {
        console.error(error);
        this.showErrorModalMessage(error);
      }
    });

    
  }

}
