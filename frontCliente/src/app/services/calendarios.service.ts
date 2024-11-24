import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CalendariosService {
  private apiUrl = 'https://localhost:5009/api/Calendarios';

  constructor(private http: HttpClient) { }

  getCalendariosByEspecialidadFecha(especialidadId: string, fecha: string): Observable<any[]> {
    console.log('Obteniendo especialidades');
    return this.http.get<any[]>(`${this.apiUrl}/especialidad/${especialidadId}/fecha/${fecha}`);
  }
}
