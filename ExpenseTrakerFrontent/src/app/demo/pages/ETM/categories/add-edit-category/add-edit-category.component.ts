import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { ShowErrorComponent } from 'src/shared/show-error/show-error.component';
import {InputTextareaModule} from 'primeng/inputtextarea';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { CategoryService, ToastService, UserService } from 'src/services';

@Component({
  selector: 'app-add-edit-category',
  standalone: true,
  imports: [ShowErrorComponent,DropdownModule,ReactiveFormsModule,CommonModule,ButtonModule,InputTextareaModule],
  templateUrl: './add-edit-category.component.html',
  styleUrl: './add-edit-category.component.scss'
})
export class AddEditCategoryComponent {
  CategoryForm:FormGroup;
  categoryList:any;
  submitted:boolean
  constructor( private formBuilder: FormBuilder,
    private config: DynamicDialogConfig,
    private userservice:UserService,
    private toast:ToastService,
    private modalRef: DynamicDialogRef,
    private categoryservice:CategoryService
  ){}

  ngOnInit() {
    this.formbuild();
    this.categoryList=this.config.data.id
    if (this.categoryList.categoryID > 0) {
      this.patchDetails();
    }
  }

  formbuild() {
    this.CategoryForm = this.formBuilder.group({
      categoryName: ["", [Validators.required,Validators.pattern(/^[a-zA-Z\s]+$/)]],
      Description: ["", [Validators.required,Validators.maxLength(300)]],
    });
  }
  OnSave(){
  this.submitted=true
  if( this.CategoryForm.invalid){
    this.CategoryForm.markAllAsTouched();
    return
  }else{
      let model={
        categoryID:this.categoryList.categoryID?this.categoryList.categoryID:0,
        categoryName:this.CategoryForm.value.categoryName?this.CategoryForm.value.categoryName:"",
        description:this.CategoryForm.value.Description?this.CategoryForm.value.Description:"",
        userId:0
      }
      this.categoryservice.AddEditUser(model).pipe().subscribe({
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
      });
  }
  }


  patchDetails(){
    this.CategoryForm.patchValue({
      categoryName: this.categoryList.categoryName,
      Description: this.categoryList.description,
    });
  }
}
