import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EspecialidadesService {

  private apiUrl = 'https://localhost/gestion/api/especialidades'; // URL del microservicio

  constructor(private http: HttpClient) { }

  getEspecialidades(): Observable<any[]> {
    console.log('Obteniendo especialidades');
    return this.http.get<any[]>(this.apiUrl);
  }

  deleteEspecialidad(id: string): Observable<any> {
    console.log('Borrando especialidad');
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }

  addEspecialidad(especialidad: any): Observable<any> {
    console.log('Agregando especialidad');
    return this.http.post<any>(this.apiUrl, especialidad);
  }

  updateEspecialidad(id: string, especialidad: any): Observable<any> {
    console.log('Actualizando especialidad');
    //agrgarle el parametro id a la especialidad
    especialidad.id = id;
    return this.http.put<any>(`${this.apiUrl}/${id}`, especialidad);    
  }
}
