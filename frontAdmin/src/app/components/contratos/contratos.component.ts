import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ContratosService } from '../../services/contratos.service';
import { SegurosMedicosService } from '../../services/seguros-medicos.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-contratos',
  templateUrl: './contratos.component.html',
  styleUrls: ['./contratos.component.css']
})
export class ContratosComponent implements OnInit {
  loading: boolean = false;
  contratos: any[] = [];
  segurosMedicos: any[] = [];
  busqueda: string = '';
  paginaActual: number = 1;
  contratoForm!: FormGroup;
  reactivarForm!: FormGroup;
  isModalVisibleActualizar: boolean = false;
  isModalVisibleVer: boolean = false;
  isModalVisibleConfirmarBorrado: boolean = false;
  modalTitle: string = '';
  loadingModal: boolean = false;
  isViewMode: boolean = false;
  contratoParaBorrar: any = null;
  contratoParaActualizar: any = null;
  isModalVisibleFacturas: boolean = false;
  facturas: any[] = [];
  deuda: number = 0;
  interes: number = 1;
  cantidadCuotas: number = 6;

  constructor(
    private fb: FormBuilder,
    private contratosService: ContratosService,
    private segurosMedicosService: SegurosMedicosService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.contratoForm = this.fb.group({
      seguroMedicoId: ['', Validators.required]
    });

    this.reactivarForm = this.fb.group({
      cantidadCuotas: [this.cantidadCuotas, [Validators.required]],
      interes: [this.interes, [Validators.required, Validators.min(1), Validators.max(100)]]
    });

    this.getContratos();
    this.getSegurosMedicos();
  }

  getContratos(): void {
    this.loading = true;
    this.contratosService.getContratosFiltradosPaginados(1, '')
      .subscribe({
        next: (data) => {
          this.contratos = data;
        },
        error: (error) => {
          console.error(error);
        },
        complete: () => {
          this.loading = false;
        }
      });
  }

