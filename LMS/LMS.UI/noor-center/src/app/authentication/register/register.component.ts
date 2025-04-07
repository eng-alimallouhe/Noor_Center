import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, AsyncValidatorFn, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { catchError, debounceTime, distinctUntilChanged, first, map, of, switchMap, tap } from 'rxjs';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { RegisterRequestModel } from '../dto-models/register-request.model';
import { RegisterService } from '../../services/authentication/register.service';
import { AlertComponent } from "../../shared/alert/alert.component";
import { TranslateLoader, TranslatePipe, TranslateService } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

@Component({
  selector: 'app-register',
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    TranslatePipe,
    ReactiveFormsModule,
    AlertComponent
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {

  registerForm!: FormGroup;
  isCheckingUsername: boolean = false;
  showAlert = false;
  alertMessage = '';

  constructor(private fb: FormBuilder,
    private registerService: RegisterService,
    private transalte: TranslateService,
    private router: Router) { }

  ngOnInit(): void {

    this.registerForm = this.fb.group({
      fullName: ['', [Validators.required]],

      username: this.fb.control(
        '',
        {
          validators: [Validators.required],
          asyncValidators: [this.usernameExistsValidator()],
          updateOn: 'blur'
        }
      ),

      email: ['',
        [Validators.required, Validators.email],
        [this.emailExistsValidator()]
      ],

      password: ['', [Validators.required, Validators.minLength(8)]],

      rememberMe: [false]
    });
  }


  usernameExistsValidator(): AsyncValidatorFn {
    return (control: AbstractControl) => {
      return control.value ? of(control.value).pipe(
        debounceTime(100),
        distinctUntilChanged(),
        switchMap(username => this.registerService.checkUsernameExists(username)),
        map(response => {
          if (response.isSuccess) {
            return null;
          }
          else {
            return { usernameTaken: true }
          }
        }),
        catchError(() => of(null)),
        first()
      ) : of(null);
    };
  }


  emailExistsValidator(): AsyncValidatorFn {
    return (control: AbstractControl) => {
      return control.value ? of(control.value).pipe(
        debounceTime(200),
        distinctUntilChanged(),
        switchMap(email => this.registerService.checkEmailExists(email)),
        map(response => {
          if (response.isSuccess) {
            return null;
          }
          else {
            return { emailTaken: true };
          }
        }),
        catchError(() => of(null)),
        first()
      ) : of(null);
    };
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      const registerRequest: RegisterRequestModel = this.registerForm.value;

      this.registerService.registerUser(registerRequest).subscribe(
        (response) => {
          if (response.isSuccess) {
            this.router.navigate(['/verify'], {
              queryParams: { email: registerRequest.email, purpose: 'register' }
            });
          } else {
            this.showMessage(response.message);
          }
        },
        (error) => {
          this.showMessage(error.message);
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

}

