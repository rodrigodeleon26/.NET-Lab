import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConsultaMedicaService {
  private apiUrl = 'https://localhost:5005/api/HistoriasClinicas';

  constructor(
    private http: HttpClient
  ) { }

  obtenerMedicamentos(): Observable<any> {
    const url = `${this.apiUrl}/Medicamentos`;
    return this.http.get<any>(url);
  }

  crearConsulta(consultaMedica: any): Observable<any> {
    const url = `${this.apiUrl}`;
    return this.http.post<any>(url, consultaMedica);
  }

  crearConsultaSinDatos(consultaMedicaId: number): Observable<any> {
    const url = `${this.apiUrl}/${consultaMedicaId}`;
    return this.http.post<any>(url, {});
  }

  obtenerConsultaMedica(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<any>(url);
  }

  actualizarCosultaMedica(consultaMedica: any): Observable<any> {
    const url = `${this.apiUrl}/${consultaMedica.id}`;
    return this.http.put<any>(url, consultaMedica);
  }

  guardarConsultaMedica(consultaMedicaId: number): Observable<any> {
    console.log(consultaMedicaId);
    const url = `${this.apiUrl}/${consultaMedicaId}/guardarConsulta`;
    return this.http.put<any>(url, {});
  }

  eliminarConsultaMedica(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.delete<any>(url);
  }

  agregarReceta(consultaMedicaId: number, receta: any): Observable<any> {
    console.log(receta);
    const url = `${this.apiUrl}/${consultaMedicaId}/receta`;
    return this.http.post<any>(url, receta);
  }

  editarReceta(consultaMedicaId: number, receta: any): Observable<any> {
    const url = `${this.apiUrl}/${consultaMedicaId}/receta`; 
    return this.http.put<any>(url, receta);
  }

  eliminarReceta(consultaMedicaId: number, recetaId: number): Observable<any> {
    const url = `${this.apiUrl}/${consultaMedicaId}/receta/${recetaId}`;
    return this.http.delete<any>(url);
  }

  agregarEstudio(consultaMedicaId: number, estudio: any): Observable<any> {
    console.log(estudio);
    const url = `${this.apiUrl}/${consultaMedicaId}/estudio`;
    return this.http.post<any>(url, estudio);
  }


  editarEstudio(consultaMedicaId: number, estudio: any): Observable<any> {
    const url = `${this.apiUrl}/${consultaMedicaId}/estudio`;
    return this.http.put<any>(url, estudio);
  }

  eliminarEstudio(consultaMedicaId: number, estudioId: number): Observable<any> {
    const url = `${this.apiUrl}/${consultaMedicaId}/estudio/${estudioId}`;
    return this.http.delete<any>(url);
  }

  obtenerHistoriaClinica(documento: string, pageNumber: number, pageSize: number,
    orden: string, fechaInicio: string, fechaFin: string, especialidades: any[]
  ): Observable<any> {
    const url = `${this.apiUrl}/${documento}/historiaClinica`;
    let params = new HttpParams()
        .set('pageNumber', pageNumber.toString())
        .set('pageSize', pageSize.toString())
        .set('orden', orden);

    if (fechaInicio && fechaFin) {
      params = params.set('fechaInicio', fechaInicio).set('fechaFin', fechaFin);
    }

    // Convertimos el array de especialidades a JSON y lo agregamos a los parámetros
    params = params.set('especialidades', JSON.stringify(especialidades));

    return this.http.get<any>(url, { params });
  }

  getEspecialidades(): Observable<any> {
    const url = `https://localhost:5009/api/Especialidades`;
    console.log(url);
    return this.http.get<any>(url);
  }
}
