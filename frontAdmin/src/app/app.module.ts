import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginComponent } from './components/login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthInterceptor } from './shared/auth.interceptor';
import { NavComponent } from './components/web/nav/nav.component';
import { GenerarMedicoComponent } from './components/generar-medico/generar-medico.component';
import { AddEspecialidadComponent } from './components/add-especialidad/add-especialidad.component';
import { ListMedicosComponent } from './components/list-medicos/list-medicos.component';
import { ConsultoriosComponent } from './components/consultorios/consultorios.component';
import { CopagosComponent } from './components/copagos/copagos.component';
import { ArticulosComponent } from './components/articulos/articulos.component';
import { SeguroMedicoSelectComponent } from './components/seguro-medico-select/seguro-medico-select.component';
import { PacientesComponent } from './components/pacientes/pacientes.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NavComponent,
    GenerarMedicoComponent,
    AddEspecialidadComponent,
    ListMedicosComponent,
    ConsultoriosComponent,
    CopagosComponent,
    ArticulosComponent,
    SeguroMedicoSelectComponent,
    PacientesComponent,
  ],
  imports: [
    BrowserModule,
    CommonModule,
    AppRoutingModule,
    HttpClientModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule, // required animations module
    ToastrModule.forRoot(), // ToastrModule added
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