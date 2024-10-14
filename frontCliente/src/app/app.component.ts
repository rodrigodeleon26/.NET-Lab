import { Component } from '@angular/core';
import { PacienteService } from './services/paciente.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'frontCliente';
  pacientes: any[] = [];

  constructor(private pacienteService: PacienteService) {}

  ngOnInit() {
    this.loadPacientes();
    console.log('Holaaaaaaaaaaa');
  }

  loadPacientes() {
    this.pacienteService.getPacientes().subscribe(
      (data) => {
        this.pacientes = data;
        console.log('Pacientes cargados', this.pacientes);
      },
      (error) => {
        console.error('Error al cargar los pacientes', error);
      }
    );
  }
}
