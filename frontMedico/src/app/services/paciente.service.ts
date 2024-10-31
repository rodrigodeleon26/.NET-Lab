import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PacienteService {
  private apiUrl = 'https://localhost:5005/api/ConsultaMedica/HistoriasClinicas';
  private apiUrlPacientes = 'https://localhost:5009/api/Pacientes';

  constructor(private http: HttpClient) {}

  getPacientes(id: number): Observable<any[]> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<any[]>(url);
  }

  getPacienteGestion(id: number): Observable<any[]> {
    const url = `${this.apiUrlPacientes}/${id}`;
    return this.http.get<any[]>(url);
  }
}
