import moment from "moment";
import _, { isEqual, isObject } from "underscore";

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
    return str === undefined || str === null || str.length === 0;
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

export function tryJsonToObj(str: string | undefined) {
    str = str?.trim();
    if (!str || str.length === 0)
        return undefined;

    // trim starting and ending whitespace and "
    // make sure the string starts and ends with {}
    let m = str.match(/^\s*"?\s*({.*})\s*"?\s*$/is);
    if (!m || m.length < 2)
        return undefined;
    str = m[1];

    let r = jsonStringToObj(str);
    if (r)
        return r;
    if (str.indexOf("\\\"") >= 0) {
        r = jsonEscapedStringToObj(str);
        if (r)
            return r;
    }
    return undefined;
}

export function jsonEscapedStringToObj(jsonEscapedString: string) {
    let ori = jsonEscapedString;
    // just handle 3 cases for now, keep adding when needed
    jsonEscapedString = jsonEscapedString.replace(/\\n/g, "\\\\n").replace(/\\r/g, "\\\\r").replace(/\\t/g, "\\\\t");
    let jsonString = JSON.parse(`"${jsonEscapedString}"`);
    return jsonStringToObj(jsonString);
}

export function jsonStringToObj(jsonString: string) : any {
    // handle extra comma
    let ori = jsonString;
    jsonString = jsonString.replaceAll(/,\s*?([}\]])/gis, (sub, args) => {
        return sub.substring(1);
    });
    try {
        return JSON.parse(jsonString);
    }
    catch (error) {
        console.warn("fail to parse json: \n" + ori);
        return undefined;
    }
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


export function anyToString(item: any, minify: boolean = false) {
    if (item === undefined)
        return "<undefined>";
    else if (item === null)
        return "<null>";
    else if (isObject(item))
        return JSON.stringify(item, undefined, minify ? undefined : "  ");
    else
        return item.toString();
}

export function diffObjects(baseObject: any, newObject: any, path: string, log: string[]) {

    if (!isObject(baseObject) || !isObject(newObject)) {
        if (!isEqual(baseObject, newObject)) {
            log.push(`${path}[*] => '${anyToString(newObject, true)}'`);
        }
        return;
    }
    else {
        const newKeys = Object.keys(newObject);
        const oldKeys = Object.keys(baseObject);

        for (let key of newKeys) {
            const newPath = `${path}.${key}`;
            const index = oldKeys.indexOf(key);
            if (index >= 0) {
                oldKeys.splice(index, 1);
                diffObjects(baseObject[key], newObject[key], newPath, log);
            }
            else {
                log.push(`${newPath}[+] => '${anyToString(newObject[key], true)}'`);
            }
        }
        oldKeys.forEach(k => log.push(`${path}.${k}[-]`));
    }
}

export function distinctArray(arr: any[]) {
    return arr.filter((value, index, array) => array.indexOf(value) === index);
}

export const DATE_FORMAT: string = "YYYY-MM-DDTHH:mm:ss.SSS";
export const TIME_FORMAT: string = "HH:mm:ss.SSS";
export const ONE_INDENT: string = "    ";





