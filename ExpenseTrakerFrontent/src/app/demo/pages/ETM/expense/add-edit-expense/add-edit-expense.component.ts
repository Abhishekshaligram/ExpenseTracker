import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { forkJoin } from 'rxjs';
import { CommonService, ExpenseService, ToastService, UserService } from 'src/services';
import { ShowErrorComponent } from 'src/shared/show-error/show-error.component';

@Component({
  selector: 'app-add-edit-expense',
  standalone: true,
  imports: [ShowErrorComponent,DropdownModule,ReactiveFormsModule,CommonModule,ButtonModule],
  templateUrl: './add-edit-expense.component.html',
  styleUrl: './add-edit-expense.component.scss'
})
export class AddEditExpenseComponent {
  expenseForm:FormGroup
  expenseList:any
  categoryList:any=[];
  submitted:boolean
  userId:any=0

  constructor( private formBuilder: FormBuilder,
    private config: DynamicDialogConfig,
    private toast:ToastService,
    private modalRef: DynamicDialogRef,
    private commonservice:CommonService,
   private expenseservice:ExpenseService
  ){}

  ngOnInit() {
    this.formbuild();
    this.loadData();
    this.expenseList=this.config.data.id
    
  }

  formbuild() {
    this.expenseForm = this.formBuilder.group({
      categoryName: ["", [Validators.required]],
      Amount: ["", [Validators.required,Validators.pattern(/^\d+$/),Validators.maxLength(10)]],
      Description: ["", [Validators.required,Validators.maxLength(300)]],
    });
  }

  loadData() {
    forkJoin({
      categoryList: this.commonservice.getCategoryList(this.userId),
    }).subscribe({
      next: (response) => {
        this.categoryList = response.categoryList.data;
        if (this.expenseList.expenseID > 0) {
          this.patchDetails();
        }
      },
      error: (errorMessage) => {
        this.toast.showError(errorMessage);
      },
    });
  }

   getcategoryList(){
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


  OnSave(){
 this.submitted=true
 if( this.expenseForm.invalid){
   this.expenseForm.markAllAsTouched();
   return
 }else{
     let model={
      expenseId:this.expenseList.expenseID?this.expenseList.expenseID:0,
      userId:0,
      categoryId:this.expenseForm.get("categoryName").value.categoryID?this.expenseForm.get("categoryName").value.categoryID:"",
      amount:this.expenseForm.value.Amount?this.expenseForm.value.Amount:"",
      description:this.expenseForm.value.Description?this.expenseForm.value.Description:"",
     }
    this.expenseservice.AddEditExpense(model).pipe().subscribe({
      next: (response) => {
        if (response['errCode']) {
          this.toast.showSuccess(response['errMsg']);
          this.modalRef.close(true);
        } else {
          this.toast.showError(response['errMsg']);
        }
      },
      error: (error) => {
        this.toast.showError(error);
        this.modalRef.close(true);
      },
    })
 }
  }

  patchDetails(){
    this.expenseForm.patchValue({
      categoryName:this.categoryList.filter((cat)=>cat.categoryID===this.expenseList.categoryID)[0],
      Amount: this.expenseList.amount,
      Description:this.expenseList.description
    });
  }
}
