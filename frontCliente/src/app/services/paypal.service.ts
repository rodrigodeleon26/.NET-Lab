import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaypalService {

  private apiUrl = 'https://localhost/gestion/api/payments'; // Reemplaza con la URL base de tu API

  constructor(private http: HttpClient) { }

  /**
   * Captura un pago por Order ID
   * @param orderId ID de la orden de pago a capturar
   * @returns Observable con la respuesta del servidor
   */
  capturePayment(orderId: string): Observable<any> {
    const url = `${this.apiUrl}/capture/${orderId}`;
    return this.http.post<any>(url, {});
  }
}