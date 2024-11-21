import { Injectable } from '@angular/core';
import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CitasMedicasService {
  private apiUrl = 'https://localhost:5003/api/CitasMedicas';

  constructor(
    private http: HttpClient,
    private datePipe: DatePipe
  ) { }

  // Obtener todas las citas médicas
  obtenerCitasMedicas(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  // Obtener todas las citas médicas de una especialidad
  obtenerCitasMedicasPorEspecialidad(espec: string, pag: number, fecha: Date): Observable<any[]> {
    const formattedDate = this.datePipe.transform(fecha, 'yyyy-MM-dd');
    const url = `${this.apiUrl}/especialidad/${espec}/${pag}/${formattedDate}`;
    return this.http.get<any>(url);
  }

  chequearPaginaCita(espec: string, pag: number, fecha: Date): Observable<boolean> {
    const formattedDate = this.datePipe.transform(fecha, 'yyyy-MM-dd');
    const url = `${this.apiUrl}/conteo/${espec}/${pag}/${formattedDate}`;
    return this.http.get<boolean>(url);
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