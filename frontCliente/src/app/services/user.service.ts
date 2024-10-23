import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../shared/constants'; // Importar environment desde constants.ts
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  getDatosPersonales() {
    return this.http.get(environment.AuthWebApiBaseUrl + '/prueba/datosPersonales')
  }
}