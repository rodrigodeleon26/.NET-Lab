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
import { ContratosComponent } from './components/contratos/contratos.component';
import { CalendariosComponent } from './components/calendarios/calendarios.component';
import { FacturasComponent } from './components/facturas/facturas.component';
import { InicioComponent } from './components/inicio/inicio.component';
import { authGuard } from './shared/auth.guard';
  
const routes: Routes = [
  { path: 'login', component: LoginComponent},

  { path: '', redirectTo: '/inicio', pathMatch: 'full'},
  { path: 'inicio', component: InicioComponent, canActivate: [authGuard] },
  { path: 'generarMedico', component: GenerarMedicoComponent, canActivate: [authGuard] },
  { path: 'generarMedico/:id', component: GenerarMedicoComponent, canActivate: [authGuard] },
  { path: 'listMedicos', component: ListMedicosComponent, canActivate: [authGuard] },
  { path: 'especialidades', component: AddEspecialidadComponent, canActivate: [authGuard] },
  { path: 'consultorios', component: ConsultoriosComponent, canActivate: [authGuard] },
  { path: 'copagos', component: CopagosComponent, canActivate: [authGuard] },
  { path: 'articulos', component: ArticulosComponent, canActivate: [authGuard] },//temporal quizas
  { path: 'pacientes', component: PacientesComponent, canActivate: [authGuard] },
  { path: 'contratos', component: ContratosComponent, canActivate: [authGuard] },
  { path: 'calendarios', component: CalendariosComponent, canActivate: [authGuard] },
  { path: 'facturas', component: FacturasComponent, canActivate: [authGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
