import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { GenerarMedicoComponent } from './components/generar-medico/generar-medico.component';
import { ListMedicosComponent } from './components/list-medicos/list-medicos.component';
import { AddEspecialidadComponent } from './components/add-especialidad/add-especialidad.component';
import { ConsultoriosComponent } from './components/consultorios/consultorios.component';
import { CopagosComponent } from './components/copagos/copagos.component';
import { ArticulosComponent } from './components/articulos/articulos.component';
import { PacientesComponent } from './components/pacientes/pacientes.component';
  
const routes: Routes = [
  { path: 'login', component: LoginComponent},

  { path: 'generarMedico', component: GenerarMedicoComponent },
  { path: 'generarMedico/:id', component: GenerarMedicoComponent },
  { path: 'listMedicos', component: ListMedicosComponent },
  { path: 'especialidades', component: AddEspecialidadComponent },
  { path: 'consultorios', component: ConsultoriosComponent },
  { path: 'copagos', component: CopagosComponent },
  { path: 'articulos', component: ArticulosComponent },//temporal quizas
  { path: 'pacientes', component: PacientesComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
