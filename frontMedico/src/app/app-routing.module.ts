import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { from } from 'rxjs';
import { PruebaImagenComponent } from './prueba-imagen/prueba-imagen.component';
import { ConsultaMedicaComponent } from './components/medico/consulta-medica/consulta-medica.component';
import { LoginComponent } from './components/login/login.component';
import { authGuard } from './shared/auth.guard';

const routes: Routes = [
  { path: 'prueba-imagen', component: PruebaImagenComponent },
  { path: 'consulta-medica', component: ConsultaMedicaComponent, canActivate: [authGuard] },
  { path: 'login', component: LoginComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
