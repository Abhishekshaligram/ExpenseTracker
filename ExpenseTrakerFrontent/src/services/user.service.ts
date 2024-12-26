import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, Subject } from "rxjs";
import { ApiResponse } from "src/interfaces/apiresponse.interface";
@Injectable({
    providedIn: "root",
  })
export class UserService{
    baseUrl='http://localhost:5147/'
    constructor(private http: HttpClient){}
   

    GetUserList(model: any): Observable<ApiResponse> {
    const url=this.baseUrl+"api/user/getuserlist";
    return this.http.post<ApiResponse>(url, model);
    }

    AddEditUser(model: any): Observable<ApiResponse> {
        const url=this.baseUrl+"api/user/addedituser";
        return this.http.post<ApiResponse>(url, model);
    }

 deleteUser(userId: any): Observable<ApiResponse> {
 const url=`${this.baseUrl}api/user/delete/${userId}`;
 return this.http.delete<ApiResponse>(url);
}
}