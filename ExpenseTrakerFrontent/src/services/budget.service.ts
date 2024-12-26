import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, Subject } from "rxjs";
import { ApiResponse } from "src/interfaces/apiresponse.interface";
@Injectable({
    providedIn: "root",
  })
export class BudgetService{
    baseUrl='http://localhost:5147/'
    constructor(private http: HttpClient){}
   

    GetBudgetListList(model: any): Observable<ApiResponse> {
    const url=this.baseUrl+"api/budget/getbudgetlist";
    return this.http.post<ApiResponse>(url, model);
    }

    AddEditBudget(model: any): Observable<ApiResponse> {
        const url=this.baseUrl+"api/budget/addeditbudget";
        return this.http.post<ApiResponse>(url, model);
    }

   deleteBudget(budgetId: any): Observable<ApiResponse> {
   const url=`${this.baseUrl}api/budget/delete/${budgetId}`;
   return this.http.delete<ApiResponse>(url);
  }
}