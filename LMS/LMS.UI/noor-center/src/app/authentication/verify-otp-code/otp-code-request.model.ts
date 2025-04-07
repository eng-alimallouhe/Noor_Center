export class OtpCodeRequestModel {
    constructor(
        public email: string,
        public code: string
    ) { }
}