import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { PrimeIcons } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { SkeletonModule } from 'primeng/skeleton';
import { TableModule } from 'primeng/table';
import { AddEditUserComponent } from '../add-edit-user/add-edit-user.component';
import { SpinnerService, ToastService, UserService } from 'src/services';
import { CommonConfirmDialogComponent } from 'src/shared/common-confirm-dialog/common-confirm-dialog.component';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [TableModule,ButtonModule,SkeletonModule,CommonModule],
  providers:[DialogService],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss'
})
export class UserListComponent {
  userList:any[]=[];
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
    private userservice:UserService,
    private toast:ToastService,
    private spinnerservice: SpinnerService,
  ){}

  ngOnInit() {
    this.spinnerservice.showSpinner();
    this.primeIcons = PrimeIcons;
    this.GetUserList();
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
    this.GetUserList();
  }

  openModel(data: any) {
    if (data.userID > 0) {
      this.header = "Edit User";
    } else {
      this.header = "Add User";
    }
    this.ref = this.dialogService.open(AddEditUserComponent, {
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
        this.GetUserList();
      }
    });
  }

  GetUserList(){
    this.isLoader = true;
    this.spinnerservice.showSpinner();
     let model={
      pageNumber: this.pageNumber,
      pageSize: this.pagination.per_page,
      sortColumn: this.sortColumn,
      sortOrder: this.sortOrder,
     }
     this.isLoader = true;
     this.userservice.GetUserList(model).subscribe({
      next:(response)=>{
        this.spinnerservice.hideSpinner();
       this.userList=response.data
       this.totalRecord = response?.data[0]?.totalRecords;
       this.isLoader = false;
      },
      error:(errorMessage)=>{
        this.toast.showError(errorMessage)
        this.spinnerservice.hideSpinner();
        this.isLoader = false;
      }
     })
  }
  onDelete(userID:any){
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
        this.userservice.deleteUser(userID).subscribe({
          next: (response) => {
            if (response['errCode']) {
              this.toast.showSuccess(response['errMsg']);
              if (this.userList.length === 1 && this.pageNumber > 1) {
                this.pageNumber--;
                this.first = (this.pageNumber - 1) * this.rows;
              }
              this.GetUserList();
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

