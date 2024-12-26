import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { CookieService } from "ngx-cookie-service";
import * as CryptoJS from "crypto-js";


@Injectable({
  providedIn: "root",
})
export class TokenService {
    secretKey:any= "1a2b3c4d5e6f7g8h9i0j1k2l3m4n5o6p7q8r9s0t1u2v3w4x5y6z7a8b9c0d1e2f";
    tokenKey:any="token";
    userdetails: any;
    token: any;
    constructor(
        private http: HttpClient,
        private cookieService: CookieService,
      ) {}

      setAuthToken(token: string) {
        const expireDays = 1; 
        this.cookieService.set(
          this.tokenKey,
          token,
          expireDays,
          "/",
          "",
          true,
          "Strict",
        );
      }

      getAuthToken(): string {
        try {
          this.userdetails = this.getUserDetails();
          if (this.userdetails != null) {
            this.token = this.userdetails.userToken;
          } else {
            this.token = null;
          }
          return this.token;
        } catch (error) {
          return "";
        }
      }

  setEncryptedValue(key: string, value: any): void {
        const encryptedValue = CryptoJS.AES.encrypt(
          JSON.stringify(value),
          this.secretKey,
        ).toString();
        localStorage.setItem(key, encryptedValue);
      }

 getDecryptedValue(key: string): any {
        const encryptedValue = localStorage.getItem(key);
        if (!encryptedValue) {
          return null;
        }
        const bytes = CryptoJS.AES.decrypt(encryptedValue, this.secretKey);
        const decryptedValue = JSON.parse(bytes.toString(CryptoJS.enc.Utf8));
        return decryptedValue;
      }

  setUserDetails(details: any):  void {
    this.setEncryptedValue("userDetails", details);
   
  }
  getUserDetails(): any {
    return this.getDecryptedValue("userDetails");
  }

  clearAuthToken() {
    try {
      localStorage.removeItem("token");
    } catch (error) {}
  }
  clearStorage(): void {
    localStorage.clear();
  }
}