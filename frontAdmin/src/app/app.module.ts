import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms'; 
import { ReactiveFormsModule } from '@angular/forms';
import { NavComponent } from './components/web/nav/nav.component';
import { GenerarMedicoComponent } from './components/generar-medico/generar-medico.component';
import { AddEspecialidadComponent } from './components/add-especialidad/add-especialidad.component';
import { ListMedicosComponent } from './components/list-medicos/list-medicos.component';
import { ConsultoriosComponent } from './components/consultorios/consultorios.component';
import { CopagosComponent } from './components/copagos/copagos.component';
import { ArticulosComponent } from './components/articulos/articulos.component';
@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    GenerarMedicoComponent,
    AddEspecialidadComponent,
    ListMedicosComponent,
    ConsultoriosComponent,
    CopagosComponent,
    ArticulosComponent,
  ],
  imports: [
    BrowserModule,
    CommonModule,
    AppRoutingModule,
    HttpClientModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [], 
  bootstrap: [AppComponent]
})
export class AppModule { }