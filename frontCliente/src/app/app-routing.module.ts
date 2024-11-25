import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ResetPasswordComponent } from './components/user/reset-password/reset-password.component';
import { ConfirmEmailComponent } from './components/user/confirm-email/confirm-email.component';
import { ResendEmailConfirmationComponent } from './components/user/resend-email-confirmation/resend-email-confirmation.component';
import { authGuard } from './shared/auth.guard';
import { TwoFactorAuthComponent } from './components/user/two-factor-auth/two-factor-auth.component';
import { InicioComponent } from './components/pantallas/inicio/inicio.component';
import { HistoriaClinicaComponent } from './components/pantallas/historia-clinica/historia-clinica.component';
import { MisDatosComponent } from './components/pantallas/mis-datos/mis-datos.component';
import { NotificacionesComponent } from './components/pantallas/notificaciones/notificaciones.component';
import { HistorialFacturacionComponent } from './components/pantallas/historial-facturacion/historial-facturacion.component';
import { PaymentSuccessComponent } from './components/pantallas/payment-success/payment-success.component';

const routes: Routes = [
  { path: '', redirectTo: '/inicio', pathMatch: 'full' },
  { path: '', component: UserComponent,
    children: [
      { path: 'register', component: RegisterComponent },
      { path: 'login', component: LoginComponent },
      { path: 'resetPassword', component: ResetPasswordComponent },
      { path: 'confirmEmail', component: ConfirmEmailComponent, canActivate: [authGuard] },
      { path: 'resendEmailConfirmation', component: ResendEmailConfirmationComponent, canActivate: [authGuard] },
      { path: 'twoFactorAuth', component: TwoFactorAuthComponent, canActivate: [authGuard] }
    ]
  },
  { path : 'realizar-pago', component: HistorialFacturacionComponent, canActivate: [authGuard] },
  { path : 'payment/success', component: PaymentSuccessComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },
  { path : 'inicio', component: InicioComponent, canActivate: [authGuard] },
  { path : 'historia-clinica', component: HistoriaClinicaComponent, canActivate: [authGuard] },
  { path : 'mis-datos', component: MisDatosComponent, canActivate: [authGuard] },
  { path : 'notificaciones', component: NotificacionesComponent, canActivate: [authGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }