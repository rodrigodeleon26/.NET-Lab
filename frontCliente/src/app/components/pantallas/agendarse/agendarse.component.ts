import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { EspecialidadesService } from '../../../services/especialidades.service';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CalendariosService } from '../../../services/calendarios.service';

interface CitaMedica {
  id: number;
  fecha: string;
  estado: string;
  calendarioId: number;
  consultaMedicaId: number | null;
  pacienteId: number | null;
  copagoId: number;
  horaInicio: string;
  horaFin: string;
}

@Component({
  selector: 'app-agendarse',
  templateUrl: './agendarse.component.html',
  styleUrl: './agendarse.component.css'
})
export class AgendarseComponent implements OnInit{
  cedula: string = '';
  today: string;
  diaBuscado: string = '';
  horaActual: string;
  
  buscarAgendaForm: FormGroup;
  
  especialidades: any[] = [];
  calendarios: any[] = [];

  calendarioSeleccionado: any;
  citaSeleccionada: any;

  loading: boolean = false;
  errorMessage: string = '';
  isModalVisible: boolean = false;

  constructor(
    private router: Router,
    private especialidadesService: EspecialidadesService,
    private calendariosService: CalendariosService,
    private toastr: ToastrService,
    private fb: FormBuilder
  ) { 
    const navigation = this.router.getCurrentNavigation();
    this.cedula = navigation?.extras.state?.['cedula'] || '';
    //dia actual en uruguay en formato yyyy-mm-dd para buscar agenda
    const uruguayTime = new Date().toLocaleString("en-US", { timeZone: "America/Montevideo" });
    this.today = new Date(uruguayTime).toISOString().split('T')[0];
    //hora en uruguay
    this.horaActual = new Date().toLocaleTimeString('en-US', { hour12: false, hour: '2-digit', minute: '2-digit', second: '2-digit' });

    this.horaActual = '10:30:00';

    this.buscarAgendaForm = this.fb.group({
      especialidadId: [0, [ Validators.required, Validators.min(1)]],
      fecha: ['', [ Validators.required]],
    });
  }

  ngOnInit(): void {
    this.loading = true;
    this.especialidadesService.getEspecialidades().subscribe({
      next: (especialidades) => {
        this.especialidades = especialidades;
        this.loading = false;
      },
      error: (error) => {
        console.error(error);
        this.loading = false;
      }
    });

    this.calendarios.forEach(calendario => {
      this.generarCitasParaLineaDeTiempo(calendario);
    });
  }

  BuscarAgenda(): void {
    if (!this.buscarAgendaForm.valid) {
      this.toastr.error('Por favor, completa todos los campos requeridos.');
      setTimeout(() => {
        this.errorMessage = '';
      }, 3000);
      return;
    } 

    this.loading = true;
    this.calendariosService.getCalendariosByEspecialidadFecha(this.buscarAgendaForm.value.especialidadId, this.buscarAgendaForm.value.fecha).subscribe({
      next: (calendariosRes) => {
        this.calendarios = [];
        calendariosRes.forEach(calendario => {
          this.generarCitasParaLineaDeTiempo(calendario);
        });
        this.diaBuscado = this.buscarAgendaForm.value.fecha;
        this.calendarios = calendariosRes;
        console.log(this.calendarios);
        this.loading = false;
      },
      error: (error) => {
        console.error(error);
        this.loading = false;
      }
    });
  }

  generarCitasParaLineaDeTiempo(calendario: any) {
    const { horaInicio, tiempoCita, cantidadCitas, citasMedicas } = calendario;

    // Convierte horaInicio a minutos desde las 00:00
    const [inicioHoras, inicioMinutos] = horaInicio.split(":").map(Number);
    const inicioTotalMinutos = inicioHoras * 60 + inicioMinutos;

    // Crea las citas basadas en los intervalos
    const citasActualizadas: CitaMedica[] = [];
    for (let i = 0; i < cantidadCitas; i++) {
      const citaInicioMinutos = inicioTotalMinutos + i * tiempoCita;
      const citaHoraInicio = `${Math.floor(citaInicioMinutos / 60)
        .toString()
        .padStart(2, "0")}:${(citaInicioMinutos % 60).toString().padStart(2, "0")}:00`;

      const citaHoraFinMinutos = citaInicioMinutos + tiempoCita;
      const citaHoraFin = `${Math.floor(citaHoraFinMinutos / 60)
        .toString()
        .padStart(2, "0")}:${(citaHoraFinMinutos % 60).toString().padStart(2, "0")}:00`;

      // Busca una cita existente para este cupo
      const citaExistente = citasMedicas.find((cita: { fecha: string | number | Date; }) => {
        const citaFecha = new Date(cita.fecha);
        return (
          citaFecha.getHours() === Math.floor(citaInicioMinutos / 60) &&
          citaFecha.getMinutes() === citaInicioMinutos % 60
        );
      });

      // Si existe, actualiza los horarios
      if (citaExistente) {
        citasActualizadas.push({
          ...citaExistente,
          fecha: `${new Date().toISOString().split("T")[0]}T${citaHoraInicio}`,
          horaInicio: citaHoraInicio,
          horaFin: citaHoraFin,
        });
      } else {
        // Si no existe, crea una nueva cita
        citasActualizadas.push({
          id: 0, // Nuevo id o generado en el backend
          fecha: `${new Date().toISOString().split("T")[0]}T${citaHoraInicio}`,
          estado: "Disponible",
          calendarioId: calendario.id,
          consultaMedicaId: null,
          pacienteId: null,
          copagoId: 0,
          horaInicio: citaHoraInicio,
          horaFin: citaHoraFin,
        });
      }
    }

    calendario.citasMedicas = citasActualizadas;
    console.log(calendario);
  }

  HoraMenorAHoraActual(hora: string): boolean {
    //chequear el dia del calendario
    if (this.diaBuscado !== this.today) {
      return false;
    }
    return parseInt(hora.split(':')[0]) < parseInt(this.horaActual.split(':')[0]);
  }

  selectParaAgenda(calendario: any, cita: any){
    if (cita.estado === 'Completada' || cita.estado === 'NoAsistida' || cita.estado === 'Agendada' || this.HoraMenorAHoraActual(cita.horaInicio)) {
      this.toastr.error('La hora seleccionada no está disponible.');
      return;
    }
    this.calendarioSeleccionado = calendario;
    this.citaSeleccionada = cita;
    this.isModalVisible = true;
  }

  cancelarAgenda(){
    this.isModalVisible = false;
    this.calendarioSeleccionado = null;
    this.citaSeleccionada = null;
  }

  confirmarAgenda(){
    console.log('Agendando cita');
    console.log(this.calendarioSeleccionado);
    console.log(this.citaSeleccionada);
  }
}
