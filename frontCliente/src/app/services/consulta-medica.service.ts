import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConsultaMedicaService {
  private apiUrl = 'https://localhost/historiaclinica/api/HistoriasClinicas';

  constructor(
    private http: HttpClient
  ) { }

  getEspecialidades(): Observable<any> {
    const url = `https://localhost/gestion/api/Especialidades`;
    console.log(url);
    return this.http.get<any>(url);
  }
}
