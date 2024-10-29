import { Component } from '@angular/core';
import { PacienteService } from './services/paciente.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  pacientes: any[] = [];
  pacienteId: number = 3;

  constructor(private pacienteService: PacienteService) {}

  ngOnInit() {
    this.loadPacientes();
  }

  loadPacientes() {
    this.pacienteService.getPacientes(this.pacienteId).subscribe(
      (data) => {
        this.pacientes = data || []; // Si no hay datos, usar un array vacío
        console.log('Pacientes cargados', this.pacientes);
      },
      (error) => {
        console.error('Error al cargar los pacientes', error);
      }
    );
  }
}
