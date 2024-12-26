import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { AuthService, ToastService } from 'src/services';
import { ShowErrorComponent } from 'src/shared/show-error/show-error.component';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [RouterModule,ReactiveFormsModule,FormsModule,ButtonModule,ShowErrorComponent,CommonModule],
  providers:[MessageService,ToastService],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss'
})
export class ResetPasswordComponent {
  form: FormGroup;
  resetPasswordForm: FormGroup = this.fb.group({});
  submitted = false;
  IsShowPassword: boolean = false;
  IsShowConfirmPassword: boolean = false;
  EncryptedUserId: any;
  currentUrlTime: any;
  constructor(
    private fb: FormBuilder,
    private _apiService: AuthService,
    private route: Router,
    private router: ActivatedRoute,
    private toast: ToastService,
   // private spinnerService: SpinnerService,
  ) { }

  ngOnInit() {
    this.EncryptedUserId = this.router.snapshot.paramMap.get("Id");
    this.currentUrlTime = this.router.snapshot.paramMap.get("time");
    this.initForm();
  }

  initForm() {
    this.resetPasswordForm = this.fb.group(
      {
        password: [
          "",
          Validators.required,
          Validators.maxLength(16),
          Validators.pattern(
            /^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{8,16}$/,
          ),
        ],
        confirmPassword: ["", Validators.required],
      },
      {
        validator: this.matchPasswords("password", "confirmPassword"),
      },
    );
  }

  get FormControl() {
    return this.resetPasswordForm.controls;
  }

  async resetPassword() {
    this.submitted = true;
    if (
      this.resetPasswordForm.invalid ||
      !this.resetPasswordForm.value.confirmPassword ||
      !this.resetPasswordForm.value.password
    ) {
      this.resetPasswordForm.markAllAsTouched();
      return;
    }
    if (
      this.resetPasswordForm.value.confirmPassword ===
      this.resetPasswordForm.value.password
    ) {
      let PasswordChangeRequestModel = {
        EncryptedUserId: this.EncryptedUserId,
        Password: this.resetPasswordForm.value.password?.trim(),
        IsButtonClicked: true,
        DateTime: this.currentUrlTime,
      };
     // this.spinnerService.showSpinner();
      this._apiService.resetPassword(PasswordChangeRequestModel).subscribe({
        next: (response) => {
          if (response['success']) {
            //this.spinnerService.hideSpinner();
            this.route.navigateByUrl("/login/login");
            this.toast.showSuccess(response.message);
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
    } else {
      return;
    }
  }

  togglePasswordVisibility() {
    this.IsShowPassword = !this.IsShowPassword;
  }

  toggleConfirmPasswordVisibility() {
    this.IsShowConfirmPassword = !this.IsShowConfirmPassword;
  }

  backToLogin() {
    this.route.navigate(["/login/login"]);
  }

  matchPasswords(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];

      if (matchingControl.errors && !matchingControl.errors['matchPasswords']) {
        return;
      }

      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ matchPasswords: true });
      } else {
        matchingControl.setErrors(null);
      }
    };
  }
}
