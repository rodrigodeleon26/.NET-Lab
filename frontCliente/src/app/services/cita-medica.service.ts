import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CitaMedicaService {
  private apiUrl = 'https://localhost:5003/api/CitasMedicas';

  constructor(private http: HttpClient) { }

  AgendarCita(cedula: string, calendario: any, fecha: string, hora: string, articuloId: number, citaOnline: boolean): Observable<any[]> {
    //return this.http.post<any[]>(`${this.apiUrl}/${cedula}/articulo/${articuloId}/fecha/${fecha}`);
    const data = {
      cedula: cedula,
      calendarioId: calendario.id,
      fecha: fecha,
      hora: hora,
      articuloId: articuloId,
      citaOnline: citaOnline
    }
    console.log(data);
    return this.http.post<any[]>(`${this.apiUrl}/agendar`, data);
  }
}
