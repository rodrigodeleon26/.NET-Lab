import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConsultaMedicaService {
  private apiUrl = 'https://localhost/historiaclinica/api/HistoriasClinicas';

  constructor(
    private http: HttpClient
  ) { }

  obtenerConsultaMedica(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<any>(url);
  }

  actualizarCosultaMedica(consultaMedica: any): Observable<any> {
    const url = `${this.apiUrl}/${consultaMedica.id}`;
    return this.http.put<any>(url, consultaMedica);
  }

  eliminarConsultaMedica(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.delete<any>(url);
  }

  agregarReceta(consultaMedicaId: number, receta: any): Observable<any> {
    console.log(receta);
    const url = `${this.apiUrl}/${consultaMedicaId}/receta`;
    return this.http.post<any>(url, receta);
  }

  editarReceta(consultaMedicaId: number, receta: any): Observable<any> {
    const url = `${this.apiUrl}/${consultaMedicaId}/receta`; 
    return this.http.put<any>(url, receta);
  }

  eliminarReceta(consultaMedicaId: number, recetaId: number): Observable<any> {
    const url = `${this.apiUrl}/${consultaMedicaId}/receta/${recetaId}`;
    return this.http.delete<any>(url);
  }

  agregarEstudio(consultaMedicaId: number, estudio: any): Observable<any> {
    console.log(estudio);
    const url = `${this.apiUrl}/${consultaMedicaId}/estudio`;
    return this.http.post<any>(url, estudio);
  }


  editarEstudio(consultaMedicaId: number, estudio: any): Observable<any> {
    const url = `${this.apiUrl}/${consultaMedicaId}/estudio`;
    return this.http.put<any>(url, estudio);
  }

  eliminarEstudio(consultaMedicaId: number, estudioId: number): Observable<any> {
    const url = `${this.apiUrl}/${consultaMedicaId}/estudio/${estudioId}`;
    return this.http.delete<any>(url);
  }
}
