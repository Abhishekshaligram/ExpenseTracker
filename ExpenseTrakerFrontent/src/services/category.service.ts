import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, Subject } from "rxjs";
import { ApiResponse } from "src/interfaces/apiresponse.interface";
@Injectable({
    providedIn: "root",
  })
export class CategoryService{
    baseUrl='http://localhost:5147/'
    constructor(private http: HttpClient){}
   

    GetCategoryList(model: any): Observable<ApiResponse> {
    const url=this.baseUrl+"api/category/getcategorylist";
    return this.http.post<ApiResponse>(url, model);
    }

    AddEditUser(model: any): Observable<ApiResponse> {
        const url=this.baseUrl+"api/category/addeditcategory";
        return this.http.post<ApiResponse>(url, model);
    }

 deleteCategory(categoryId: any): Observable<ApiResponse> {
 const url=`${this.baseUrl}api/category/delete/${categoryId}`;
 return this.http.delete<ApiResponse>(url);
}
}