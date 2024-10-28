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

const routes: Routes = [
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
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }