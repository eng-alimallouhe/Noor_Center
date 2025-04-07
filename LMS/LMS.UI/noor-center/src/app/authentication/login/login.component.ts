import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RegisterService } from '../../services/authentication/register.service';
import { Router, RouterModule } from '@angular/router';
import { AlertComponent } from "../../shared/alert/alert.component";
import { AccountService } from '../../services/authentication/account.service';
import { TokenService } from '../../services/token/token.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    AlertComponent,
    CommonModule,
    RouterModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})

export class LoginComponent {
  loginForm!: FormGroup;
  showAlert: boolean = false;
  alertMessage: string = '';
  isEmailNotExists: boolean = false;
  isLoginNotValid: boolean = false;

  constructor(private fb: FormBuilder,
    private accountService: AccountService,
    private tokenService: TokenService,
    private router: Router) { }

  ngOnInit(): void {

    this.loginForm = this.fb.group({
      email: ['',
        [Validators.required, Validators.email]
      ],

      password: ['', [Validators.required]],

      rememberMe: [false]
    });
  }


  onSubmit(): void {
    if (this.loginForm.valid) {
      const loginModel = this.loginForm.value;
      this.isLoginNotValid = false;
      this.isEmailNotExists = false;
      this.accountService.login(loginModel).subscribe({
        next: (response) => {
          this.tokenService.saveAccessToken(response.accessToken);
          this.tokenService.saveRefreshToken(response.refreshToken);

          this.router.navigate(["books/all"]);
        },
        error: (error) => {
          if (error.status === 401) {
            this.showMessage("sorry, you account is locked for 15 minutes");
          } else if (error.status === 404) {
            this.isEmailNotExists = true;
          } else if (error.status === 401) {
            this.isLoginNotValid = true;
          }
        }
      });

    }
  }

  showMessage(msg: string): void {
    this.alertMessage = msg;
    this.showAlert = true;
  }

  onAlertClosed(): void {
    this.showAlert = false;
  }

}
