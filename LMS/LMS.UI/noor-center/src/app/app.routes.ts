import { Routes } from '@angular/router';
import { AUTH_ROUTES } from './authentication/authentication.routs';
import { VerifyOtpCodeComponent } from './authentication/verify-otp-code/verify-otp-code.component';
import { Books_ROUTES } from './books/books.routs';

export const routes: Routes = [
    {
        path: "authentication",
        children: AUTH_ROUTES
    },

    {
        path: "verify",
        component: VerifyOtpCodeComponent
    },

    {
        path: "books",
        children: Books_ROUTES
    }
];
