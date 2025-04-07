import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { RegisterRequestModel } from '../../authentication/dto-models/register-request.model';
import { OtpCodeRequestModel } from '../../authentication/verify-otp-code/otp-code-request.model';
import { RegisterResponseModel } from '../../authentication/dto-models/register-response.model';
import { VerifyInformationModel } from '../../authentication/dto-models/verify-information.model';
import { AuthenticationResponseModel } from '../../authentication/dto-models/authentication-response.model';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  private apiUrl: string = "http://localhost:5116/api/Register";

  constructor(private http: HttpClient) { }

  checkEmailExists(email: string) {
    return this.http.get<VerifyInformationModel>(`${this.apiUrl}/verify-email/${email}`);
  }

  checkUsernameExists(username: string) {
    return this.http.get<VerifyInformationModel>(`${this.apiUrl}/verify-username/${username}`);
  }


  registerUser(data: RegisterRequestModel): Observable<RegisterResponseModel> {
    return this.http.post<RegisterResponseModel>(`${this.apiUrl}`, data);
  }

  verifyRegister(otpCodeModel: OtpCodeRequestModel): Observable<AuthenticationResponseModel> {
    return this.http.post<AuthenticationResponseModel>(`${this.apiUrl}/verify-register`, otpCodeModel);
  }
}
