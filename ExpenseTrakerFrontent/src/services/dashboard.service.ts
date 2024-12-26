import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, Subject } from "rxjs";
import { ApiResponse } from "src/interfaces/apiresponse.interface";
@Injectable({
    providedIn: "root",
  })
export class DashboardService{
    baseUrl='http://localhost:5147/'
    constructor(private http: HttpClient){}
   
    getExpenseForCard(userId: any): Observable<ApiResponse> {
        const url = `${this.baseUrl}api/budget/getexpenseforcard`;
        const payload = { userId }; 
        return this.http.post<ApiResponse>(url, payload);
    }

    getExpenseForGraph(userId: any): Observable<ApiResponse>{
        const url = `${this.baseUrl}api/Expense/getexpensebycategoryforgraph`;
        const payload = { userId }; 
        return this.http.post<ApiResponse>(url, payload);
    }

    expensePDFDownload(userId: number): Observable<ApiResponse> {
        const url = `${this.baseUrl}api/general/expensepdf?userId=${userId}`;
        return this.http.post<ApiResponse>(url, {}); 
    }
   
}