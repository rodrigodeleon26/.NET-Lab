import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs'; // Asegúrate de tener un modelo Paciente

@Injectable({
  providedIn: 'root',
})
export class PacientesService {
  private apiUrl = 'https://localhost/gestion/api/Pacientes'; // URL del microservicio

  constructor(private http: HttpClient) {}

  getPacientes(): Observable<any[]> {
    console.log('Obteniendo pacientes');
    return this.http.get<any[]>(this.apiUrl);
  }
}
