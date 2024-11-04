import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CitasMedicasService {
  private apiUrl = '/citasmedicas/api/CitasMedicas';

  constructor(
    private http: HttpClient
  ) { }

  // Obtener todas las citas médicas
  obtenerCitasMedicas(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  // Obtener una cita médica por ID
  obtenerCitaMedica(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<any>(url);
  }

  // Crear una nueva cita médica
  crearCitaMedica(calendarioId: number, pacienteId: number, nuevaCita: any): Observable<any> {
    const url = `${this.apiUrl}/${calendarioId}/${pacienteId}`;
    return this.http.post<any>(url, nuevaCita);
  }

  // Cambiar el estado de una cita médica
  editarEstado(id: number, estado: string): Observable<void> {
    const url = `${this.apiUrl}/${id}/${estado}`;
    return this.http.put<void>(url, {});
  }

  // Actualizar una cita médica existente
  actualizarCitaMedica(id: number, citaActualizada: any): Observable<void> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.put<void>(url, citaActualizada);
  }

  // Eliminar una cita médica
  eliminarCitaMedica(id: number): Observable<void> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.delete<void>(url);
  }
}