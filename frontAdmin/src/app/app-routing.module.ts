import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GenerarMedicoComponent } from './components/generar-medico/generar-medico.component';

const routes: Routes = [
  { path: 'generarMedico', component: GenerarMedicoComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
