import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TOKEN_KEY } from '../shared/constants';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http:HttpClient) { }
  baseUrl = 'https://localhost:5007/api/auth';

  registerUser(formData:any){
    return this.http.post(this.baseUrl + '/register', formData);
  }

  loginUser(formData:any){
    return this.http.post(this.baseUrl + '/login', formData);
  }

  isLoggedIn(){
    return localStorage.getItem(TOKEN_KEY) != null ? true : false;
  }

  deleteToken(){
    localStorage.removeItem(TOKEN_KEY);
  }

  saveToken(token:string){
    localStorage.setItem(TOKEN_KEY, token);
  }
}
