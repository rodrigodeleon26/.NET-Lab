import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ContratosService {
  private apiUrl = 'https://localhost:5009/api/Contratos'; // URL del microservicio

  constructor(private http: HttpClient) { }

  getContratosFiltradosPaginados(pag: number, filtro: string): Observable<any[]> {
    const url = `${this.apiUrl}/filtradosPaginados?pag=${pag}&filtro=${filtro}`;
    return this.http.get<any[]>(url);
  }

  getContratoPorId(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<any>(url);
  }

  actualizarContrato(contrato: any): Observable<any> {
    const url = `${this.apiUrl}/${contrato.id}`;
    return this.http.put<any>(url, contrato);
  }

  borrarContrato(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.delete<any>(url);
  }
}