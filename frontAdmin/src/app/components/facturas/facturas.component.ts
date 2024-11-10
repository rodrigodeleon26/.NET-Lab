import { Component, OnInit } from '@angular/core';
import { FacturasService } from '../../services/factura.service';

@Component({
  selector: 'app-facturas',
  templateUrl: './facturas.component.html',
  styleUrls: ['./facturas.component.css']
})
export class FacturasComponent implements OnInit {
  facturas: any[] = [];
  loading: boolean = false;
  errorMessage: string = '';

  constructor(private facturasService: FacturasService) { }

  ngOnInit(): void {
    this.cargarFacturas();
  }

  cargarFacturas(): void {
    this.loading = true; // Mostrar indicador de carga
    this.facturasService.getFacturas().subscribe(
      (data) => {
        this.facturas = data;
        this.loading = false; // Ocultar indicador de carga
      },
      (error) => {
        this.errorMessage = 'Error al cargar las facturas';
        console.error('Error al cargar facturas:', error);
        this.loading = false; // Ocultar indicador de carga
      }
    );
  }
}