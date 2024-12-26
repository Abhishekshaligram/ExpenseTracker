import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DropdownModule } from 'primeng/dropdown';
import { ShowErrorComponent } from 'src/shared/show-error/show-error.component';
import { CheckboxModule } from 'primeng/checkbox';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ButtonModule } from 'primeng/button';
import { ToastService, UserService } from 'src/services';

@Component({
  selector: 'app-add-edit-user',
  standalone: true,
  imports: [ShowErrorComponent,DropdownModule,ReactiveFormsModule,CommonModule,CheckboxModule,ButtonModule],
  templateUrl: './add-edit-user.component.html',
  styleUrl: './add-edit-user.component.scss'
})
export class AddEditUserComponent {
  UserForm:FormGroup;
  submitted:boolean;
  userList:any
  constructor( private formBuilder: FormBuilder,
    private config: DynamicDialogConfig,
    private userservice:UserService,
    private toast:ToastService,
    private modalRef: DynamicDialogRef,

  ){}

  ngOnInit() {
    this.formbuild();
    this.userList=this.config.data.id
    if (this.userList.userID > 0) {
      this.patchDetails();
    }
  }


  formbuild() {
    this.UserForm = this.formBuilder.group({
      userName: ["", [Validators.required,Validators.pattern(/^[a-zA-Z\s]+$/)]],
      Email: ["", [Validators.required,Validators.email]],
      isActive:[true]
    });
  }

  OnSave(){
    debugger
    this.submitted=true;
    if(this.UserForm.invalid){
      this.UserForm.markAllAsTouched();
      return
    }else{
      let model={
        userID:this.userList.userID?this.userList.userID:0,
        userName:this.UserForm.value.userName,
        email:this.UserForm.value.Email,
        isActive:this.UserForm.value.isActive === true
      }
      this.userservice.AddEditUser(model).pipe()
      .subscribe({
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

  patchDetails() {
    this.UserForm.setValue({
      userName: this.userList.userName,
      Email: this.userList.email,
      isActive:this.userList.isActive !== undefined ? this.userList.isActive : true
    });
  }
}
