import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { authGuard } from './shared/auth.guard';
import { AdminOnlyComponent } from './components/dashboard/admin-only/admin-only.component';
import { AdminOrMedicoComponent } from './components/dashboard/admin-or-medico/admin-or-medico.component';
import { AdminOrMedicoOrPacienteComponent } from './components/dashboard/admin-or-medico-or-paciente/admin-or-medico-or-paciente.component';
import { ResetPasswordComponent } from './components/user/reset-password/reset-password.component';

const routes: Routes = [
  { path: '', component: UserComponent,
    children: [
      { path: 'register', component: RegisterComponent },
      { path: 'login', component: LoginComponent },
      { path: 'resetPassword', component: ResetPasswordComponent },
    ]
  },

  {path: 'dashboard', component: DashboardComponent,
    canActivate: [authGuard]
  },

  {path: 'admin-only', component: AdminOnlyComponent,
    canActivate: [authGuard]
  },

  {path: 'admin-or-medico', component: AdminOrMedicoComponent,
    canActivate: [authGuard]
  },

  {path: 'admin-or-medico-or-paciente', component: AdminOrMedicoOrPacienteComponent,
    canActivate: [authGuard]
  },


  { path: '', redirectTo: '/login', pathMatch: 'full' }, 
  { path: '**', redirectTo: '/login' },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }