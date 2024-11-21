import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CalendariosService {

  private apiUrl = 'https://localhost:5009/api/Calendarios'; // URL del microservicio

  constructor( private http: HttpClient) { }

  getCalendariosByMedicoId(medicoId: string): Observable<any> {
    console.log('Obteniendo calendarios');
    return this.http.get<any[]>(`${this.apiUrl}/medico/${medicoId}`);
  }

  checkCalendarioOcupado(calendario: any): Observable<any> {
    console.log('Verificando disponibilidad');
    console.log(calendario);
    return this.http.post<any>(`${this.apiUrl}/checkOcupacionConsultorio`, calendario);
  }

  addCalendario(calendario: any): Observable<any> {
    console.log('Agregando calendario');
    return this.http.post<any>(`${this.apiUrl}`, calendario);
  }

  removeCalendario(calendarioId: number): Observable<any> {
    console.log('Eliminando calendario');
    return this.http.delete<any>(`${this.apiUrl}/${calendarioId}`);
  }

  updateCalendario(calendario: any): Observable<any> {
    console.log('Actualizando calendario');
    return this.http.put<any>(`${this.apiUrl}/${calendario.Id}`, calendario);
  }

  validarEspecialidadesParaBorrar(medicoId: string, especialidades: any): Observable<any> {
    console.log('Validando especialidades para borrar');
    console.log(especialidades);
    return this.http.post<any>(`${this.apiUrl}/validarEspecialidadesParaBorrar/${medicoId}`, especialidades);
  }

  BorrarCalendariosIncompatibles(medicoId: string, especialidades: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/borrarCalendariosIncompatibles/${medicoId}`, especialidades);
  }

  getCalendariosFiltrados(medicoId: string, filtros: string[]): Observable<any> {
    console.log('Obteniendo calendarios filtrados');
    console.log(filtros);
    return this.http.post<any>(`${this.apiUrl}/filtrarCalendarios/${medicoId}`, filtros);
  }
}
