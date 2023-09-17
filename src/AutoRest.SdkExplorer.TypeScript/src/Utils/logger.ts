import moment from "moment";
import { DATE_FORMAT } from "./utils";
import { MessageItem } from "./messageItem";

export function logInfo(msg: string | MessageItem){
    console.info(formatLog(msg));
}

export function logWarning(msg: string | MessageItem) {
    console.warn(formatLog(msg));
}

export function logError(msg: string | MessageItem) {
    console.error(formatLog(msg));
}

export function logTrace(msg: string | MessageItem) {
    console.trace(formatLog(msg));
}

export function logDebug(msg: string | MessageItem) {
    console.debug(formatLog(msg));
}

function formatLog(msg: string | MessageItem): string {
    return `[${moment().format(DATE_FORMAT)}]: ${msg.toString()}`;
}

