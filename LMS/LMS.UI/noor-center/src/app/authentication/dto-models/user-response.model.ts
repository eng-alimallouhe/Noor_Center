import { NotificationModel } from "../../users/dto-models/notification.model";

export interface UserResponseModel {
    userId: string;
    fullName: string;
    userName: string;
    email: string;
    createdAt: string;
    updatedAt: string;
    lastLogIn: string;
    notifications: NotificationModel[];
}