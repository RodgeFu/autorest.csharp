import moment from "moment";

export type MessageLevel = "none" | "success" | "info" | "warning" | "error";

export class MessageItem {

    public timestamp: moment.Moment = moment.utc();
    public id: string;

    constructor(public message: string = "", public level: MessageLevel = "none", public inToast: boolean = false) {
        this.id = crypto.randomUUID();
    }

    public toString() : string{
        return this.message;
    }

    public get textClass(): string {
        if (this.level === "none")
            return '';
        else if (this.level === "success")
            return 'success-text';
        else if (this.level === "info")
            return 'info-text';
        else if (this.level === "warning")
            return 'warning-text';
        else if (this.level === "error")
            return 'error-text';
        else
            return '';
    }

    public get title(): string {
        if (this.level === "none")
            return 'Message';
        else if (this.level === "success")
            return 'Success Message';
        else if (this.level === "info")
            return 'Info Message';
        else if (this.level === "warning")
            return 'Warning Message';
        else if (this.level === "error")
            return 'Error Message';
        else
            return 'Message';
    }
}
