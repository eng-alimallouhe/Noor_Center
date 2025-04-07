import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RegisterService } from '../../services/authentication/register.service';
import { OtpCodeRequestModel } from './otp-code-request.model';
import { TokenService } from '../../services/token/token.service';
import { AlertComponent } from "../../shared/alert/alert.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-verify-modal',
  templateUrl: './verify-otp-code.component.html',
  styleUrls: ['./verify-otp-code.component.css'],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    AlertComponent
  ]
})
export class VerifyOtpCodeComponent implements OnInit {
  purpose: string = "";
  codeForm!: FormGroup;
  hasError: boolean = false;
  errorMessage: string = "";
  showAlert = false;
  alertMessage = '';

  verifyCodeDTO: OtpCodeRequestModel = new OtpCodeRequestModel("", "");

  @Output() codeVerified = new EventEmitter<boolean>();

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private registerService: RegisterService,
    private tokenService: TokenService,
    private route: ActivatedRoute) {

  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.purpose = params['purpose'];
      this.verifyCodeDTO.email = params['email']
    });

    this.codeForm = this.fb.group({
      email: [{
        value: this.verifyCodeDTO.email,
        disabled: true
      },
      [
        Validators.required,
        Validators.email
      ]],

      code: ['',
        [Validators.minLength(6), Validators.maxLength(6), Validators.required]
      ]

    })


  }


  onSubmit() {
    if (this.codeForm.valid) {
      this.verifyCodeDTO.code = this.codeForm.get("code")?.value;
      this.registerService.verifyRegister(this.verifyCodeDTO).subscribe({
        next: (response) => {
          this.tokenService.saveAccessToken(response.accessToken);
          this.tokenService.saveRefreshToken(response.refreshToken);

          this.router.navigate(["books/all"]);
        },

        error: (errorResponse) => {
          if (errorResponse.status === 401) {
            this.showMessage("The verification code has expired or is invalid. You will be redirected to the registration page.");

            setTimeout(() => {
              this.router.navigate(["authentication/register"]);
            }, 5000);
          } else if (errorResponse.status === 400) {
            this.showMessage("The code you entered is incorrect. Please try again.");
          } else {
            this.showMessage("An unexpected error occurred. Please try again later.");
          }
        }
      }
      );
    }
  }

  showMessage(msg: string): void {
    this.alertMessage = msg;
    this.showAlert = true;
  }

  onAlertClosed(): void {
    this.showAlert = false;
  }

  getNewCode(){
    
  }

}
