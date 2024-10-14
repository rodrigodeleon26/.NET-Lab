import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PacienteService {
  private apiUrl = 'https://localhost:5001/api/pacientes'; // URL del microservicio

  constructor(private http: HttpClient) {}

  getPacientes(): Observable<any[]> {
    console.log('Obteniendo pacientes');
    return this.http.get<any[]>(this.apiUrl);
  }
}
