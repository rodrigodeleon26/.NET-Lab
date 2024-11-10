import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ArticulosService {

  private apiUrl = 'https://localhost/gestion/api/Articulos'; // URL del microservicio

  constructor(private http: HttpClient) { }

  getArticulos(): Observable<any[]> {
    console.log('Obteniendo articulos');
    return this.http.get<any[]>(this.apiUrl);
  }

  deleteArticulo(id: string): Observable<any> {
    console.log('Borrando articulo');
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }

  addArticulo(articulo: any): Observable<any> {
    console.log('Agregando articulo');
    console.log(articulo);
    return this.http.post<any>(this.apiUrl, articulo);
  }

  updateArticulo(id: string, articulo: any): Observable<any> {
    console.log('Actualizando articulo');
    //agrgarle el parametro id a la articulo
    articulo.id = id;
    return this.http.put<any>(`${this.apiUrl}/${id}`, articulo);    
  }

  getArticulo(id: string): Observable<any> {
    console.log('Obteniendo articulo');
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  getArticulosFiltrados(busqueda: string): Observable<any[]> {
    console.log('Obteniendo articulos filtrados');
    return this.http.get<any[]>(`${this.apiUrl}/filtro/${busqueda}`);
  }
}
