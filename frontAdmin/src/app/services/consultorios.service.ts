import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConsultoriosService {

  private apiUrl = 'https://localhost:5009/api/Consultorios'; // URL del microservicio

  constructor(private http: HttpClient) { }

  getConsultorios(): Observable<any[]> {
    console.log('Obteniendo consultorios');
    return this.http.get<any[]>(this.apiUrl);
  }
  
  deleteConsultorio(id: string): Observable<any> {
    console.log('Borrando consultorio');
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }

  addConsultorio(consultorio: any): Observable<any> {
    console.log('Agregando consultorio');
    return this.http.post<any>(this.apiUrl, consultorio);
  }

  updateConsultorio(id: string, consultorio: any): Observable<any> {
    console.log('Actualizando consultorio');
    //agrgarle el parametro id a la consultorio
    consultorio.id = id;
    return this.http.put<any>(`${this.apiUrl}/${id}`, consultorio);    
  }

  getConsultorio(id: string): Observable<any> {
    console.log('Obteniendo consultorio');
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }
}
