// angular import
import { HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import {DropdownModule} from 'primeng/dropdown';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AuthService, CommonService, ToastService, TokenService } from 'src/services';
import { ShowErrorComponent } from 'src/shared/show-error/show-error.component';
import { AddEditUserComponent } from '../../ETM/user/add-edit-user/add-edit-user.component';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule,ReactiveFormsModule,FormsModule,ButtonModule,DropdownModule,ShowErrorComponent,HttpClientModule],
  // providers:[MessageService,ToastService],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export default class LoginComponent {
  loginForm!:FormGroup
  submitted = false;
  IsShowPassword: boolean = false;
  header: string;
  ref: DynamicDialogRef | undefined;
  constructor(
    private fb: FormBuilder,
    private _apiService: AuthService,
    private route: Router,
    private toast: ToastService,
   // private spinnerService: SpinnerService,
    private tokenService:TokenService,
    private Commonservice:CommonService,
    private dialogService: DialogService
  ){}

  ngOnInit(): void {  
    this.initForm();
  }

  initForm() {
    this.loginForm = this.fb.group({
      Email: [
        "",
        [
          Validators.required,
          Validators.email,
          Validators.pattern(
            /^(?=.{1,30}@)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/,
          ),
        ],
      ],
      password: ["", [Validators.required, Validators.maxLength(100)]],
    });
  }
  get loginFormControl() {
    return this.loginForm.controls;
  }
  async login() {
    this.submitted = true;
    if (
      this.loginForm.invalid ||
      !this.loginForm.value.Email ||
      !this.loginForm.value.password
    ) {
      this.loginForm.markAllAsTouched();
      return;
    }
    let LoginObj = {
      Email: this.loginForm.value.Email?.trim(),
      Password: this.loginForm.value.password?.trim(),
    };
   // this.spinnerService.showSpinner();
    this._apiService.loginUser(LoginObj).subscribe({
      next: (response) => {
        if (response['success']) {
          let userDetails = {
            userToken: response.data.token,
            UserID: response.data.userID,
            UserEmail: response.data.email,
            UserName: response.data.userName,
            UserImage: response.data.images,
          };
          this.tokenService.setUserDetails(userDetails);
          this.tokenService.setAuthToken(response.data.token);
          this.tokenService.getAuthToken();
          this.Commonservice.showPopup.next(true);
          this.route.navigate(['/default']);
        } else {
          //this.spinnerService.hideSpinner();
          this.toast.showError(response.message);
        }
      },
      error: (errorMessage) => {
      //  this.spinnerService.hideSpinner();
        this.toast.showError(errorMessage);
      },
    });
  }
  togglePasswordVisibility(){
    this.IsShowPassword = !this.IsShowPassword;
  }
  onForgotPassword(){
    this.route.navigate(["/login/forgot-password"]);
  }

  openModel(data: any) {
    debugger
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
        //this.GetUserList();
      }
    });
  }

}
