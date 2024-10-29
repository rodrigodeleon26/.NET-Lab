import { Component, OnInit } from '@angular/core';
import { PacienteService } from '../services/paciente.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-prueba-imagen',
  templateUrl: './prueba-imagen.component.html',
  styleUrl: './prueba-imagen.component.css'
})
export class PruebaImagenComponent implements OnInit {
  consultaMedica: any = {};
  consultaMedicaId: number = 3;

  constructor(
    private pacienteService: PacienteService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.pacienteService.getPacientes(this.consultaMedicaId).subscribe((data: any) => {
      this.consultaMedica = data;
      console.log(this.consultaMedica);
    });
  }

}
