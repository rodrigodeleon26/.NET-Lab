import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
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
import { HistoriaClinicaComponent } from './components/medico/historia-clinica/historia-clinica.component'; 


@NgModule({
  declarations: [
    AppComponent,
    PruebaImagenComponent,
    NavComponent,
    ConsultaMedicaComponent,
    HistoriaClinicaComponent,
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
  providers: [], 
  bootstrap: [AppComponent]
})
export class AppModule { }