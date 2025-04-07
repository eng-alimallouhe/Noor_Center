import { RegisterRequestModel } from "./register-request.model";

export interface RegisterResponseModel {
    isSuccess: boolean;
    message: string;
    entity: RegisterRequestModel;
}