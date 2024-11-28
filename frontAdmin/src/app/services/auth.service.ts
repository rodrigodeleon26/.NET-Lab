import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment, REFRESH_TOKEN_KEY, TOKEN_KEY } from '../shared/constants';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http:HttpClient) { }
  loginUser(formData:any){
    return this.http.post(environment.AuthWebApiBaseUrl + '/auth/login', formData);
  }

  logout() {
    const refreshToken = this.getRefreshToken();
    return this.http.post(environment.AuthWebApiBaseUrl + '/auth/logout', { refreshToken });
  }

  refreshToken() {
    const token = this.getToken();
    const refreshToken = this.getRefreshToken();
    return this.http.post(environment.AuthWebApiBaseUrl + '/auth/refreshToken', { token, refreshToken });
  }

  isLoggedIn(){
    return this.getToken() != null ? true : false;
  }

  deleteToken(){
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
  }

  saveToken(token:string, refreshToken:string){
    localStorage.setItem(TOKEN_KEY, token);
    localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
  }

  getToken(){
    return localStorage.getItem(TOKEN_KEY);
  }

  getRefreshToken(){
    return localStorage.getItem(REFRESH_TOKEN_KEY);
  }

  getClaims(){
    return JSON.parse(atob(this.getToken()!.split('.')[1]));
  }
}
