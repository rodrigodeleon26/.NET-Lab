import { Component, OnInit } from '@angular/core';
import { FacturasService } from '../../services/factura.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-facturas',
  templateUrl: './facturas.component.html',
  styleUrls: ['./facturas.component.css']
})
export class FacturasComponent implements OnInit {
  facturas: any[] = [];
  loading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';
  numPagina: number = 1; // Página actual
  pacienteString: string = ''; // Filtro de búsqueda de paciente
  fechaAsc: boolean = false; // Orden de fecha
  estaPago: boolean | undefined = undefined;
  showPagoDropdown: boolean = false;
  hayMasResultados: boolean = true; // Indica si hay más resultados

  constructor(
    private facturasService: FacturasService,
    private route: ActivatedRoute // Inyección de ActivatedRoute
  ) { }

  ngOnInit(): void {
    // Lee los parámetros de la URL
    console.log("Entro al ngInit");
    this.route.queryParams.subscribe(params => {
      console.log("Entro al subscribe del init");
      this.pacienteString = params['pacienteString'] || this.pacienteString;
      this.fechaAsc = params['fechaAsc'] === 'true'; // Convierte el parámetro a booleano
      this.estaPago = params['estaPago'] === 'true' ? true : (params['estaPago'] === 'false' ? false : undefined);
      
      // Cargar las facturas con los parámetros iniciales
      this.cargarFacturas();
    });
  }

  cargarFacturas(): void {
    this.loading = true; // Mostrar indicador de carga
    console.log("Entro al ngInit");
    this.facturasService.getFacturasPaginadas(this.numPagina, this.pacienteString, this.fechaAsc, this.estaPago).subscribe(
      (data) => {
        console.log("Entro al getFacturas");
        this.facturas = data;
        this.hayMasResultados = data.length === 20;
        this.loading = false; // Ocultar indicador de carga
      },
      (error) => {
        this.errorMessage = 'Error al cargar las facturas';
        console.error('Error al cargar facturas:', error);
        this.loading = false; // Ocultar indicador de carga
      }
    );
  }

  // Cambia a la página anterior o siguiente
  cambiarPagina(incremento: number): void {
    this.numPagina += incremento;
    if (this.numPagina < 1) {
      this.numPagina = 1;
    }
    this.cargarFacturas();
  }

  // Cambiar el estado del filtro de pago
  cambiarFiltroPago(estado: boolean | undefined): void {
    this.estaPago = estado;
    this.cargarFacturas();
  }

  // Cambiar el orden de la fecha
  cambiarOrdenFecha(): void {
    this.fechaAsc = !this.fechaAsc;
    this.cargarFacturas();
  }

  // Actualizar filtro de paciente
  actualizarFiltroPaciente(nombre: string): void {
    this.pacienteString = nombre;
    this.cargarFacturas();
  }

  toggleOrdenFecha(): void {
    this.fechaAsc = !this.fechaAsc;
    this.cargarFacturas();
  }

  togglePagoDropdown(): void {
    this.showPagoDropdown = !this.showPagoDropdown;
  }

  setFiltroPago(estado: boolean | undefined): void {
    this.estaPago = estado;
    this.showPagoDropdown = false;
    this.cargarFacturas();
  }

  generarFactura(factura: any) {
    console.log(`Generando factura para: ${factura.paciente.nombres} ${factura.paciente.apellidos}, Monto: ${factura.monto} $`);
    // Aquí puedes agregar lógica adicional para manejar la generación de la factura real.
    this.facturasService.generarFacturaPdf(factura.id).subscribe(
      (data) => {
        const blob = new Blob([data], { type: 'application/pdf' });
        const url = window.URL.createObjectURL(blob);
        window.open(url);
      },
      (error) => {
        console.error('Error al generar factura:', error);
      }
    );
  }
}