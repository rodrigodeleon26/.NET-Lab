import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs'; // Asegúrate de tener un modelo Paciente

@Injectable({
  providedIn: 'root',
})
export class PacientesService {
  private apiUrl = 'https://localhost:5009/api/Pacientes'; // URL del microservicio

  constructor(private http: HttpClient) {}

  getPacientes(): Observable<any[]> {
    console.log('Obteniendo pacientes');
    return this.http.get<any[]>(this.apiUrl);
  }

  getPacientesFiltradosPaginados(pag: number, filtro: string): Observable<any[]> {
    const url = `${this.apiUrl}/filtradosPaginados?pag=${pag}&filtro=${filtro}`;
    return this.http.get<any[]>(url);
  }

  getPaciente(id: number): Observable<any[]> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  addPaciente(paciente: any): Observable<any[]> {
    return this.http.post<any>(this.apiUrl, paciente);
  }
}
