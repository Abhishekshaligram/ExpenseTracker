import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';


const routes: Routes = [
  {
    path: "category-list",
    loadChildren: () =>
      import("./categories/category.module").then((m) => m.CategoryModule),
  },
  // {
  //   path: "expense-list",
  //   loadChildren: () =>
  //     import("./expense/expense.module").then((m) => m.ExpenseModule),
  // },
  // {
  //   path: "user-list",
  //   loadChildren: () =>
  //     import("./user/user.module").then((m) => m.UserModule),
  // },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ETMRoutingModule {}
