import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GenerarMedicoComponent } from './components/generar-medico/generar-medico.component';
import { ListMedicosComponent } from './components/list-medicos/list-medicos.component';
import { AddEspecialidadComponent } from './components/add-especialidad/add-especialidad.component';

const routes: Routes = [
  { path: 'generarMedico', component: GenerarMedicoComponent },
  { path: 'generarMedico/:id', component: GenerarMedicoComponent },
  { path: 'listMedicos', component: ListMedicosComponent },
  { path: 'especialidades', component: AddEspecialidadComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
