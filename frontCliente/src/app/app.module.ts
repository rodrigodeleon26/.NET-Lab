import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule, provideHttpClient, withInterceptors } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AuthInterceptor } from './shared/auth.interceptor';
import { ResetPasswordComponent } from './components/user/reset-password/reset-password.component';
import { ConfirmEmailComponent } from './components/user/confirm-email/confirm-email.component';
import { ResendEmailConfirmationComponent } from './components/user/resend-email-confirmation/resend-email-confirmation.component';
import { TwoFactorAuthComponent } from './components/user/two-factor-auth/two-factor-auth.component';
import { NavComponent } from '../app/components/web/nav/nav.component';
import { InicioComponent } from './components/pantallas/inicio/inicio.component';
import { HistoriaClinicaComponent } from './components/pantallas/historia-clinica/historia-clinica.component'; 

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    LoginComponent,
    RegisterComponent,
    DashboardComponent,
    ResetPasswordComponent,
    ConfirmEmailComponent,
    ResendEmailConfirmationComponent,
    TwoFactorAuthComponent,
    NavComponent,
    InicioComponent,
    HistoriaClinicaComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule,
    FormsModule, // Agrega FormsModule aquí
    BrowserAnimationsModule, 
    ToastrModule.forRoot(), 
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }