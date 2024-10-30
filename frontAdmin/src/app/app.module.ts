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
@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    GenerarMedicoComponent,
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