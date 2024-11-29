import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CalendariosService {
  private apiUrl = 'https://localhost:5009/api/Calendarios';

  constructor(private http: HttpClient) { }

  getCalendariosByArticuloFecha(cedula: string, articuloId: string, fecha: string): Observable<any[]> {
    console.log('Obteniendo especialidades');
    return this.http.get<any[]>(`${this.apiUrl}/${cedula}/articulo/${articuloId}/fecha/${fecha}`);
  }

  getCalendariosByEspecialidadFecha(cedula: string ,especialidadId: string, fecha: string): Observable<any[]> {
    console.log('Obteniendo especialidades');
    return this.http.get<any[]>(`${this.apiUrl}/${cedula}/especialidad/${especialidadId}/fecha/${fecha}`);
  }
}
