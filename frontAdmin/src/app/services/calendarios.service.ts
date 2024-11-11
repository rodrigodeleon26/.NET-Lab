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
}
