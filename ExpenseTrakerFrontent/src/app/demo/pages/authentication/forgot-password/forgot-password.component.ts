import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { AuthService, ToastService } from 'src/services';
import { ShowErrorComponent } from 'src/shared/show-error/show-error.component';
import { DialogModule } from "primeng/dialog";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [ButtonModule,ShowErrorComponent,DialogModule,RouterModule,ReactiveFormsModule,FormsModule,CommonModule],
  providers:[MessageService,ToastService],
  templateUrl:'./forgot-password.component.html',
  styleUrl: './forgot-password.component.scss'
})
export class ForgotPasswordComponent {
  form: FormGroup;
  display: boolean = false;
  forgotPasswordForm: FormGroup = this.fb.group({});
  submitted = false;
  constructor(
    private fb: FormBuilder,
    private _apiService: AuthService,
    private route: Router,
    private toast: ToastService,
  //  private spinnerService: SpinnerService,
  ) { }

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    this.forgotPasswordForm = this.fb.group({
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
    });
  }

  get FormControl() {
    return this.forgotPasswordForm.controls;
  }

  async getResetPasswordLink() {
    this.submitted = true;
    if (
      this.forgotPasswordForm.invalid ||
      !this.forgotPasswordForm.value.Email
    ) {
      this.forgotPasswordForm.markAllAsTouched();
      return;
    }
    let forgotPasswordObj = {
      email: this.forgotPasswordForm.value.Email?.trim(),
      forgetPasswordUrl: window.location.origin + "/login/resetpassword",
    };
   // this.spinnerService.showSpinner();
    this._apiService.sentResetPasswordLink(forgotPasswordObj).subscribe({
      next: (response) => {
        if (response['success']) {
          //this.spinnerService.hideSpinner();
          this.display = true;
        } else {
         // this.spinnerService.hideSpinner();
          this.toast.showError(response.message);
        }
      },
      error: (errorMessage) => {
       // this.spinnerService.hideSpinner();
        this.toast.showError(errorMessage);
      },
    });
  }

  backToLogin() {
    this.route.navigate(["login/login"]);
  }

  backToForgotPassword() {
    this.display = false;
  }
}
