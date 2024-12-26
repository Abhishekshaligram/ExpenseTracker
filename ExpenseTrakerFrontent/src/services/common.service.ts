import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, Subject } from "rxjs";
import { ApiResponse } from "src/interfaces/apiresponse.interface";
@Injectable({
    providedIn: "root",
  })
export class CommonService{
    baseUrl='http://localhost:5147/'
    constructor(private http: HttpClient){}
    showPopup = new Subject<boolean>();
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

    getCategoryList(userId:number): Observable<ApiResponse> {
        const url =  this.baseUrl+`api/general/getcategorylist/${userId}`;
        return this.http.get<ApiResponse>(url);
      }

      getUserList(): Observable<ApiResponse> {
        const url =  this.baseUrl+"api/general/getuserlist";
        return this.http.get<ApiResponse>(url);
      }
      getCategoryListForDashboard(userId:number): Observable<ApiResponse> {
        const url =  this.baseUrl+`api/general/getcategoryfordashboard/${userId}`;
        return this.http.get<ApiResponse>(url);
      }

      convertDate(date: string | Date) {
        return new Date(new Date(date).getTime() + 24 * 60 * 60 * 1000)
          .toISOString()
          .split("T")[0]; 
      }

      
  saveProfilePicture(model: any): Observable<ApiResponse> {
    const url = this.baseUrl+"api/general/SaveUserProfile";
    return this.http.post<ApiResponse>(url, model);
  }

  getUserProfileImage(userId: number): Observable<ApiResponse> {
    const url = this.baseUrl+`api/general/details/${userId}`;
    return this.http.get<ApiResponse>(url);
  }
}
