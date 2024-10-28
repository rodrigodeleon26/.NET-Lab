import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { REFRESH_TOKEN_KEY, TOKEN_KEY } from '../shared/constants';
import { environment } from '../shared/constants';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private twoFactorAuthenticated: boolean = false;

  constructor(private http:HttpClient) { }

  registerUser(formData:any){
    return this.http.post(environment.AuthWebApiBaseUrl + '/auth/register', formData);
  }
  
  loginUser(formData:any){
    return this.http.post(environment.AuthWebApiBaseUrl + '/auth/login', formData);
  }
  
  refreshToken() {
    const token = this.getToken();
    const refreshToken = this.getRefreshToken();
    return this.http.post(environment.AuthWebApiBaseUrl + '/auth/refreshToken', { token, refreshToken });
  }
  
  forgotPassword(email: string) {
    return this.http.post(environment.AuthWebApiBaseUrl + '/auth/forgotPassword', { email });
  }

  resetPassword(email: string, token: string, newPassword: string) {
    return this.http.post(environment.AuthWebApiBaseUrl + '/auth/resetPassword', { email, token, newPassword });
  }

  confirmEmail(email: string, token: string) {
    return this.http.post(environment.AuthWebApiBaseUrl + '/auth/confirmEmail', { email, token });
  }

  resendEmailConfirmation(email: string) {
    return this.http.post(environment.AuthWebApiBaseUrl + '/auth/resendConfirmationEmail', { email });
  }

  generateQrCode(email: string)  {
    return this.http.post(environment.AuthWebApiBaseUrl + '/auth/generateQrCode', { email });
  }

  validateTwoFactorCode(email: string, code: string) {
    return this.http.post(environment.AuthWebApiBaseUrl + '/auth/validateTwoFactorCode', { email, code });
  }

  enableTwoFactorAuth(email: string) {
    return this.http.post(environment.AuthWebApiBaseUrl + '/auth/enableTwoFactorAuth', { email });
  }

  disableTwoFactorAuth(email: string) {
    return this.http.post(environment.AuthWebApiBaseUrl + '/auth/disableTwoFactorAuth', { email });
  }

  isLoggedIn(){
    return this.getToken() != null ? true : false;
  }

  deleteToken(){
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    this.setTwoFactorAuthenticated(false);
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

  getEmailConfirmedStatus(): boolean {
    const claims = this.getClaims();
    return claims ? claims.emailConfirmed === 'True' : false;
  }

  getEmail(): string {
    const claims = this.getClaims();
    return claims ? claims.email : '';
  }

  getTwoFactorEnabledStatus(): boolean {
    const claims = this.getClaims();
    return claims ? claims.TwoFactorEnabled === 'True' : false;
  }

  setTwoFactorAuthenticated(status: boolean) {
    this.twoFactorAuthenticated = status;
  }

  isTwoFactorAuthenticated(): boolean {
    return this.twoFactorAuthenticated;
  }
}
