import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PacienteService {
  private apiUrl = 'https://localhost/paciente/api/Pacientes'; // URL del microservicio

  constructor(private http: HttpClient) {}

  obtenerMisDatos(cedula: string): Observable<any> {
    console.log('Obtener mis datos:', cedula);
    const url = `${this.apiUrl}/${cedula}/misDatos`;
    console.log('URL:', url);
    return this.http.get<any>(url);
  }

  actualizarMisDatos(cedula: string, paciente: any): Observable<any> {  
    const url = `${this.apiUrl}/${cedula}/actulizarDatos`;
    return this.http.put<any>(url, paciente);
  }

  obtenerMiHistoriaClinica(documento: string, pageNumber: number, pageSize: number,
    orden: string, fechaInicio: string, fechaFin: string, especialidades: any[]
  ): Observable<any> {
    const url = `${this.apiUrl}/${documento}/miHistoriaClinica`;
    let params = new HttpParams()
        .set('pageNumber', pageNumber.toString())
        .set('pageSize', pageSize.toString())
        .set('orden', orden);

    if (fechaInicio && fechaFin) {
      params = params.set('fechaInicio', fechaInicio).set('fechaFin', fechaFin);
    }

    console.log('Especialidades:', especialidades);

    // Convertimos el array de especialidades a JSON y lo agregamos a los parámetros
    params = params.set('especialidades', JSON.stringify(especialidades));

    console.log(params.toString());

    return this.http.get<any>(url, { params });
  }

  obtenerNotificaciones(documento: string, pageNumber: number, pageSize: number): Observable<any> {
    const url = `${this.apiUrl}/${documento}/notificaciones`;
    let params = new HttpParams()
        .set('pageNumber', pageNumber.toString())
        .set('pageSize', pageSize.toString());

    return this.http.get<any>(url, { params });
  }

  verNotificacion(id: number): Observable<any> {
    console.log('Ver notificación:', id);
    const url = `${this.apiUrl}/${id}/notificaciones`;
    return this.http.put<any>(url, null);
  }

  obtenerCitas(cedula: string): Observable<any> {
    const url = `${this.apiUrl}/${cedula}/misCitas`;
    return this.http.get<any>(url);
  }
  cancelarCita(cedula: string, citaId: number): Observable<any> {
    const url = `${this.apiUrl}/${cedula}/citas/${citaId}/cancelarCita`;
    console.log('Cancelar cita:', url);
    return this.http.delete<any>(url);
  }

  obtenerHistorialFacturas(documento: string, pageNumber: number, pageSize: number): Observable<any> {
    const url = `${this.apiUrl}/${documento}/historialFacturacion`;
    let params = new HttpParams()
        .set('pageNumber', pageNumber.toString())
        .set('pageSize', pageSize.toString());

    return this.http.get<any>(url, { params });
  }

  desvincularConGoogle(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}/desvincularGoogle`;
    return this.http.put<any>(url, null);
  }
}
