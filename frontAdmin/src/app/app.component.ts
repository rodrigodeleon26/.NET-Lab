import { Component, OnInit } from '@angular/core';
import { PacientesService } from './services/pacientes.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'frontAdmin';
  pacientes: any[] = [];

  constructor(private pacientesService: PacientesService) {}

//   ngOnInit() {
//     this.loadPacientes();
//   }

//   loadPacientes() {
//     this.pacientesService.getPacientes().subscribe(
//       (data) => {
//         this.pacientes = data;
//         console.log('Pacientes cargados', this.pacientes);
//       },
//       (error) => {
//         console.error('Error al cargar los pacientes', error);
//       }
//     );
//   }
}
