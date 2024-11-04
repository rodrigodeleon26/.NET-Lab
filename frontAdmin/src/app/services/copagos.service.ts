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

  private copagosDeSeguroMedicoSource = new BehaviorSubject<any[]>([]);
  copagosDeSeguroMedico$ = this.copagosDeSeguroMedicoSource.asObservable();

  constructor() { }

  changeSelectedArticulo(articulo: any) {
    this.selectedArticuloSource.next(articulo);
  }

  changeSelectedSeguroMedico(seguroMedico: any) {
    this.selectedSeguroMedicoSource.next(seguroMedico);
    this.changeCopagosDeSeguroMedico(seguroMedico.copagos);
  }

  changeCopagosDeSeguroMedico(articulos: any[]) {
    this.copagosDeSeguroMedicoSource.next(articulos);
  }
}
