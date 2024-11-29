import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ArticulosService {

  private apiUrl = 'https://localhost/gestion/api/articulos'; // URL del microservicio

  constructor(private http: HttpClient) { }

  getArticulosDelSeguro(cedula: string): Observable<any[]> {
    const url = `${this.apiUrl}/articulosHabilitados/${cedula}`;
    return this.http.get<any>(url);
  }
}
