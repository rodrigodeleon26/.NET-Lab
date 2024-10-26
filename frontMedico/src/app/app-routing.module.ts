import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { from } from 'rxjs';
import { PruebaImagenComponent } from './prueba-imagen/prueba-imagen.component';
import { ConsultaMedicaComponent } from './components/medico/consulta-medica/consulta-medica.component';

const routes: Routes = [
  { path: 'prueba-imagen', component: PruebaImagenComponent },
  { path: 'consulta-medica', component: ConsultaMedicaComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
