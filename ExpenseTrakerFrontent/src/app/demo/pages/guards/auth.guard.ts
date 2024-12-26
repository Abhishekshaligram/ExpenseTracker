import { Injectable } from "@angular/core";
import {
  ActivatedRouteSnapshot,
  Router,
  RouterStateSnapshot,
} from "@angular/router";
import { ToastService, TokenService } from "src/services";


@Injectable({ providedIn: "root" })
export class AuthGuard {
  constructor(
    private _token:TokenService,
    private _router: Router,
    private _tost:ToastService,
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const accessToken = this._token.getAuthToken();
    if (route.routeConfig?.path === 'login') {
        return true;
      }
    if (accessToken) {
        return true;
    } else {
      this._router.navigateByUrl("/login/login");
      return false;
    }
  }
}
