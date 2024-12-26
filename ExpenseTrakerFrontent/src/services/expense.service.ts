import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, Subject } from "rxjs";
import { ApiResponse } from "src/interfaces/apiresponse.interface";
@Injectable({
    providedIn: "root",
  })
export class ExpenseService{
    baseUrl='http://localhost:5147/'
    constructor(private http: HttpClient){}
   
    GetExpenseList(model: any): Observable<ApiResponse> {
    const url=this.baseUrl+"api/Expense/getexpenselist";
    return this.http.post<ApiResponse>(url, model);
    }

    AddEditExpense(model: any): Observable<ApiResponse> {
        const url=this.baseUrl+"api/Expense/addeditexpense";
        return this.http.post<ApiResponse>(url, model);
    }

    deleteExpense(expenseId: any): Observable<ApiResponse> {
    const url=`${this.baseUrl}api/Expense/delete/${expenseId}`;
    return this.http.delete<ApiResponse>(url);
    }

    GetmonthlyExpenseList(model: any): Observable<ApiResponse> {
        const url=this.baseUrl+"api/Expense/getexpensebycategory";
        return this.http.post<ApiResponse>(url, model);
        }

   GetmonthlyExpenseReport(model: any): Observable<ApiResponse> {
    const url=this.baseUrl+"api/Expense/getmonthlyexpense";
    return this.http.post<ApiResponse>(url, model); 
  }

  ExportmonthlyExpenseReport(model: any): Observable<ApiResponse> {
    const url=this.baseUrl+"api/Expense/monthlyexpensereport";  
    return this.http.post<ApiResponse>(url, model); 
  }
}