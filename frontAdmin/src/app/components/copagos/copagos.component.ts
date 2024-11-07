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
  copagosViejosEditados: any[] = [];

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
    const uruguayTimeZone = 'America/Montevideo';
    this.today = today.toLocaleDateString('en-CA', { timeZone: uruguayTimeZone });
  }

  ngOnInit(): void {
    this.loading = true;
    console.log('today:' + this.today);
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
            valido: false,
            id: 0,
            articulo: data,
            especialidad: null,
            seguroMedico: this.selectedSeguroMedico,
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
    this.loading = false;
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
    const fechaString =  fechaInicio.toDateString();
    
    if (fechaString < this.today) {
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

  copagoValidoCheck(index: number){
    const copago = this.nuevosCopagos[index];
    copago.valido = copago.especialidad !== null && copago.precios.length > 0;
  }

  cadaCopagoValidoCheck(){
    return this.nuevosCopagos.every(copago => copago.valido);
  }

  especialidadCopagoNuevo(event: any, index: number){    
    const especialidadId = event.target.value;
    const copago = this.nuevosCopagos[index];
    copago.especialidad = this.especialidades.find(e => e.id === parseInt(especialidadId));
    this.copagoValidoCheck(index);
    console.log('copago', copago);
  }

  precioCopagoNuevo(event: any, index: number){
    const precioBase = event.target.value;
    const copago = this.nuevosCopagos[index];
    copago.precios = [{
      precioBase: parseFloat(precioBase),
      fechaInicio: this.today,
      SeguroMedico: null,
      copago: { id: 0 }
    }];
    this.copagoValidoCheck(index);
    console.log('copago', copago);
  }

  guardarCopagosNuevos(){
    if (!this.cadaCopagoValidoCheck()) {
      this.showErrorMessage('Todos los copagos deben tener una especialidad y un precio');
      return;
    }

    this.nuevosCopagos.forEach(copago => {
      this.CopagosService.addCopago(copago).subscribe({
        next: (data) => {
          this.showSuccessMessage('Copago agregado correctamente');
          this.nuevosCopagos = [];
        },
        error: (error) => {
          this.showErrorMessage('Error al agregar copago');
          console.error(error);
        },
        complete: () => {
          this.CopagosService.RefreshCopagos();
        }
      });
    });
  }

  borrarCopago(id: number){
    //mostrar un alert
    if (!confirm('¿Está seguro de eliminar el copago?')) {
      return;
    }
    this.CopagosService.deleteCopago(id).subscribe({
      next: (data) => {
        this.showSuccessMessage('Copago eliminado correctamente');
      },
      error: (error) => {
        this.showErrorMessage('Error al eliminar copago');
        console.error(error);
      },
      complete: () => {
        this.CopagosService.RefreshCopagos();
      }
    });
  }

  editandoCopago(copagoId: number){
    const copago = this.selectedSeguroCopagos.find(copago => copago.id === copagoId);
    
    //ingresar el copago o sacarlo de la lista de editados
    if (this.estaSiendoEditado(copagoId)) {
      this.copagosViejosEditados = this.copagosViejosEditados.filter(copago => copago.id !== copagoId);
    } else {
      this.copagosViejosEditados.push(copago);
    }
  }

  estaSiendoEditado(copagoId: number){
    return this.copagosViejosEditados.some(copago => copago.id === copagoId);
  }

  especialidadCopagoEdit(event: any, copagoIndex: number){
    const especialidadId = event.target.value;
    const copago = {
      id: this.selectedSeguroCopagos[copagoIndex].id,
      especialidad: this.especialidades.find(e => e.id === parseInt(especialidadId)),
      seguroMedico: this.selectedSeguroMedico,
      articulo: this.selectedSeguroCopagos[copagoIndex].articulo,
    }
    console.log('copago', copago);
    this.CopagosService.updateCopago(copago).subscribe({
      next: (data) => {
        this.showSuccessMessage('Especialidad actualizada correctamente');
      },
      error: (error) => {
        this.showErrorMessage('Error al actualizar especialidad');
        console.error(error);
      },
      complete: () => {
        this.copagosViejosEditados = this.copagosViejosEditados.filter(copago => copago.id !== copago.id);
        this.CopagosService.RefreshCopagos();
      }
    });
  }
}
