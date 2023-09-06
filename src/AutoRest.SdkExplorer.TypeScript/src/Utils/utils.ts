import moment from "moment";
import _ from "underscore";

/**
 * 
 * @param a
 * @param b
 * @param caseSensitive - default is false
 * @returns
 */
export function isStringEqualCaseInsensitive(a: string | undefined, b: string | undefined): boolean {

    if (a === undefined && b === undefined)
        return true;
    if (a === undefined || b === undefined)
        return false;
    if (a.length !== b.length)
        return false;

    return a.toUpperCase() === b.toUpperCase();
}

export function isStringNullOrEmpty(str: string | undefined) {
    return str === undefined || str.length === 0;
}

export function containsString(str: string, strToFind: string, ignoreCase: boolean = true) {
    return ignoreCase ? str.toLowerCase().indexOf(strToFind.toLowerCase()) > -1 : str.indexOf(strToFind) > -1;
}

export const strcmp = (a: string | undefined, b: string | undefined) => (a ?? "").toLowerCase() > (b ?? "").toLowerCase() ? 1 : -1;

export function escapeRegExp(string: string) {
    return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
}

export function escapeHtml(string: string) {
    return _.escape(string);
}

export function copyMap<T, P>(src: Map<T, P>): Map<T, P> {
    let r = new Map<T, P>();
    src.forEach((v, k) => r.set(k, v));
    return r;
}

export function htmlDecode(input: string): string {
    var doc = new DOMParser().parseFromString(input, "text/html");
    return doc.documentElement.textContent ?? input;
}

export function toReadableTime(date: Date): string {
    return moment.utc(date).fromNow();
}
export const DATE_FORMAT: string = "YYYY-MM-DDTHH:mm:ss.SSS";
export const TIME_FORMAT: string = "HH:mm:ss.SSS";




