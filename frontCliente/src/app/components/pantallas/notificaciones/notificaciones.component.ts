import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PacienteService } from '../../../services/paciente.service';
import { not } from 'rxjs/internal/util/not';

@Component({
  selector: 'app-notificaciones',
  templateUrl: './notificaciones.component.html',
  styleUrl: './notificaciones.component.css'
})
export class NotificacionesComponent implements OnInit {
  cedula: string = '';

  notificaciones: any[] = [];
  notificacion: any = {};

  hayNotificacionAbierta = false;

  pageNumber = 1;
  pageSize = 5;
  pages: number[] = [];
  totalItems = 0;
  totalPages = 0;

  loading = false;

  constructor(
    private router: Router,
    private pacienteService: PacienteService
  ) { 
    const navigation = this.router.getCurrentNavigation();
    this.cedula = navigation?.extras.state?.['cedula'] || '';
  }

  ngOnInit(): void {
    this.loading = true;
    this.pacienteService.obtenerNotificaciones(this.cedula, this.pageNumber, this.pageSize)
      .subscribe((response: any) => {
        console.log('Notificaciones:', response);
        this.notificaciones = response.notificaciones;
        this.loading = false;
        this.totalPages = response.totalPages;
        this.updatePages();
      });
  }
  changePage(event: any) {
    this.pageNumber = event;
    this.ngOnInit();
  }
  updatePages() {
    const visiblePages = 7; // Cuántas páginas mostrar en total (3 a cada lado + actual)
    let startPage = Math.max(1, this.pageNumber - 3);
    let endPage = Math.min(this.totalPages, this.pageNumber + 3);

    // Ajuste en caso de estar al inicio o al final del rango de páginas
    if (this.pageNumber <= 3) {
      endPage = Math.min(visiblePages, this.totalPages);
    } else if (this.pageNumber + 3 >= this.totalPages) {
      startPage = Math.max(this.totalPages - visiblePages + 1, 1);
    }

    // Generar el array de páginas a mostrar
    this.pages = Array.from({ length: (endPage - startPage + 1) }, (_, i) => startPage + i);
  }

  verNotificacion(notificacion: any) {
    if (!notificacion.visto) {
      this.loading = true;
      notificacion.visto = true;
      this.pacienteService.verNotificacion(notificacion.id)
        .subscribe((response: any) => {
          this.notificacion = notificacion;
          this.loading = false;
        });
    }
    else {
      this.notificacion = notificacion;
    }
    this.hayNotificacionAbierta = true;
  }
  irAtras() {
    this.notificacion = {};
    this.hayNotificacionAbierta = false;
  }
}
