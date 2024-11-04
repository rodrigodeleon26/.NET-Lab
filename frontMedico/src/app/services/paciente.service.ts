import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PacienteService {
  private apiUrl = '/historiaclinica/api/HistoriasClinicas';
  private apiUrlPacientes = '/gestion/api/Pacientes';

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
