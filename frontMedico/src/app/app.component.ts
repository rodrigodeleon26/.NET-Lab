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

  constructor() {}

  ngOnInit() {
   
  }
}
