import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SegurosMedicosService {

  private apiUrl = 'https://localhost:5009/api/SegurosMedicos'; // URL del microservicio

  constructor(private http: HttpClient) { }

  getSegurosMedicos(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  getSeguroMedico(id: number): Observable<any[]> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  addSeguroMedico(seguroMedico: any): Observable<any[]> {
    return this.http.post<any>(this.apiUrl, seguroMedico);
  }

  updateSeguroMedico(id: string, seguroMedico: any): Observable<any[]> {
    seguroMedico.id = id;
    console.log(seguroMedico);
    return this.http.put<any>(`${this.apiUrl}/${id}`, seguroMedico);
  }

  deleteSeguroMedico(id: number): Observable<any[]> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
