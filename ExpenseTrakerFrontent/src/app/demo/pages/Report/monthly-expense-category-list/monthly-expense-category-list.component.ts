import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { SkeletonModule } from 'primeng/skeleton';
import { TableModule } from 'primeng/table';
import { ExpenseService, SpinnerService, ToastService } from 'src/services';

@Component({
  selector: 'app-monthly-expense-category-list',
  standalone: true,
  imports: [TableModule,ButtonModule,SkeletonModule,CommonModule],
  providers:[DialogService],
  templateUrl: './monthly-expense-category-list.component.html',
  styleUrl: './monthly-expense-category-list.component.scss'
})
export class MonthlyExpenseCategoryListComponent {
  primeIcons: any;
  loading: boolean = false;
  header: string;
  ref: DynamicDialogRef | undefined;
  isLoader: boolean = false;
  pageSizeOptions: number[] = [5, 10, 25, 100];
  sortColumn: any;
  sortOrder: any;
  totalRecord: number = 0;
  first = 0;
  rows = 10;
  pagination: any = { per_page: 10 };
  pageNumber: number = 1;
  monthlyExpenseList:any[]=[];
 
  constructor(private expenseservice:ExpenseService,
    private toast:ToastService,
    private spinner:SpinnerService
  ){}
  ngOnInit(){
   this.spinner.showSpinner();
  }
  loadData(event: any) {
    if (event) {
      this.pagination.per_page = event.rows;
      this.pagination.next_page = event.first / event.rows + 1;
      this.pageNumber = event.first / event.rows + 1
      this.first = event.first;
      this.rows = event.rows;
      this.sortOrder =
        event.multiSortMeta != undefined
          ? event.multiSortMeta[0].order == 1
            ? "asc"
            : "desc"
          : event.sortOrder === 1
            ? "asc"
            : "desc";
      this.sortColumn = event.sortField;
    }
    this.getMonthlyExpenseList();
  }

   getMonthlyExpenseList(){
    this.spinner.showSpinner();
    let model={
      pageNumber: this.pageNumber,
      pageSize: this.pagination.per_page,
      sortColumn: this.sortColumn,
      sortOrder: this.sortOrder,
      userID:0
    }
    this.expenseservice.GetmonthlyExpenseList(model).subscribe({
      next:(response)=>{
      this.monthlyExpenseList=response.data;
      this.totalRecord = response?.data[0]?.totalRecords;
      this.spinner.hideSpinner();
      },
      error:(errorMessage)=>{
      this.toast.showError(errorMessage)
      this.spinner.hideSpinner();
      }
    })
   }
}
