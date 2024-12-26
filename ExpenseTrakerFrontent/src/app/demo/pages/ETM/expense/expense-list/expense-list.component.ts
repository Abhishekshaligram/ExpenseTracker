import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { TableModule } from "primeng/table";
import { DialogService, DynamicDialogRef } from "primeng/dynamicdialog";
import { AddEditExpenseComponent } from '../add-edit-expense/add-edit-expense.component';
import { SkeletonModule } from "primeng/skeleton";
import { CommonModule } from '@angular/common';
import { PrimeIcons } from 'primeng/api';
import { ExpenseService, SpinnerService, ToastService } from 'src/services';
import { CommonConfirmDialogComponent } from 'src/shared/common-confirm-dialog/common-confirm-dialog.component';
@Component({
  selector: 'app-expense-list',
  standalone: true,
  imports: [TableModule,ButtonModule,SkeletonModule,CommonModule],
  providers:[DialogService],
  templateUrl: './expense-list.component.html',
  styleUrl: './expense-list.component.scss'
})
export class ExpenseListComponent {
  expenseList:any[]=[];
  primeIcons: any;
  loading: boolean = false;
  header: string;
  ref: DynamicDialogRef | undefined;
  searchKeyword: string;
  isLoader: boolean
  pageSizeOptions: number[] = [5, 10, 25, 100];
  sortColumn: any;
  sortOrder: any;
  totalRecord: number = 0;
  first = 0;
  rows = 10;
  pagination: any = { per_page: 10 };
  pageNumber: number = 1;
  constructor(private dialogService: DialogService,
    private expenseservice:ExpenseService,
    private toast:ToastService,
    private spinnerservice: SpinnerService,
  ){}

  ngOnInit() {
    this.spinnerservice.showSpinner();
    this.primeIcons = PrimeIcons;
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
   this.GetExpenseList();
  }

  openModel(data: any) {
    if (data.expenseID > 0) {
      this.header = "Edit Expense";
    } else {
      this.header = "Add Expense";
    }
    this.ref = this.dialogService.open(AddEditExpenseComponent, {
      position: "middle",
      header: this.header,
      width:"30%",
      data: {
        id: data,
        title: this.header,
      },
    });
    this.ref.onClose.subscribe((data: any) => {
      if (data) {
        this.GetExpenseList();
      }
    });
  }

  GetExpenseList(){
    this.isLoader=true
    this.spinnerservice.showSpinner()
    let model={
     pageNumber: this.pageNumber,
     pageSize: this.pagination.per_page,
     sortColumn: this.sortColumn,
     sortOrder: this.sortOrder,
    }
    this.isLoader=true
    this.expenseservice.GetExpenseList(model).subscribe({
     next:(response)=>{
      this.expenseList=response.data
      this.totalRecord = response?.data[0]?.totalRecords;
      this.isLoader=false
      this.spinnerservice.hideSpinner();
     },
     error:(errorMessage)=>{
     this.toast.showError(errorMessage)
     this.isLoader=false
     this.spinnerservice.hideSpinner();
     }
    })
 }
   onDelete(expenseID:any){
    this.ref = this.dialogService.open(CommonConfirmDialogComponent, {
      position: "center",
      width:"20%",
      data: {
        visible: true,
        title: "Delete Confirmation",
        message: "Do you want to delete this record?",
        confirmButtonText: "Delete",
        cancelButtonText: "Cancel",
      },
    });
    this.ref.onClose.subscribe((result: any) => {
      if (result === 'confirm') {
        this.expenseservice.deleteExpense(expenseID).subscribe({
          next: (response) => {
            if (response['errCode']) {
              this.toast.showSuccess(response['errMsg']);
              if (this.expenseList.length === 1 && this.pageNumber > 1) {
                this.pageNumber--;
                this.first = (this.pageNumber - 1) * this.rows;
              }
              this.GetExpenseList();
            } else {
              this.toast.showError(response['errMsg']);
            }
          },
          error: (errorMessage) => {
            this.toast.showError(errorMessage);
          },
        });
      }
    });
   }
  }

