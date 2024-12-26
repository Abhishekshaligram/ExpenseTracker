import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "src/interfaces/apiresponse.interface";
@Injectable({
    providedIn: "root",
  })
export class AuthService{
    baseUrl='http://localhost:5147/'
    constructor(private http: HttpClient){}
    
    isLoggedIn(): boolean {
      return !!localStorage.getItem('user');
    }

    resetPassword(resetPasswordObj: any): Observable<ApiResponse> {
    const url=this.baseUrl+"api/login/success-forget-password";
    return this.http.post<ApiResponse>(url, resetPasswordObj);
    }

    sentResetPasswordLink(forgotPasswordObj: any): Observable<ApiResponse> {
        const url= this.baseUrl+"api/login/forget-password";
        return this.http.post<ApiResponse>(url, forgotPasswordObj);
      }

      loginUser(LoginObj: any): Observable<ApiResponse> {
        const url =  this.baseUrl+"api/login/login";
        return this.http.post<ApiResponse>(url, LoginObj);
      }
    
      changePassword(changePasswordObj: any): Observable<ApiResponse> {
        const url=  this.baseUrl+"api/login/change-password";
        return this.http.post<ApiResponse>(url, changePasswordObj);
      }

      logoutUser(): Observable<ApiResponse> {
        const url = this.baseUrl+"api/login/logout";
        return this.http.post<ApiResponse>(url, "");
      }

}
