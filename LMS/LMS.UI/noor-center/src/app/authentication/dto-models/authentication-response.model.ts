import { UserResponseModel } from "./user-response.model";

export interface AuthenticationResponseModel{
    accessToken: string;
    refreshToken: string;
    user: UserResponseModel
}