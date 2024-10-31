import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { PruebaImagenComponent } from './prueba-imagen/prueba-imagen.component';
import { NavComponent } from './components/web/nav/nav.component';
import { ConsultaMedicaComponent } from './components/medico/consulta-medica/consulta-medica.component';
import { FormsModule } from '@angular/forms'; 
import { NgSelectModule } from '@ng-select/ng-select';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './components/login/login.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { AuthInterceptor } from './shared/auth.interceptor';


@NgModule({
  declarations: [
    AppComponent,
    PruebaImagenComponent,
    NavComponent,
    ConsultaMedicaComponent,
    LoginComponent,
  ],
  imports: [
    BrowserModule,
    CommonModule,
    AppRoutingModule,
    HttpClientModule,
    RouterModule,
    NgSelectModule,
    FormsModule,
    ReactiveFormsModule,
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