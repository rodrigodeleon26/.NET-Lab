import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PacienteService {
  private apiUrl = 'https://localhost:5005/api/ConsultaMedica/HistoriasClinicas';

  constructor(private http: HttpClient) {}

  getPacientes(id: number): Observable<any[]> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<any[]>(url);
  }
}
