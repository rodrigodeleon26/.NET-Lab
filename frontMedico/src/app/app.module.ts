import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule, DatePipe } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { PruebaImagenComponent } from './prueba-imagen/prueba-imagen.component';
import { NavComponent } from './components/web/nav/nav.component';
import { ConsultaMedicaComponent } from './components/medico/consulta-medica/consulta-medica.component';
import { FormsModule } from '@angular/forms'; 
import { NgSelectModule } from '@ng-select/ng-select';
import { ReactiveFormsModule } from '@angular/forms';
import { CitaMedicaComponent } from './components/medico/cita-medica/cita-medica.component'; // Importa ReactiveFormsModule
import { HistoriaClinicaComponent } from './components/medico/historia-clinica/historia-clinica.component';
import { SeleccionEspecialidadComponent } from './components/medico/seleccion-especialidad/seleccion-especialidad.component'; 


@NgModule({
  declarations: [
    AppComponent,
    PruebaImagenComponent,
    NavComponent,
    ConsultaMedicaComponent,
    CitaMedicaComponent,
    HistoriaClinicaComponent,
    SeleccionEspecialidadComponent,
  ],
  imports: [
    BrowserModule,
    CommonModule,
    AppRoutingModule,
    HttpClientModule,
    RouterModule,
    NgSelectModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [DatePipe], 
  bootstrap: [AppComponent]
})
export class AppModule { }