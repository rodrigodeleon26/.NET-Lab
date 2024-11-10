import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MedicosService {

  private apiUrl = 'https://localhost/gestion/api/Medicos';

  constructor( private http: HttpClient ) { }

  getMedicos(): Observable<any[]> {
    console.log('Obteniendo médicos');
    return this.http.get<any[]>(this.apiUrl);
  }

  addMedico(medico: any): Observable<any> {
    console.log('Agregando médico');
    return this.http.post<any>(this.apiUrl, medico);
  }

  getMedicoById(id: string): Observable<any> {
    console.log('Obteniendo médico por id');
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  updateMedico(id: string, medico: any): Observable<any> {
    console.log('Actualizando médico');
    return this.http.put<any>(`${this.apiUrl}/${id}`, medico);
  }

  deleteMedico(id: string): Observable<any> {
    console.log('Eliminando médico');
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }

  getMedicosPaginadosYFiltrados(pagina: number, filtro: string): Observable<any> {
    console.log('Obteniendo médicos paginados y filtrados');
    return this.http.get<any>(`${this.apiUrl}/getMedicosPaginadosYFiltrados/${pagina}?filtro=${filtro}`);
  }
}
