import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CalendariosService } from '../../../services/calendarios.service';
import { ArticulosService } from '../../../services/articulos.service';
import { CitaMedicaService } from '../../../services/cita-medica.service';

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
  
  calendarios: any[] = [];
  articulos: any[] = [];

  calendarioSeleccionado: any;
  citaSeleccionada: any;

  loading: boolean = false;
  errorMessage: string = '';
  isModalVisible: boolean = false;

  constructor(
    private router: Router,
    private calendariosService: CalendariosService,
    private toastr: ToastrService,
    private fb: FormBuilder,
    private articulosService: ArticulosService,
    private citaMedicaService: CitaMedicaService
  ) { 
    const navigation = this.router.getCurrentNavigation();
    this.cedula = navigation?.extras.state?.['cedula'] || '';
    //dia actual en uruguay en formato yyyy-mm-dd para buscar agenda
    const uruguayTime = new Date().toLocaleString("en-US", { timeZone: "America/Montevideo" });
    this.today = new Date(uruguayTime).toISOString().split('T')[0];
    //hora en uruguay
    this.horaActual = new Date().toLocaleTimeString('en-US', { hour12: false, hour: '2-digit', minute: '2-digit', second: '2-digit' });

    //this.horaActual = '10:30:00';

    this.buscarAgendaForm = this.fb.group({
      articuloId: [0, [ Validators.required, Validators.min(1)]],
      fecha: ['', [ Validators.required]],
    });
  }

  ngOnInit(): void {
    this.loading = true;

    this.articulosService.getArticulosDelSeguro(this.cedula).subscribe({
      next: (articulos) => {
        console.log(articulos);
        this.articulos = articulos;
        this.loading = false;
      },
      error: (error) => {
        console.error(error);
      }
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
    this.calendariosService.getCalendariosByArticuloFecha(this.cedula ,this.buscarAgendaForm.value.articuloId, this.buscarAgendaForm.value.fecha).subscribe({
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
    this.loading = true;
    this.citaMedicaService.AgendarCita(this.cedula, this.calendarioSeleccionado, this.diaBuscado, this.citaSeleccionada.horaInicio, this.buscarAgendaForm.value.articuloId).subscribe({
      next: (cita) => {
        console.log(cita);
        this.toastr.success('Cita agendada correctamente.');
        this.isModalVisible = false;
        this.calendarioSeleccionado = null;
        this.citaSeleccionada = null;
        this.loading = false;
        //redireccionar al inicio
        this.router.navigate(['/']);
      },
      error: (error) => {
        console.error(error);
        const exceptionMessage = this.extractExceptionMessage(error.error);
        this.toastr.error('Ocurrió un error al agendar la cita.');
        this.toastr.error(exceptionMessage);
        this.loading = false;
      }
    });
  }

  extractExceptionMessage(error: string): string {
    const match = error.match(/System\.Exception: (.*?)(?:\r?\n| at )/);
    return match ? match[1] : 'Error desconocido';
  }
}
