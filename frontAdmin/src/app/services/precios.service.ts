import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PreciosService {

  private apiUrl = 'https://localhost:5009/api/Precios'; // URL del microservicio

  constructor(private http: HttpClient) { }

  getPrecios(): Observable<any[]> {
    console.log('Obteniendo precios');
    return this.http.get<any[]>(this.apiUrl);
  }

  getPrecio(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  addPrecio(precio: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, precio);
  }

  updatePrecio(id:string, precio: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, precio);
  }

  deletePrecio(id: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
