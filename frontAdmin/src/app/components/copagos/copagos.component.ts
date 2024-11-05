import { Component, OnInit } from '@angular/core';
import { CopagosService } from '../../services/copagos.service';
import { EspecialidadesService } from '../../services/especialidades.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PreciosService } from '../../services/precios.service';

@Component({
  selector: 'app-copagos',
  templateUrl: './copagos.component.html',
  styleUrl: './copagos.component.css'
})
export class CopagosComponent implements OnInit {
  DatosPrecioForm: FormGroup;
  loading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';
  isModalVisiblePrecios: boolean = false;
  today: string;

  selectedSeguroMedico: any = null;
  selectedSeguroCopagos: any[] = [];
  copagoVerDetalle: any = null;
  nuevosCopagos: any[] = [];

  especialidades: any[] = [];

  constructor(
    private CopagosService: CopagosService,
    private EspecialidadesService: EspecialidadesService,
    private PreciosService: PreciosService,
    private fb: FormBuilder
  ) { 
    this.DatosPrecioForm = this.fb.group({
      precioBase: ['', [Validators.required, Validators.min(0), Validators.pattern('^[0-9]+(\.[0-9]{1,2})?$')]],
      fechaInicio: ['', [Validators.required]]
    });

    const today = new Date();
    this.today = today.toISOString().split('T')[0]; // Formato YYYY-MM-DD
  }

  ngOnInit(): void {
    this.CopagosService.selectedSeguroMedico$.subscribe({
      next: (data) => {
        this.selectedSeguroMedico = data;
        this.nuevosCopagos = [];
      },
      error: (error) => {
        console.error(error);
      },
    });

    this.CopagosService.copagosDeSeguroMedico$.subscribe({
      next: (data) => {
        this.selectedSeguroCopagos = data;
      },
      error: (error) => {
        console.error(error);
      }
    });

    this.CopagosService.selectedArticulo$.subscribe({
      next: (data) => {
        if (data) {
          //crearle un nuevo copago con el articulo seleccionado
          this.nuevosCopagos.push({
            id: null,
            articulo: data,
            especialidad: null,
            precios: []
          });
          console.log('nuevosCopagos', this.nuevosCopagos);
        }
      },
      error: (error) => {
        console.error(error);
      }
    });

    this.EspecialidadesService.getEspecialidades().subscribe({
      next: (data) => {
        this.especialidades = data;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  getPrecioActual(precios: any[]): any | null {
    const today = new Date();
    const preciosValidos = precios.filter(precio => new Date(precio.fechaInicio) <= today);
    
    if (preciosValidos.length === 0) {
      return null; // no hay precios válidos para la fecha actual
    }
  
    const precioActual = preciosValidos
      .sort((a, b) => new Date(b.fechaInicio).getTime() - new Date(a.fechaInicio).getTime())[0];
    
    return precioActual ? precioActual : null;
  }

  showSuccessMessage(message: string) {
    this.successMessage = message;
      setTimeout(() => {
        this.successMessage = '';
      }, 3000);
  }

  showErrorMessage(message: string) {
    this.errorMessage = message;
    setTimeout(() => {
      this.errorMessage = '';
    }, 3000);
  }

  selectCopagoDetalle(copago: any) {
    this.copagoVerDetalle = copago;

    //ordenar las fechas de los precios de forma descendente
    this.copagoVerDetalle.precios = this.copagoVerDetalle.precios.sort((
      a: { fechaInicio: string | number | Date; }, 
      b: { fechaInicio: string | number | Date; }) => new Date(b.fechaInicio).getTime() - new Date(a.fechaInicio).getTime());
  }

  agregarPrecio(){
    if (this.DatosPrecioForm.invalid) {
      this.showErrorMessage('Formulario inválido');
      return;
    }
    const formPrecio = this.DatosPrecioForm.value;

    //chequear que la fecha no sea anterior a hoy
    const fechaInicio = new Date(formPrecio.fechaInicio);
    const today = new Date();
    if (fechaInicio < today) {
      this.showErrorMessage('La fecha de inicio no puede ser anterior a hoy');
      return;
    }
    

    //chequear que no haya otro precio con la misma fecha de inicio para ese copago
    const precios = this.copagoVerDetalle.precios;
    const precioExistente = precios.find((precio: { fechaInicio: any; }) => precio.fechaInicio === formPrecio.fechaInicio);
    if (precioExistente) {
      this.showErrorMessage('Ya existe un precio con la misma fecha de inicio');
      return;
    }

    //convertir el precio a un objeto que acepte el servicio con copago y seguro como objetos
    const nuevoPrecio = {
      precioBase: formPrecio.precioBase,
      fechaInicio: formPrecio.fechaInicio,
      copago: {
        id: this.copagoVerDetalle.id
      },
      seguroMedico: null,
    };

    console.log('agregarPrecio', nuevoPrecio);
    this.PreciosService.addPrecio(nuevoPrecio).subscribe({
      next: (data) => {
        this.showSuccessMessage('Precio agregado correctamente');
        this.DatosPrecioForm.reset();
      },
      error: (error) => {
        this.showErrorMessage('Error al agregar precio');
        console.error(error);
      },
      complete: () => {
        //agregar el precio a la lista del copago
        const copago = this.selectedSeguroCopagos.find(copago => copago.id === this.copagoVerDetalle.id);
        if (copago) {
          copago.precios.push(nuevoPrecio);
        }

      }
    });
  }

  borrarPrecio(id: number){
    //mostrar un alert
    if (!confirm('¿Está seguro de eliminar el precio?')) {
      console.log('id:' + id);
      return;
    }
    this.PreciosService.deletePrecio(id.toString()).subscribe({
      next: (data) => {
        this.showSuccessMessage('Precio eliminado correctamente');
      },
      error: (error) => {
        this.showErrorMessage('Error al eliminar precio');
        console.error(error);
      },
      complete: () => {
        //eliminar el precio de la lista del copago
        this.selectedSeguroCopagos.forEach(copago => {
          copago.precios = copago.precios.filter((precio: { id: number; }) => precio.id !== id);
        });
      }
    });
  }

  borrarNuevo(index: number){
    this.nuevosCopagos.splice(index, 1);
  }
}
