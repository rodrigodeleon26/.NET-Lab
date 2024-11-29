import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class FacturaService {
  private apiUrl = 'https://localhost/gestion/api/Facturas'; // URL del microservicio

  constructor(private http: HttpClient, private router: Router,) {}

  downloadFacturasPdf(ids: number[]): Observable<Blob> {
    const url = `${this.apiUrl}/pdf/lista`;
    return this.http.post(url, ids, { responseType: 'blob' });
  }

  getFacturasByPaypal(paypalOrderId: string): Observable<any[]> {
    const url = `${this.apiUrl}/paypal/${paypalOrderId}`;
    return this.http.get<any[]>(url);
  }

  getFacturasPaginadas(
    numPagina: number,
    fechaAsc: boolean = true,
    estaPago?: boolean,
    cedula?: string
  ): Observable<any[]> {
    const navigation = this.router.getCurrentNavigation();
    const pacienteString = cedula || navigation?.extras.state?.['cedula'] || ''; // http://localhost:4200/cliente/historial-facturas

    if (pacienteString == '') {
      return of([]);
    }
  
    let params = new HttpParams()
      .set('fechaAsc', fechaAsc.toString())
      .set('pacienteString', pacienteString);
  
    if (estaPago !== undefined) {
      params = params.set('estaPago', estaPago.toString());
    }
  
    const url = `${this.apiUrl}/pagina/${numPagina}`;
    return this.http.get<any[]>(url, { params });
  }

  updateFactura(id: number, factura: any): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.put<any>(url, factura);
  }
}
