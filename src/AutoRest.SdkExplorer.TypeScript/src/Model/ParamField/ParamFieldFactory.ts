import { logError } from "../../Utils/logger";
import { TypeDesc } from "../Code/TypeDesc";
import { ParamFieldAny } from "./ParamFieldAny";
import { ParamFieldArray } from "./ParamFieldArray";
import { ParamFieldBase, ParamFieldExtraConstructorParameters } from "./ParamFieldBase";
import { ParamFieldBinary } from "./ParamFieldBinary";
import { ParamFieldBool } from "./ParamFieldBool";
import { ParamFieldDate } from "./ParamFieldDate";
import { ParamFieldDictionary } from "./ParamFieldDictionary";
import { ParamFieldDictionaryItem } from "./ParamFieldDictionaryItem";
import { ParamFieldEnum } from "./ParamFieldEnum";
import { ParamFieldNumber } from "./ParamFieldFloat";
import { ParamFieldInteger } from "./ParamFieldInteger";
import { ParamFieldObject } from "./ParamFieldObject";
import { ParamFieldObjectDiscriminator } from "./ParamFieldObjectDiscriminator";
import { ParamFieldObjectEnum } from "./ParamFieldObjectEnum";
import { ParamFieldObjectOneProperty } from "./ParamFieldObjectOneProperty";
import { ParamFieldString } from "./ParamFieldString";

export type ParamFieldType = 'cancellationtoken' | 'waituntil' | 'onestringproperty' | 'string' | 'integer' | 'number' |
    'bool' | 'binary' | 'date' | 'any' | 'array' | 'dictionary' | 'dictionaryitem' | 'objectenum' | 'objectdiscriminator' | 'object' | 'enum' | 'unknown';

export class ParamFieldFactory {

    public static createParamField(fieldName: string, type: TypeDesc, parent: ParamFieldBase | undefined,
        params: ParamFieldExtraConstructorParameters): ParamFieldBase {

        if (type.name) {
            let typeName = type.name.toLowerCase();

            // handle cancellationtoken and waituntil specially which won't show in portal anyway
            if (["cancellationtoken"].includes(typeName)) {
                let r = new ParamFieldAny(fieldName, "cancellationtoken", type, parent, params);
                r.value = "default";
                return r;
            }
            else if (["waituntil"].includes(typeName)) {
                let r = new ParamFieldAny(fieldName, "waituntil", type, parent, params);
                r.value = "global::Azure.WaitUntil.Completed";
                return r;
            }
            else if (typeName === "guid" || typeName === "resourceidentifier" || typeName === "uri" || typeName == "ipaddress") {
                return new ParamFieldObjectOneProperty(fieldName, "onestringproperty", type, parent, params);
            }
            else if (typeName === "string") {
                return new ParamFieldString(fieldName, "string", type, parent, params);
            }
            else if (["int16", "int32", "int64", "integer"].includes(typeName)) {
                return new ParamFieldInteger(fieldName, "integer", type, parent, params);
            }
            else if (['float', 'double', 'number', 'single'].includes(typeName)) {
                return new ParamFieldNumber(fieldName, "number", type, parent, params);
            }
            else if (['boolean', 'bool'].includes(typeName)) {
                return new ParamFieldBool(fieldName, "bool", type, parent, params);
            }
            else if (["binary", "binarydata", "bytearray"].includes(typeName) || type.isBinaryData) {
                return new ParamFieldBinary(fieldName, "binary", type, parent, params);
            }
            else if (["date", "datetime", "datetimeoffset"].includes(typeName)) {
                return new ParamFieldDate(fieldName, "date", type, parent, params);
            }
            else if (['any', 'anyobject'].includes(typeName)) {
                return new ParamFieldAny(fieldName, "any", type, parent, params);
            }
            else if (['array'].includes(typeName) || type.isList) {
                return new ParamFieldArray(fieldName, "array", type, parent, params);
            }
            else if (["dictionary"].includes(typeName) || type.isDictionary) {
                return new ParamFieldDictionary(fieldName, "dictionary", type, parent, params);
            }
            else if (["dictionaryitem"].includes(typeName)) {
                return new ParamFieldDictionaryItem(fieldName, "dictionaryitem", type, parent, params);
            }
            // TODO: use object to handle struct too for simplity, add struct support when needed
            else if (["object"].includes(typeName) || type.isValueType === false || (type.isFrameworkType && type.isEnum == false)) {
                if (type.schemaObject?.isEnum === true) {
                    return new ParamFieldObjectEnum(fieldName, "objectenum", type, parent, params);
                }
                else if (type.schemaObject?.isDiscriminatorBase) {
                    return new ParamFieldObjectDiscriminator(fieldName, "objectdiscriminator", type, parent, params);
                }
                else {
                    return new ParamFieldObject(fieldName, "object", type, parent, params);
                }
            }
            else if (["choice"].includes(typeName) || type.isEnum) {
                return new ParamFieldEnum(fieldName, "enum", type, parent, params);
            }
        }
        // do we need to worry about struct?
        // constant, conditional, time, unixtime, shall we handle them? wait for real world example to decide
        logError("unknown type: " + JSON.stringify(type));
        // use any to avoid breaking portal
        return new ParamFieldAny(fieldName, "unknown", type, parent, params);
    }
}
