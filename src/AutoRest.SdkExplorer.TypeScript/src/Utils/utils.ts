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

export function convertStringIndexdArray<P, T>(src: { [index: string]: P } | undefined, convert: (v: P, k: string) => T): { [index: string]: T } | undefined {
    const r: { [index: string]: T } = {};
    if (!src)
        return undefined;
    Object.keys(src).forEach(key => r[key] = convert(src[key], key));
    return r;
}

export function convertStringIndexdArrayToMap<P, T>(src: { [index: string]: P } | undefined, convert: (v: P, k: string) => T): Map<string, T> | undefined{
    if (!src)
        return undefined;
    const r = new Map<string, T>();
    Object.keys(src).forEach(key => r.set(key, convert(src[key], key)));
    return r;
}

export function convertStringIndexArrayFromMap<P, T>(src: Map<string, P> | undefined, convert: (v: P, k: string) => T): { [index: string]: T } | undefined {
    if (src === undefined)
        return undefined;
    const r: { [index: string]: T } = {};
    src.forEach((v, k) => {
        r[k] = convert(v, k);
    })
    return r;
}

export function getStringIndexArrayFromMap<T>(src: Map<string, T> | undefined) {
    if (src === undefined)
        return undefined;
    const r: { [index: string]: T } = {};
    src.forEach((v, k) => {
        r[k] = v;
    })
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
export const ONE_INDENT: string = "    ";





