import { Component, OnInit } from '@angular/core';
import { FacturaService } from '../../../services/factura.service';
import { PaypalService } from '../../../services/paypal.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-pago',
  templateUrl: './pago.component.html',
  styleUrl: './pago.component.css'
})
export class PagoComponent implements OnInit {
  facturasAgrupadas: any = {}; // Agrupación de facturas por PagoPayPalId
  loading: boolean = false;
  errorMessage: string = '';
  numPagina: number = 1; // Página actual
  fechaAsc: boolean = false; // Orden de fecha
  estaPago: boolean = false;
  showPagoDropdown: boolean = false;
  cedula: string = ''; // Cédula fija

  constructor(
    private facturasService: FacturaService,
    private paypalService: PaypalService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const navigation = history.state;
    this.cedula = navigation?.['cedula'] || '';
    this.cargarFacturas();
  }

  puedeRealizarPago(facturasAgrupadas: any, keyActual: string): boolean {
    // Obtener todas las claves del objeto agrupado
    const keys = Object.keys(facturasAgrupadas);
  
    // Encontrar la agrupación con la factura más antigua
    let keyMasAntigua: string | null = null;
    let fechaMasAntigua: Date | null = null;
  
    keys.forEach((key) => {
      const fechaPrimeraFactura = new Date(facturasAgrupadas[key].facturas[0]?.fecha);
      if (!fechaMasAntigua || fechaPrimeraFactura < fechaMasAntigua) {
        fechaMasAntigua = fechaPrimeraFactura;
        keyMasAntigua = key;
      }
    });
  
    // Permitir la selección solo si la clave actual coincide con la agrupación más antigua
    return keyActual === keyMasAntigua;
  }

  cargarFacturas(): void {
    this.loading = true;
    this.facturasService.getFacturasPaginadas(this.numPagina, this.fechaAsc, this.estaPago, this.cedula).subscribe(
      (data) => {
        this.agruparFacturas(data);
        this.loading = false;
      },
      (error) => {
        this.errorMessage = 'Error al cargar las facturas';
        console.error('Error al cargar facturas:', error);
        this.loading = false;
      }
    );
  }

  getKeys(obj: any): string[] {
    return Object.keys(obj);
  }

  agruparFacturas(facturas: any[]): void {
    this.facturasAgrupadas = {};
  
    facturas.forEach((factura) => {
      const key = factura.pagoPayPal?.id || 'sinPago';
  
      // Inicializar el grupo si no existe
      if (!this.facturasAgrupadas[key]) {
        this.facturasAgrupadas[key] = { facturas: [], montoTotal: 0 };
      }
  
      // Agregar la factura al grupo
      this.facturasAgrupadas[key].facturas.push(factura);
  
      // Sumar el monto de la factura al monto total del grupo
      this.facturasAgrupadas[key].montoTotal += factura.monto;
    });
  
    // Log de los montos totales agrupados
    console.log(this.facturasAgrupadas);
  }

  realizarPago(id: string, link: string): void {
    // Redirige al usuario al enlace de PayPal
    window.location.href = link;
  }

  obtenerFactura(facturasAgrupadas: any): void {
    // Extraer las IDs de las facturas agrupadas
    const facturaIds = facturasAgrupadas.facturas.map((factura: any) => factura.id);
  
    if (facturaIds.length === 0) {
      alert('No hay facturas disponibles para mostrar.');
      return;
    }
  
    this.facturasService.downloadFacturasPdf(facturaIds).subscribe(
      (response) => {
        const blob = new Blob([response], { type: 'application/pdf' });
        const url = window.URL.createObjectURL(blob);
        window.open(url, '_blank'); // Abre el PDF en una nueva pestaña/ventana
      },
      (error) => {
        console.error('Error al mostrar las facturas:', error);
      }
    );
  }

  cambiarPagina(incremento: number): void {
    this.numPagina += incremento;
    if (this.numPagina < 1) {
      this.numPagina = 1;
    }
    this.cargarFacturas();
  }

  cambiarOrdenFecha(): void {
    this.fechaAsc = !this.fechaAsc;
    this.cargarFacturas();
  }
}
