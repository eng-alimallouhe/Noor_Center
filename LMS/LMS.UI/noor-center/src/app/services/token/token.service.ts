import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor() { }

  saveAccessToken(accessToken: string) {
    localStorage.setItem("accessToken", accessToken);
  }


  saveRefreshToken(refreshToken: string) {
    localStorage.setItem("refreshToken", refreshToken);
  }

  getAccessToken() {
    return localStorage.getItem("accessToken");
  }

  getRefreshToken() {
    return localStorage.getItem("refreshToken");
  }

}
