import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConsultorioService {
  private apiUrl = '/gestion/api/Consultorios';

  constructor(private http: HttpClient) {}

  getConsultorioGestion(id: number): Observable<any[]> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<any[]>(url);
  }
}
