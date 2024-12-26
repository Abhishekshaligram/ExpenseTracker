import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import LoginComponent from './login/login.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { ChangePasswordComponent } from './change-password/change-password.component';

const routes: Routes = [
  // {  
  //   path: '',
  //   children: [
  //     {
  //       path: 'login',
  //       loadComponent: () => import('./login/login.component')
  //     },
  //     {
  //       path: 'register',
  //       loadComponent: () => import('./register/register.component')
  //     }
  //   ]
  // }
  {
    path: "login",
    component: LoginComponent,
  },
  {
    path: "forgot-password",
    component: ForgotPasswordComponent,
  },
  {
    path: "resetpassword/:Id/:time",
    component: ResetPasswordComponent,
  },
  // {
  //   path: "changepassword",
  //   component: ChangePasswordComponent,
  // },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthenticationRoutingModule {}
