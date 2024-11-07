import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { from } from 'rxjs';
import { PruebaImagenComponent } from './prueba-imagen/prueba-imagen.component';
import { ConsultaMedicaComponent } from './components/medico/consulta-medica/consulta-medica.component';
import { LoginComponent } from './components/login/login.component';
import { authGuard } from './shared/auth.guard';
import { CitaMedicaComponent } from './components/medico/cita-medica/cita-medica.component';
import { HistoriaClinicaComponent } from './components/medico/historia-clinica/historia-clinica.component';
import { SeleccionEspecialidadComponent } from './components/medico/seleccion-especialidad/seleccion-especialidad.component';
  
const routes: Routes = [
  { path: 'prueba-imagen', component: PruebaImagenComponent },
  { path: 'consulta-medica', component: ConsultaMedicaComponent, canActivate: [authGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'prueba-imagen', component: PruebaImagenComponent },
  { path: 'consulta-medica', component: ConsultaMedicaComponent },
  { path: 'cita-medica', component: CitaMedicaComponent },
  { path: 'elegir-especialidad', component: SeleccionEspecialidadComponent },
  { path: 'historia-clinica', component: HistoriaClinicaComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
