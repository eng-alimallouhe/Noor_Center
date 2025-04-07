import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthenticationResponseModel } from '../../authentication/dto-models/authentication-response.model';
import { LoginRequestModel } from '../../authentication/dto-models/login-request.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private apiUrl: string = "http://localhost:5116/api/Account";

  constructor(private http: HttpClient) { }

  login(loginModel: LoginRequestModel): Observable<AuthenticationResponseModel>{
    return this.http.post<AuthenticationResponseModel>(`${this.apiUrl}/login`, loginModel);
  }
}
