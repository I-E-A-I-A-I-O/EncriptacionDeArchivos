export class RequestError {
    readonly message: string;
    statusCode?: number;

    constructor(message: string) {
        this.message = message;
    }
}