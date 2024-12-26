import { HttpEvent, HttpHandler, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { TokenService } from "src/services";

@Injectable({
  providedIn: "root",
})
export class TokenInterceptor {
  constructor(public _tokenService:TokenService ) { }
  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler,
  ): Observable<HttpEvent<unknown>> {
    return next.handle(
      request.clone({
        headers: request.headers.append(
          "Authorization",
          this._tokenService.getAuthToken()
            ? "Bearer " + this._tokenService.getAuthToken()
            : "",
        ),
        withCredentials: true,
      }),
    );
  }
}
