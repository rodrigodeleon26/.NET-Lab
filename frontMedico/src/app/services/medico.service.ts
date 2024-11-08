import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MedicoService {
  private apiUrl = 'https://localhost/gestion/api/Medicos';

  constructor(private http: HttpClient) { }

  // Obtener todos los médicos
  obtenerMedicos(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}`);
  }

  // Crear un nuevo médico
  crearMedico(medico: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, medico);
  }

  // Obtener un médico por ID
  obtenerMedicoPorId(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  // Actualizar un médico existente
  actualizarMedico(id: number, medico: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, medico);
  }

  // Eliminar un médico por ID
  eliminarMedico(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }

  // Asignar una especialidad a un médico
  asignarEspecialidad(medId: number, espId: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/asignarEspecialidad/${medId}/${espId}`, {});
  }
}