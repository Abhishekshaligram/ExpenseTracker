
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import {Router, RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { AuthService, ToastService, TokenService } from 'src/services';
import { ShowErrorComponent } from 'src/shared/show-error/show-error.component';


@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [RouterModule,ButtonModule,ShowErrorComponent,DialogModule,ReactiveFormsModule,FormsModule,CommonModule],
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export  class ChangePasswordComponent {
  form: FormGroup;
  changePasswordForm: FormGroup = this.fb.group({});
  submitted = false;
  IsShowPassword: boolean = false;
  IsShowConfirmPassword: boolean = false;
  IsShowOldPassword: boolean = false;
  EncryptedUserId: any;
  currentUrlTime: any;
  roleValue: any;

  constructor(
    private fb: FormBuilder,
    private _apiService: AuthService,
    private route: Router,
    private toast: ToastService,
    private tokenService: TokenService,
  ) { }

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    this.changePasswordForm = this.fb.group(
      {
        oldpassword: ["", Validators.required],
        password: [
          "",
          [Validators.required, Validators.maxLength(16), Validators.pattern(/^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{8,16}$/)]
        ],
        confirmPassword: ["", Validators.required],
      },
      {
        validator: this.matchPasswords("password", "confirmPassword"),
      }
    );
  }

  get FormControl() {
    return this.changePasswordForm.controls;
  }

  async changePassword() {
    this.submitted = true;
    if (
      this.changePasswordForm.invalid ||
      !this.changePasswordForm.value.confirmPassword ||
      !this.changePasswordForm.value.password
    ) {
      this.changePasswordForm.markAllAsTouched();
      return;
    }
    let PasswordChangeRequestModel = {
      OldPassword: this.changePasswordForm.value.oldpassword,
      CreatePassword: this.changePasswordForm.value.password,
      ConfirmPassword: this.changePasswordForm.value.confirmPassword,
    };
    this._apiService.changePassword(PasswordChangeRequestModel).subscribe({
      next: (response) => {
        if (response['success']) {
          this.route.navigateByUrl("/login/login");
          this.toast.showSuccess(response.message);
        } else {
          this.toast.showError(response.message);
        }
      },
      error: (errorMessage) => {
        this.toast.showError(errorMessage);
      },
    });
  }

  togglePasswordVisibility() {
    this.IsShowPassword = !this.IsShowPassword;
  }

  toggleConfirmPasswordVisibility() {
    this.IsShowConfirmPassword = !this.IsShowConfirmPassword;
  }

  toggleOldPasswordVisibility() {
    this.IsShowOldPassword = !this.IsShowOldPassword;
  }

  backToDashboard() {
      this.route.navigate(["/default"]);
    
  }

  matchPasswords(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];
  
      if (matchingControl.errors && !matchingControl.errors['matchPasswords']) {
        return null; // No errors
      }
  
      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ matchPasswords: true });
      } else {
        matchingControl.setErrors(null);
      }
    };
  };
 }
