import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CopagosService {
  private selectedArticuloSource = new BehaviorSubject<any>(null);
  selectedArticulo$ = this.selectedArticuloSource.asObservable();

  private selectedSeguroMedicoSource = new BehaviorSubject<any>(null);
  selectedSeguroMedico$ = this.selectedSeguroMedicoSource.asObservable();

  private articulosDeSeguroMedicoSource = new BehaviorSubject<any[]>([]);
  articulosDeSeguroMedico$ = this.articulosDeSeguroMedicoSource.asObservable();

  constructor() { }

  changeSelectedArticulo(articulo: any) {
    this.selectedArticuloSource.next(articulo);
  }

  changeSelectedSeguroMedico(seguroMedico: any) {
    console.log('changeSelectedSeguroMedico', seguroMedico);
    this.selectedSeguroMedicoSource.next(seguroMedico);
  }

  changeArticulosDeSeguroMedico(articulos: any[]) {
    this.articulosDeSeguroMedicoSource.next(articulos);
  }
}
