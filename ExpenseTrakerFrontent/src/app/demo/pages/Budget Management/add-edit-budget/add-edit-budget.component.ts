import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { forkJoin } from 'rxjs';
import { BudgetService, CommonService, ToastService } from 'src/services';
import { ShowErrorComponent } from 'src/shared/show-error/show-error.component';

@Component({
  selector: 'app-add-edit-budget',
  standalone: true,
  imports: [ShowErrorComponent,DropdownModule,ReactiveFormsModule,CommonModule,ButtonModule,CalendarModule],
  templateUrl: './add-edit-budget.component.html',
  styleUrl: './add-edit-budget.component.scss'
})
export class AddEditBudgetComponent {
  budgetForm:FormGroup
  budgetList:any
  submitted:boolean
  startMinDate: any;
  startMaxDate: any;
  endMaxDate: any;
  endMinDate: any;
  categoryList:any=[];
  userId:any=0
  constructor(private formBuilder: FormBuilder,
    private config: DynamicDialogConfig,
    private toast:ToastService,
    private modalRef: DynamicDialogRef,
    private commonService:CommonService,
    private budgetservice:BudgetService){}
   

  ngOnInit(){
    this.budgetList=this.config.data.id
    this.formbuild();
     this.loadData();
  }


  formbuild() {
    this.budgetForm = this.formBuilder.group({
      categoryName:["", [Validators.required]],
      Amount: ["", [Validators.required,Validators.pattern(/^\d+$/),Validators.maxLength(10)]],
      startdate: ["", [Validators.required]],
      enddate: ["", [Validators.required]],
    });
    this.budgetForm.get("startdate")?.valueChanges.subscribe((startDate) => {
      this.updateEndDateMinDate(startDate);
    });
  }

  
  loadData() {
    forkJoin({
      categoryList: this.commonService.getCategoryList(this.userId),
    }).subscribe({
      next: (response) => {
        this.categoryList = response.categoryList.data;
        if (this.budgetList.budgetID > 0) {
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
     this.commonService.getCategoryList(userId).pipe().subscribe({
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
  if(this.budgetForm.invalid){
    this.budgetForm.markAllAsTouched()
    return
  }else{
   let model={
    budgetID:this.budgetList.budgetID?this.budgetList.budgetID:0,
    userID:0,
    categoryID:this.budgetForm.get("categoryName").value.categoryID?this.budgetForm.get("categoryName").value.categoryID:"",
    amount:this.budgetForm.value.Amount?this.budgetForm.value.Amount:0,
    startDate:this.budgetForm.value.startdate?this.commonService.convertDate(this.budgetForm.value.startdate):null,
    endDate:this.budgetForm.value.enddate?this.commonService.convertDate(this.budgetForm.value.enddate):null,
   }
    this.budgetservice.AddEditBudget(model).subscribe({
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
    this.budgetForm.patchValue({
      categoryName:this.categoryList.filter((cat)=>cat.categoryID===this.budgetList.categoryID)[0],
      Amount: this.budgetList.amount,
      startdate:this.budgetList.startDate ? new Date(this.budgetList.startDate) : null,
      enddate:this.budgetList.endDate ? new Date(this.budgetList.endDate) : null,
    });
  }

  updateEndDateMinDate(startDate: Date) {
    if (startDate) {
      this.endMinDate = startDate;
      const endDateControl = this.budgetForm.get("enddate");
      if (endDateControl?.value && new Date(endDateControl.value) < new Date(startDate)) {
        endDateControl.setValue(null);
      }
    } else {
      this.endMinDate = null;
    }
  }
}
