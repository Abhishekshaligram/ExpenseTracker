import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { SkeletonModule } from 'primeng/skeleton';
import { TableModule } from 'primeng/table';
import { AddEditBudgetComponent } from '../add-edit-budget/add-edit-budget.component';
import { PrimeIcons } from 'primeng/api';
import { DropdownModule } from 'primeng/dropdown';
import { CalendarModule } from 'primeng/calendar';
import { BudgetService, CommonService, SpinnerService, ToastService } from 'src/services';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonConfirmDialogComponent } from 'src/shared/common-confirm-dialog/common-confirm-dialog.component';

@Component({
  selector: 'app-budget-management-list',
  standalone: true,
  imports: [TableModule,ButtonModule,SkeletonModule,CommonModule,DropdownModule,CalendarModule,FormsModule,ReactiveFormsModule,FormsModule],
  providers:[DialogService],
  templateUrl: './budget-management-list.component.html',
  styleUrl: './budget-management-list.component.scss'
})
export class BudgetManagementListComponent {
  BudgetList:any[]=[];
  primeIcons: any;
  loading: boolean = false;
  header: string;
  ref: DynamicDialogRef | undefined;
  isLoader: boolean 
  pageSizeOptions: number[] = [5, 10, 25, 100];
  sortColumn: any;
  sortOrder: any;
  totalRecord: number = 0;
  first = 0;
  rows = 10;
  pagination: any = { per_page: 10 };
  pageNumber: number = 1;
  categoryList:any[]=[];
  categoryName:any;
  UserList:any[]=[];
  //userName:any
  startMinDate: any;
  startMaxDate: any;
  endMaxDate: any;
  endMinDate: any;
  startdate:any
  enddate:any;
  showFilters: boolean = true;
  minDate:Date;
  managementForm:FormGroup
 constructor(private dialogService:DialogService,
  public commonService:CommonService,
  private commonservice:CommonService,
  private toast:ToastService,
  private budgetservice:BudgetService,
  private fb:FormBuilder,
  private spinner:SpinnerService
 ){}

 ngOnInit() {
  this.spinner.showSpinner()
  this.primeIcons = PrimeIcons;
  this.formBuild();
  this.getUserList();
  this.getCategoryList();
}


formBuild(){
 this.managementForm=this.fb.group({
  //userName:[""],
  categoryName: [""],
  startdate: [""],
  enddate: [""],
 });
 this.managementForm.get("startdate")?.valueChanges.subscribe((startDate) => {
  this.updateEndDateMinDate(startDate);
});
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
    this.getBudgetList();
  }

  openModel(data: any) {
    if (data.userID > 0) {
      this.header = "Edit Budget";
    } else {
      this.header = "Add Budget";
    }
    this.ref = this.dialogService.open(AddEditBudgetComponent, {
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
        this.getBudgetList();
      }
    });
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

  getCategoryList(){
    let userId=0
    this.commonservice.getCategoryList(userId).pipe().subscribe({
      next:(response)=>{
      this.categoryList=response.data;
      },
      error:(errorMessage)=>{
       this.toast.showError(errorMessage)
      }
    })
  }

  filter(event: any) {
    this.pageNumber = 1;
    this.getBudgetList();
  }


getBudgetList(){
  this.isLoader=true
  this.spinner.showSpinner()
  let model={
    pageNumber: this.pageNumber,
    pageSize: this.pagination.per_page,
    sortColumn: this.sortColumn,
    sortOrder: this.sortOrder,
    // userID:this.managementForm.get("userName").value?.userID?this.managementForm.get("userName").value?.userID:0,
    userID:0,
    categoryID:this.managementForm.get("categoryName").value?.categoryID?this.managementForm.get("categoryName").value?.categoryID:0,
    startDate:this.managementForm.get("startdate").value?this.commonService.convertDate(this.managementForm.get("startdate").value):null,
    endDate:this.managementForm.get("enddate").value?this.commonService.convertDate(this.managementForm.get("enddate").value):null,  
  }
  this.isLoader=true
  this.budgetservice.GetBudgetListList(model).subscribe({
    next:(response)=>{
      this.BudgetList=response.data
      this.totalRecord = response?.data[0]?.totalRecords;
      this.isLoader=false
      this.spinner.hideSpinner()
     },
     error:(errorMessage)=>{
      this.toast.showError(errorMessage)
      this.isLoader=false
      this.spinner.hideSpinner()
     }
  })
   
}


resetFilters(){
  this.managementForm.reset();
   this.getBudgetList();
}


ShowHideFilters(){
  this.showFilters = !this.showFilters; 
}

onDelete(budgetID:any){
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
      this.budgetservice.deleteBudget(budgetID).subscribe({
        next: (response) => {
          if (response['errCode']) {
            this.toast.showSuccess(response['errMsg']);
            if (this.BudgetList.length === 1 && this.pageNumber > 1) {
              this.pageNumber--;
              this.first = (this.pageNumber - 1) * this.rows;
            }
            this.getBudgetList();
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

updateEndDateMinDate(startDate: Date) {
  if (startDate) {
    this.endMinDate = startDate;
    const endDateControl = this.managementForm.get("enddate");
    if (endDateControl?.value && new Date(endDateControl.value) < new Date(startDate)) {
      endDateControl.setValue(null);
    }
  } else {
    this.endMinDate = null;
  }
}

}
