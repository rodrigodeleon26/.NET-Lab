import { Component, OnInit } from '@angular/core';
import { SegurosMedicosService } from '../../services/seguros-medicos.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-seguro-medico-select',
  templateUrl: './seguro-medico-select.component.html',
  styleUrl: './seguro-medico-select.component.css'
})
export class SeguroMedicoSelectComponent implements OnInit{
  DatosSMForm: FormGroup;

  SegurosMedicos: any[] = [];
  isModalVisible: boolean = false;

  editando: any = '';

  constructor(
    private fb: FormBuilder,
    private segurosMedicosService: SegurosMedicosService,
  ) { 
    this.DatosSMForm = this.fb.group({
      nombre: ['', [Validators.required, Validators.maxLength(50)]],
      descripcion: ['', [Validators.required, Validators.maxLength(500)]],
    });
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
  }

  elegirParaEditar(smId: string){
    if (smId === '' || smId === null || smId == this.editando.id) {
      this.editando = '';
      this.DatosSMForm.reset();
      return;
    }
    this.editando = this.SegurosMedicos.find(a => a.id === smId);
    this.DatosSMForm.patchValue(this.editando);
    console.log(this.editando);
  }

  editar(): void {
    if (this.editando === '' || this.editando === null || this.editando === undefined || this.editando.id === '') {
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
    if (this.editando === '' || this.editando === null || this.editando === undefined || this.editando.id === '') {
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
}
