import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ContratosService {
  private apiUrl = 'https://localhost/gestion/api/Contratos'; // URL del microservicio

  constructor(private http: HttpClient) { }

  getContratosFiltradosPaginados(pag: number, filtro: string): Observable<any[]> {
    const url = `${this.apiUrl}/filtradosPaginados?pag=${pag}&filtro=${filtro}`;
    return this.http.get<any[]>(url);
  }

  getContratoPorId(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<any>(url);
  }

  borrarContrato(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.delete<any>(url);
  }

  cambiarContrato(request: { IdContratoActual: number, IdNuevoSeguroMedico: number }): Observable<any> {
    const url = `${this.apiUrl}/cambiarContrato`;
    return this.http.post<any>(url, request);
  }

  activarContrato(idContrato: number): Observable<any> {
    const url = `${this.apiUrl}/activarContrato/${idContrato}`;
    return this.http.post<any>(url, {});
  }

  getUltimasFacturas(idContrato: number): Observable<any[]> {
    const url = `${this.apiUrl}/${idContrato}/getUltimasFacturas`;
    return this.http.get<any[]>(url);
  }

  reactivarContrato(id: number, cantidadCuotas: number, interes: number) {
    return this.http.post(`${this.apiUrl}/${id}/reactivarContrato`, { cuotas: cantidadCuotas, interes });
  }
}