  getSegurosMedicos(): void {
    this.segurosMedicosService.getSegurosMedicos().subscribe({
      next: (data) => {
        this.segurosMedicos = data;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  buscar(): void {
    this.contratosService.getContratosFiltradosPaginados(1, this.busqueda).subscribe({
      next: (data) => {
        this.contratos = data;
        this.paginaActual = 1;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  cambiarPagina(pagina: number): void {
    if (pagina < 1) {
      return;
    }
    this.contratosService.getContratosFiltradosPaginados(pagina, this.busqueda).subscribe({
      next: (data) => {
        if (!data || data.length === 0) {
          return;
        }
        this.contratos = data;
        this.paginaActual = pagina;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  LimpiarBusqueda(): void {
    this.busqueda = '';
    this.contratosService.getContratosFiltradosPaginados(1, '').subscribe({
      next: (data) => {
        this.contratos = data;
        this.paginaActual = 1;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  //   abrirModalVer(id: number): void {
  //   console.log("El id que se manda es: " + id);
  //   this.contratosService.getContratoPorId(id).subscribe({
  //     next: (contrato) => {
  //       this.isModalVisibleVer = true;
  //       this.modalTitle = 'Ver Contrato';
  //       this.isViewMode = true;
  //       this.contratoForm.patchValue(contrato);
  //     },
  //     error: (error) => {
  //       console.error('Error al obtener el contrato', error);
  //     }
  //   });
  // }

  abrirModalActualizar(contrato: any): void {
    this.contratosService.getContratoPorId(contrato.id).subscribe({
      next: (contrato) => {
        this.contratoParaActualizar = contrato;
        this.isModalVisibleActualizar = true;
        this.modalTitle = 'Actualizar Contrato';
        this.isViewMode = false;
        this.contratoForm.patchValue({ seguroMedicoId: contrato.seguroMedico.id });
      },
      error: (error) => {
        console.error('Error al obtener el contrato', error);
      }
    });
  }

  cerrarModal(): void {
    this.isModalVisibleActualizar = false;
    this.isModalVisibleFacturas = false;
  }

  onSubmit(): void {
    if (this.contratoForm.invalid) {
      return;
    }
  
    this.loadingModal = true;
    const contrato = this.contratoForm.value;
  
    if (this.isModalVisibleActualizar) {
      const request = {
        IdContratoActual: this.contratoParaActualizar.id,
        IdNuevoSeguroMedico: contrato.seguroMedicoId
      };
  
      this.contratosService.cambiarContrato(request).subscribe({
        next: (response: any) => {
          this.getContratos();         
          this.cerrarModal();
          this.toastr.success(response.message, 'Actualización de contrato');
        },
        error: (error) => {
          this.loadingModal = false;
          // console.error('Error al activar el contrato', error);
          if (error.status === 400) {
            this.toastr.error(error.error, 'Pagos pendientes');
          } else {
            this.toastr.error('Error al cambiar el contrato');
          }
        },
        complete: () => {
          this.loadingModal = false;
        }
      });
    }
  }

  abrirModalConfirmarBorrado(contrato: any): void {
    this.contratoParaBorrar = contrato;
    this.isModalVisibleConfirmarBorrado = true;
  }

  confirmarBorrado(): void {
    if (this.contratoParaBorrar && this.contratoParaBorrar.id) {
      this.loadingModal = true;
      this.contratosService.borrarContrato(this.contratoParaBorrar.id).subscribe({
        next: () => {
          this.getContratos();
          this.isModalVisibleConfirmarBorrado = false;
          this.contratoParaBorrar = null;
          this.toastr.success('Contrato dado de baja exitosamente', 'Baja de contrato');
          this.loadingModal = false;
        },
        error: (error) => {
          this.loadingModal = false;
          console.error(error);
          if (error.error && error.error.description) {
            this.toastr.error(error.error.description, error.error.code);
          } else {
            this.toastr.error('Error al eliminar el contrato');
          }
        }
      });
    }
  }

  get precioTotalFinanciado(): number {
    if (this.reactivarForm.valid) {
      return this.deuda * (1 + this.reactivarForm.get('interes')?.value / 100);
    }
    return 0;
  }

  get cuota(): number {
    if (this.reactivarForm.valid) {
      return this.precioTotalFinanciado / this.reactivarForm.get('cantidadCuotas')?.value;
    }
    return 0;
  }

  activarContrato(contrato: any): void {
    this.loadingModal = true;
    console.log(contrato);
    this.contratosService.getUltimasFacturas(contrato.id).subscribe({
      next: (response: any) => {
        this.facturas = response.ultimasfacturas;
        this.deuda = response.deuda;
        this.loadingModal = false;
        console.log(response);
        this.isModalVisibleFacturas = true;
      },
      error: (error) => {
        this.loadingModal = false;
        console.error(error);
      }
    });
    // this.loadingModal = true;
    // this.contratosService.activarContrato(contrato.id).subscribe({
    //   next: (response: any) => {
    //     this.getContratos();
    //     this.toastr.success(response.message, 'Activación de contrato');
    //     this.loadingModal = false;
    //   },
    //   error: (error) => {
    //     this.loadingModal = false;
    //     // console.error('Error al activar el contrato', error);
    //     if (error.status === 400) {
    //       this.toastr.error(error.error, 'Pagos pendientes');
    //     } else {
    //       this.toastr.error('Error al activar el contrato');
    //     }
    //   }
    // });
  }

  reactivarContrato() {
    if (this.reactivarForm.valid) {
      const { cantidadCuotas, interes } = this.reactivarForm.value;
      // Lógica para reactivar el contrato con los valores de cantidadCuotas e interes
      console.log('reactivando contrato con:', cantidadCuotas, interes);
    } else {
      console.log('Formulario inválido');
    }
  }
}