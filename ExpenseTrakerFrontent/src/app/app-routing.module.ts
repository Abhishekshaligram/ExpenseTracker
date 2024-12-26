import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './theme/layout/admin/admin.component';
import { GuestComponent } from './theme/layout/guest/guest.component';
import { AuthGuard } from './demo/pages/guards/auth.guard';

const routes: Routes = [
  {
    path: "login",
    loadChildren: () =>
      import("./demo/pages/authentication/authentication.module").then(
        (m) => m.AuthenticationModule,
      ),
    // canActivate: [AuthGuard],
  },
  {
    path: '',
    component: AdminComponent,
    children: [
      {
        path: '',
        redirectTo: '/login/login',
        pathMatch: 'full'
      },
      {
        path: 'default',
        loadComponent: () => import('./demo/default/default.component').then((c) => c.DefaultComponent)
      },
      {
        path: 'typography',
        loadComponent: () => import('./demo/elements/typography/typography.component')
      },
      {
        path: 'color',
        loadComponent: () => import('./demo/elements/element-color/element-color.component')
      },
      {
        path: 'sample-page',
        loadComponent: () => import('./demo/sample-page/sample-page.component')
      },
      {
        path: 'category-list',
        loadComponent: () => import('./demo/pages/ETM/categories/category-list/category-list.component').then((c)=>c.CategoryListComponent)
      },
      {
        path: 'user-list',
        loadComponent: () => import('./demo/pages/ETM/user/user-list/user-list.component').then((c)=>c.UserListComponent)
      },
      {
        path: 'expense-list',
        loadComponent: () => import('./demo/pages/ETM/expense/expense-list/expense-list.component').then((c)=>c.ExpenseListComponent)
      },
      {
        path: 'report-expense-list',
        loadComponent: () => import('./demo/pages/Report/monthly-expense-category-list/monthly-expense-category-list.component').then((c)=>c.MonthlyExpenseCategoryListComponent)
      },
      {
        path: 'expense-report',
        loadComponent: () => import('./demo/pages/Report/monthly-expense-report/monthly-expense-report.component').then((c)=>c.MonthlyExpenseReportComponent)
      },
      {
        path: 'budget-list',
        loadComponent: () => import('./demo/pages/Budget Management/budget-management-list/budget-management-list.component').then((c)=>c.BudgetManagementListComponent)
      },
      {
        path: 'change-password',
        loadComponent: () => import('./demo/pages/authentication/change-password/change-password.component').then((c)=>c.ChangePasswordComponent)
      },
    ],
    canActivate: [AuthGuard],
  },
  {
    path: '',
    component: GuestComponent,
    children: [
      {
        path: 'guest',
        loadChildren: () => import('./demo/pages/authentication/authentication.module').then((m) => m.AuthenticationModule)
      }
    ]
  },
  {
    path: "etm",
    loadChildren: () =>
      import("./demo/pages/ETM/etm.module").then(
        (m) => m.ETMModule,
      ),
     canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
