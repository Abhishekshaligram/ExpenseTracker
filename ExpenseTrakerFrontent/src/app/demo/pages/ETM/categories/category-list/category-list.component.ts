import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { PrimeIcons } from 'primeng/api';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AddEditCategoryComponent } from '../add-edit-category/add-edit-category.component';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { SkeletonModule } from 'primeng/skeleton';
import { CommonModule } from '@angular/common';
import { CategoryService, SpinnerService, ToastService } from 'src/services';
import { CommonConfirmDialogComponent } from 'src/shared/common-confirm-dialog/common-confirm-dialog.component';

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [RouterModule,TableModule,ButtonModule,SkeletonModule,CommonModule],
  providers:[DialogService],
  templateUrl: './category-list.component.html',
  styleUrl: './category-list.component.scss'
})
export class CategoryListComponent {
  categoryList:any[]=[];
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
    private categryservice:CategoryService,
    private toast:ToastService,
    private spinner:SpinnerService
  ){}

  ngOnInit() {
    this.spinner.showSpinner();
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
    this.GetCategoryList();
  }

  openModel(data: any) {
    if (data.categoryID > 0) {
      this.header = "Edit Category";
    } else {
      this.header = "Add Category";
    }
    this.ref = this.dialogService.open(AddEditCategoryComponent, {
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
        this.GetCategoryList();
      }
    });
  }

   GetCategoryList(){
    this.isLoader=true
    this.spinner.showSpinner();
    let model={
      pageNumber: this.pageNumber,
      pageSize: this.pagination.per_page,
      sortColumn: this.sortColumn,
      sortOrder: this.sortOrder,
     }
     this.isLoader=true
    this.categryservice.GetCategoryList(model).subscribe({
      next:(response)=>{
      this.categoryList=response.data
      this.totalRecord = response?.data[0]?.totalRecords;
      this.isLoader=false
      this.spinner.hideSpinner();
      },
      error:(errorMessage)=>{
     this.toast.showError(errorMessage)
     this.isLoader=false
     this.spinner.hideSpinner();
      }
    })
   }

  onDelete(categoryID:any){
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
        this.categryservice.deleteCategory(categoryID).subscribe({
          next: (response) => {
            if (response['errCode']) {
              this.toast.showSuccess(response['errMsg']);
              if (this.categoryList.length === 1 && this.pageNumber > 1) {
                this.pageNumber--;
                this.first = (this.pageNumber - 1) * this.rows;
              }
              this.GetCategoryList();
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
