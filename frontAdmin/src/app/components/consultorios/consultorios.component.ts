import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ConsultoriosService } from '../../services/consultorios.service';

@Component({
  selector: 'app-consultorios',
  templateUrl: './consultorios.component.html',
  styleUrl: './consultorios.component.css'
})
export class ConsultoriosComponent implements OnInit{
  DatosConsultorioForm: FormGroup;

  loading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';

  consultorios: any[] = [];
  verDetallePara: number | null = null;
  consultorioBorrarId: string | null = null;
  isModalVisible: boolean = false;
  editando: string | null = null;

  sortNum: number = 0;
  sortPiso: number = 0;

  constructor( 
    private consultoriosService: ConsultoriosService,
    private fb: FormBuilder,
  ) { 
    this.DatosConsultorioForm = this.fb.group({
      numero: ['', [Validators.required, Validators.pattern('^[0-9]*$')]],
      piso: ['', [Validators.required, Validators.pattern('^[0-9]*$')]],
    });
  }

  ngOnInit(): void {
    this.loading = true;
    this.consultoriosService.getConsultorios().subscribe({
      next: (data) => {
        this.consultorios = data;
        console.log(data);
      },
      error: (error) => {
        console.error(error);
      },
      complete: () => {
        this.loading = false;
      }
    })
  }

  showSuccessMessage(message: string) {
    this.successMessage = message;
      setTimeout(() => {
        this.errorMessage = '';
      }, 3000);
  }

  showErrorMessage(message: string) {
    this.errorMessage = message;
    setTimeout(() => {
      this.errorMessage = '';
    }, 3000);
  }

  sort(sortBy: string){
    //sortby puede ser num o piso
    if(sortBy === 'Num'){
      this.sortPiso = 0;
      switch(this.sortNum){
        case 0:
          this.sortNum = 1; //ascendente
          this.consultorios.sort((a, b) => {
            return a.numero - b.numero;
          });
          break;
        case 1:
          this.sortNum = -1; //descendente
          this.consultorios.sort((a, b) => {
            return b.numero - a.numero;
          });
          break;
        case -1:
          this.sortNum = 0; //sin orden-ordenar por id
          this.consultorios.sort((a, b) => {
            return a.id - b.id;
          });
          break;
      }
    }
    else if(sortBy === 'Piso'){
      this.sortNum = 0;
      switch(this.sortPiso){
        case 0:
          this.sortPiso = 1; //ascendente
          this.consultorios.sort((a, b) => {
            return a.piso - b.piso;
          });
          break;
        case 1:
          this.sortPiso = -1; //descendente
          this.consultorios.sort((a, b) => {
            return b.piso - a.piso;
          });
          break;
        case -1:
          this.sortPiso = 0; //sin orden-ordenar por id
          this.consultorios.sort((a, b) => {
            return a.id - b.id;
          });
          break;
      }
    }
  }

  editarConsultorio( id: string ){
    this.editando = id;
    const consultorio = this.consultorios.find( consultorio => consultorio.id === id);
    if(consultorio){
      this.DatosConsultorioForm.patchValue(consultorio);
    }
  }

  onModalContainerClick(event: MouseEvent): void {
    if ((event.target as HTMLElement).classList.contains('fixed')) {
      this.isModalVisible = false;
      this.consultorioBorrarId = null;
    }
  }

  borrarConsultorios(){
    if(this.consultorioBorrarId){
      this.consultoriosService.deleteConsultorio(this.consultorioBorrarId).subscribe({
        next: (data) => {
          this.showSuccessMessage('Consultorio eliminado correctamente');
          //reiniciar la lista de medicos
          this.consultoriosService.getConsultorios()
            .subscribe({
              next: (data) => {
                this.consultorios = data;
              },
              error: (error) => {
                console.error(error);
              }
            });
        },
        error: (error) => {
          console.error(error);
          this.showErrorMessage('Error al eliminar el consultorio');
        }
      });
    }
    else{
      this.showErrorMessage('Error al eliminar el consultorio');
    }
    this.isModalVisible = false;
    this.consultorioBorrarId = null;
  }

  agregarConsultorio(){
    if(this.DatosConsultorioForm.invalid){
      this.showErrorMessage('Ingrese todos los campos correctamente');
      return;
    }
    if(this.editando){
      this.editarConsultorioSend();
      return;
    }
    this.loading = true;
    this.consultoriosService.addConsultorio(this.DatosConsultorioForm.value).subscribe({
      next: (data) => {
        this.showSuccessMessage('Consultorio agregado correctamente');
        //reiniciar la lista de especialidades
        this.consultoriosService.getConsultorios()
          .subscribe({
            next: (data) => {
              this.consultorios = data;
            },
            error: (error) => {
              console.error(error);
            }
          });
      },
      error: (error) => {
        console.error(error);
        this.showErrorMessage('Error al agregar el consultorio');
      },
      complete: () => {
        this.DatosConsultorioForm.reset();
        this.loading = false;
      }
    });
  }

  editarConsultorioSend(){
    this.loading = true;
    if(this.editando){
      this.consultoriosService.updateConsultorio(this.editando ,this.DatosConsultorioForm.value).subscribe({
        next: (data) => {
          this.showSuccessMessage('Consultorio editado correctamente');
          //reiniciar la lista de especialidades
          this.consultoriosService.getConsultorios()
            .subscribe({
              next: (data) => {
                this.consultorios = data;
              },
              error: (error) => {
                console.error(error);
              }
            });
        },
        error: (error) => {
          console.error(error);
          this.showErrorMessage('Error al editar el consultorio');
        },
        complete: () => {
          this.DatosConsultorioForm.reset();
          this.loading = false;
          this.editando = null;
        }
      });
    }
  }
}