import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { SegurosMedicosService } from './seguros-medicos.service';

@Injectable({
  providedIn: 'root'
})
export class CopagosService {

  private apiUrl = 'https://localhost:5009/api/Copagos'; // URL del microservicio

  private selectedArticuloSource = new BehaviorSubject<any>(null);
  selectedArticulo$ = this.selectedArticuloSource.asObservable();

  private selectedSeguroMedicoSource = new BehaviorSubject<any>(null);
  selectedSeguroMedico$ = this.selectedSeguroMedicoSource.asObservable();

  private copagosDeSeguroMedicoSource = new BehaviorSubject<any[]>([]);
  copagosDeSeguroMedico$ = this.copagosDeSeguroMedicoSource.asObservable();

  private refreshSeguroSource = new Subject<void>();
  refreshSeguro$ = this.refreshSeguroSource.asObservable();

  constructor(
    private http: HttpClient,
    private segurosMedicosService: SegurosMedicosService
  ) { }

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

  addCopago(copago: any) {
    console.log("añadiendo Copago:" + copago);
    
    return this.http.post<any>(this.apiUrl, copago);
  }

  RefreshCopagos(){
    this.refreshSeguroSource.next();
  }

  deleteCopago(id: number) {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
