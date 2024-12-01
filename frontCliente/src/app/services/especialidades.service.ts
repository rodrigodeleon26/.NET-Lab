import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EspecialidadesService {

  private apiUrl = 'https://localhost/gestion/api/especialidades'; // URL del microservicio

  constructor(private http: HttpClient) { }

  getEspecialidades(): Observable<any[]> {
    console.log('Obteniendo especialidades');
    return this.http.get<any[]>(this.apiUrl);
  }

  getEspecialidadesHabilitadas(cedula: string): Observable<any[]> {
    console.log('Obteniendo especialidades habilitados');
    return this.http.get<any[]>(`${this.apiUrl}/EspecialidadesHabilitados/${cedula}`);
  }
}
