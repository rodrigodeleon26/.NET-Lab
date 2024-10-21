import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TOKEN_KEY } from '../shared/constants';
import { environment } from '../shared/constants';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http:HttpClient) { }

  registerUser(formData:any){
    return this.http.post(environment.AuthWebApiBaseUrl + '/auth/register', formData);
  }

  loginUser(formData:any){
    return this.http.post(environment.AuthWebApiBaseUrl + '/auth/login', formData);
  }

  isLoggedIn(){
    return this.getToken() != null ? true : false;
  }

  deleteToken(){
    localStorage.removeItem(TOKEN_KEY);
  }

  saveToken(token:string){
    localStorage.setItem(TOKEN_KEY, token);
  }

  getToken(){
    return localStorage.getItem(TOKEN_KEY);
  }
}
