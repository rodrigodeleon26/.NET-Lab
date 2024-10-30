import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MedicosService {

  private apiUrl = 'https://localhost:5009/api/Medicos';

  constructor( private http: HttpClient ) { }

  getMedicos(): Observable<any[]> {
    console.log('Obteniendo médicos');
    return this.http.get<any[]>(this.apiUrl);
  }

  addMedico(medico: any): Observable<any> {
    console.log('Agregando médico');
    return this.http.post<any>(this.apiUrl, medico);
  }
}
