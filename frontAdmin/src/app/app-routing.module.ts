import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GenerarMedicoComponent } from './components/generar-medico/generar-medico.component';
import { ListMedicosComponent } from './components/list-medicos/list-medicos.component';
import { AddEspecialidadComponent } from './components/add-especialidad/add-especialidad.component';
import { ConsultoriosComponent } from './components/consultorios/consultorios.component';
import { CopagosComponent } from './components/copagos/copagos.component';
import { ArticulosComponent } from './components/articulos/articulos.component';

const routes: Routes = [
  { path: 'generarMedico', component: GenerarMedicoComponent },
  { path: 'generarMedico/:id', component: GenerarMedicoComponent },
  { path: 'listMedicos', component: ListMedicosComponent },
  { path: 'especialidades', component: AddEspecialidadComponent },
  { path: 'consultorios', component: ConsultoriosComponent },
  { path: 'copagos', component: CopagosComponent },
  { path: 'articulos', component: ArticulosComponent },//temporal quizas
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
