import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { SkeletonModule } from 'primeng/skeleton';
import { TableModule } from 'primeng/table';
import { CommonService, ExpenseService, SpinnerService, ToastService } from 'src/services';
import { Months } from '../../Constants/enum';
import { CalendarModule } from 'primeng/calendar';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-monthly-expense-report',
  standalone: true,
  imports: [TableModule,ButtonModule,SkeletonModule,CommonModule,DropdownModule,FormsModule,CalendarModule],
  providers:[DialogService],
  templateUrl: './monthly-expense-report.component.html',
  styleUrl: './monthly-expense-report.component.scss'
})
export class MonthlyExpenseReportComponent {
  monthlyExpenseReport:any[]=[];
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
 // userName:any;
  UserList:any[]=[];
  monthList:any[]=[]
  monthName:any;
  dateValue:any;
  exportModel:any
  constructor(private expenseservice:ExpenseService,
    private toast:ToastService,
    private commonservice:CommonService,
    private spinner:SpinnerService
  ){}


  ngOnInit(){
    this.spinner.showSpinner()
     this.monthList=Months
     this.getUserList();
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
    this.GetMonthlyReport();
  }
  
  getUserList(){
   
    this.commonservice.getUserList().pipe().subscribe({
      next:(response)=>{
      this.UserList=response.data;
     
      },
      error:(errorMessage)=>{
       this.toast.showError(errorMessage)
       
      }
    })
  }

  onChange(){
   this.GetMonthlyReport();
  }


  resetFilters(){
    //this.userName=null;
    this.monthName=null;
    this.dateValue=null;
    this.GetMonthlyReport();
  }


  GetMonthlyReport(){
    this.spinner.showSpinner()
   let model={
    pageNumber: this.pageNumber,
    pageSize: this.pagination.per_page,
    sortColumn: this.sortColumn,
    sortOrder: this.sortOrder,
   // userID:this.userName?.userID?this.userName?.userID:0,
    userID:0,
    year:this.dateValue ? this.dateValue.getFullYear() : 0,
    month:this.monthName?.value?this.monthName?.value:0
   }
   this.exportModel=model
   this.expenseservice.GetmonthlyExpenseReport(model).subscribe({
     next:(response)=>{
      this.monthlyExpenseReport=response.data
      this.totalRecord = response?.data[0]?.totalRecords;
      this.spinner.hideSpinner()

     },
     error:(errorMessage)=>{
      this.toast.showError(errorMessage)
      this.spinner.hideSpinner()

     }
   })

  }

  downloadExpenseReport(){
    this.spinner.showSpinner()
    this.expenseservice.ExportmonthlyExpenseReport(this.exportModel).subscribe({
      next: (response) => {
        if (response['success']) {
          const base64String = response.data.fileData;
          const byteCharacters = atob(base64String);
          const byteNumbers = new Array(byteCharacters.length);
          for (let i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
          }
          const byteArray = new Uint8Array(byteNumbers);
          const blob = new Blob([byteArray], { type: "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
          const link = document.createElement("a");
          link.href = window.URL.createObjectURL(blob);
          link.download = response.data.fileName;
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
        } else {
          this.toast.showError("Something Went Wrong");
        }
        this.spinner.hideSpinner()
      },
      error: (errorMessage) => {
        this.toast.showError(errorMessage);
        this.spinner.hideSpinner()
      },
    });
  }
}
