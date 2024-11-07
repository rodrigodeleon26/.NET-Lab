import { Component, OnInit } from '@angular/core';
import { SegurosMedicosService } from '../../services/seguros-medicos.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CopagosService } from '../../services/copagos.service';
import { PreciosService } from '../../services/precios.service';

@Component({
  selector: 'app-seguro-medico-select',
  templateUrl: './seguro-medico-select.component.html',
  styleUrl: './seguro-medico-select.component.css'
})
export class SeguroMedicoSelectComponent implements OnInit{
  DatosPrecioForm: FormGroup;
  DatosSMForm: FormGroup;

  SegurosMedicos: any[] = [];
  isModalVisible: boolean = false;
  isFormVisible: boolean = false;


  today: string;
  editando: any = '';
  seleccionado: number = 0;

  constructor(
    private fb: FormBuilder,
    private segurosMedicosService: SegurosMedicosService,
    private copagosService: CopagosService,
    private preciosService: PreciosService
  ) { 
    this.DatosSMForm = this.fb.group({
      nombre: ['', [Validators.required, Validators.maxLength(50)]],
      descripcion: ['', [Validators.required, Validators.maxLength(500)]],
    });

    this.DatosPrecioForm = this.fb.group({
      precioBase: ['', [Validators.required, Validators.min(0), Validators.pattern('^[0-9]+(\.[0-9]{1,2})?$')]],
      fechaInicio: ['', [Validators.required]]
    });

    const today = new Date();
    this.today = today.toISOString().split('T')[0]; // Formato YYYY-MM-DD
  }
  
  ngOnInit(): void {
    this.segurosMedicosService.getSegurosMedicos().subscribe({
      next: (data) => {
        this.SegurosMedicos = data;
      },
      error: (error) => {
        console.error(error);
      }
    });
    this.copagosService.refreshSeguro$.subscribe({
      next: () => {
        this.segurosMedicosService.getSeguroMedico(this.seleccionado).subscribe({
          next: (seguroMedico) => {
            this.copagosService.changeSelectedSeguroMedico(seguroMedico);
          },
          error: (error) => {
            console.error(error);
          }
        });
      }
    })
  }

  elegirParaEditar(smId: string){
    if (smId === '' || smId === null || smId == this.editando.id) {
      this.editando = '';
      this.DatosSMForm.reset();
      this.DatosPrecioForm.reset();
      return;
    }
    this.editando = this.SegurosMedicos.find(a => a.id === smId);
    this.DatosSMForm.patchValue(this.editando);
    this.DatosPrecioForm.reset();
    console.log(this.editando);
  }

  editar(): void {
    if (this.editando === '') {
      return;
    }
    console.log(this.editando.id, this.DatosSMForm.value);
    this.segurosMedicosService.updateSeguroMedico(this.editando.id, this.DatosSMForm.value).subscribe({
      next: (data) => {
        this.segurosMedicosService.getSegurosMedicos().subscribe({
          next: (data) => {
            this.SegurosMedicos = data;
          },
          error: (error) => {
            console.error(error);
          }
        });
        this.DatosSMForm.reset();
      },
      error: (error) => {
        console.error(error);
      },
      complete: () => {
        this.editando = '';
      }
    });
  }

  Delete(){
    if (this.editando === '') {
      return;
    }
    this.segurosMedicosService.deleteSeguroMedico(this.editando.id).subscribe({
      next: (data) => {
        this.segurosMedicosService.getSegurosMedicos().subscribe({
          next: (data) => {
            this.SegurosMedicos = data;
          },
          error: (error) => {
            console.error(error);
          }
        });
        this.DatosSMForm.reset();
      },
      error: (error) => {
        console.error(error);
      },
      complete: () => {
        this.editando = '';
      }
    });
  }

  agregarSeguroMedico(){
    if (this.DatosSMForm.invalid) {
      return;
    }
    if (this.editando !== '' && this.editando !== null && this.editando !== undefined && this.editando.id !== '') {
      return;
    }
    this.segurosMedicosService.addSeguroMedico(this.DatosSMForm.value).subscribe({
      next: (data) => {
        this.segurosMedicosService.getSegurosMedicos().subscribe({
          next: (data) => {
            this.SegurosMedicos = data;
          },
          error: (error) => {
            console.error(error);
          }
        });
        this.DatosSMForm.reset();
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  onSeguroMedicoChange(event: any){
    const seguroId = event.target.value;
    this.seleccionado = seguroId;
    this.segurosMedicosService.getSeguroMedico(seguroId).subscribe({
      next: (seguroMedico) => {
        this.copagosService.changeSelectedSeguroMedico(seguroMedico);
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

  borrarPrecio(precioId: string){
    if(this.editando === '') {
      return;
    }
    if (this.editando.precios.filter((p: { id: string; }) => p.id !== precioId).length === 0) {
      return;
    }
    if (!confirm('¿Está seguro de eliminar el precio?')) {
      return;
    }

    this.preciosService.deletePrecio(precioId).subscribe({
      next: (data) => {
        this.copagosService.selectedSeguroMedico$.subscribe({
          next: (data) => {
            this.editando.precios = this.editando.precios.filter((p: { id: string; }) => p.id !== precioId);
            this.segurosMedicosService.getSegurosMedicos().subscribe({
              next: (data) => {
                this.SegurosMedicos = data;
              },
              error: (error) => {
                console.error(error);
              }
            });
          },
          error: (error) => {
            console.error(error);
          }
        });
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  agregarPrecio(){
    if (this.editando === '') {
      return;
    }
    if (this.DatosPrecioForm.invalid) {
      return;
    }

    const formPrecio = this.DatosPrecioForm.value;

    //chequear que la fecha no sea anterior a hoy
    const fechaInicio = new Date(formPrecio.fechaInicio);
    const today = new Date();
    if (fechaInicio < today) {
      return;
    }

    const precios = this.editando.precios;
    const precioExistente = precios.find((precio: { fechaInicio: any; }) => precio.fechaInicio === formPrecio.fechaInicio);
    if (precioExistente) {
      return;
    }

    //convertir el precio a un objeto que acepte el servicio con seguro y seguro como objetos
    const nuevoPrecio = {
      precioBase: formPrecio.precioBase,
      fechaInicio: formPrecio.fechaInicio,
      copago: null,
      seguroMedico: {
        id: this.editando.id
      },
    };

    console.log('agregarPrecio', nuevoPrecio);
    this.preciosService.addPrecio(nuevoPrecio).subscribe({
      next: (data) => {
        this.DatosPrecioForm.reset();
      },
      error: (error) => {
        console.error(error);
      },
      complete: () => {
        //agregarlo a la lista de precios del seguro editando
        this.editando.precios.push(nuevoPrecio);
        
        //agregar el precio a la lista del seguro
        this.segurosMedicosService.getSegurosMedicos().subscribe({
          next: (data) => {
            this.SegurosMedicos = data;
          },
          error: (error) => {
            console.error(error);
          }
        });
      }
    });
  }
}
