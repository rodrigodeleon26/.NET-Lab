import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FacturasService {

  private apiUrl = 'https://localhost/gestion/api/Facturas';

  constructor(private http: HttpClient) { }

  getFacturas(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  getFacturasPaginadas(numPagina: number, pacienteString?: string, fechaAsc: boolean = true, estaPago?: boolean): Observable<any[]> {
    let params = new HttpParams()
      .set('numPagina', numPagina.toString())
      .set('fechaAsc', fechaAsc.toString());

    if (pacienteString) {
      params = params.set('pacienteString', pacienteString);
    }
    if (estaPago !== undefined) {
      params = params.set('estaPago', estaPago.toString());
    }

    const url = `${this.apiUrl}/pagina/${numPagina}`;
    return this.http.get<any[]>(url, { params });
  }

  getFacturaById(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<any>(url);
  }

  addFactura(factura: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, factura);
  }

  editFactura(id: number, factura: any): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.put<any>(url, factura);
  }

  deleteFactura(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.delete<any>(url);
  }
}