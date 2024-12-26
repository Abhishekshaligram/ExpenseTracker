namespace PracticeCrud.Common.CommonMethods
{
    public static class ErrorMessages
    {
        //user
        public const string InvalidCredential = "Invalid email ID or password";
        public const string InvalidEmailId = "Email ID is not valid.";
        public const string validEmailId = "Email ID is valid.";
        public const string NotRegisterEmailId = "Email ID not register with us.";
        public const string AccountDeactive = "Your account has been deactivated, please contact to admin.";
        public const string FirstNameRequired = "First name is required";
        public const string CurrentPassword = "Please enter correct current password";
        public const string DiffPassword = "Current password & new password should not be the same. Please enter a different password.";
        public const string ChangePasswordSuccessfully = "Change password successfully !";
        public const string ValidateEmailPassword = "Email address or password is not valid.";
        public const string Tag = "Tag is required.";
        public const string AccountDeactivated = "Your account has been inactivated,please contact to admin.";
        public const string UserIsDeletedByAdmin = "Email doesn't exist.";
        public const string EmailNotExist = "You are not registered with us.";



        //Login
        public const string LoginSuccess = "Logged in successfully";
        public const string LoginError = "User not authorized";
        public const string ForgetPasswordSuccessEmail = "Reset password link send successfully. Please check inbox.";
        public const string EmailVerifyOrWait = "Please check your email is correct or try after sometime sorry for inconvenience in service.";

        //general 
        public const string Error = "An error occured";
        public const string SomethingWentWrong = "Something went wrong. Please try again later.";

        //forgot apassword
        public const string UrlForPasswordChangeExpired = "Link is expired please request in next 10 min for forget password change request.";
        public const string URLAlreadyUsed = "You can use reset password link only once.";
        public const string UrlIsValid = "URL is valid.";
        public const string ResetPasswordSuccess = "Password reset successfully";
        public const string PasswordFieldValidation = "One or more fields are required.";
        public const string PasswordCheck = "Please enter valid old Password.";
        public const string PasswordMatch = "New password can't be same as old password.";
        public const string PasswordValidation = "Both password and confirm password are required";
        public const string ConfirmPassword = "Password and confirmation password does not match";
        public const string StrongPassword = "Please enter strong password";
        public const string UserLogoutSuccess = "User logged out successfully";
        public const string UserLogoutError = "An error occurred while logging out user";
        public const string UserInsertedSuccess = "User added succesfully";
        public const string UserInsertedFail = "Failed to add user! Please try again";
        public const string UserUpdateSuccess = "User updated succesfully";
    }
}
