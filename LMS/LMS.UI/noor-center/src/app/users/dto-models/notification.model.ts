export interface NotificationModel {
    title: string;
    message: string;
    isRead: boolean;
    ReadAt: string | null;
    RedirectUrl: string;
}