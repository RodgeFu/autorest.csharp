import { TypeDesc } from "../Code/TypeDesc";
import { SchemaBase } from "./SchemaBase";
import { SchemaEnumValue } from "./SchemaEnumValue";
import { SchemaMethod } from "./SchemaMethod";
import { SchemaProperty } from "./SchemaProperty";

export class SchemaObject extends SchemaBase implements AutoRest.SdkExplorer.Interface.SchemaObject {
    properties: SchemaProperty[];
    inheritFrom?: TypeDesc;
    inheritBy: TypeDesc[];
    initializationConstructor?: SchemaMethod;
    serializationConstructor?: SchemaMethod;
    staticCreateMethod?: SchemaMethod;
    description: string;
    isStruct: boolean;
    isEnum: boolean;
    enumValues: SchemaEnumValue[];
    discriminatorKey?: string;
    isDiscriminatorBase: boolean;
    discriminatorProperty?: SchemaProperty;

    constructor(data: AutoRest.SdkExplorer.Interface.SchemaObject) {
        super(data);
        this.properties = data.properties?.map(d => new SchemaProperty(d)) ?? []
        this.inheritFrom = data.inheritFrom ? new TypeDesc(data.inheritFrom) : undefined;
        this.inheritBy = data.inheritBy?.map(d => new TypeDesc(d)) ?? [];
        this.initializationConstructor = data.initializationConstructor ? new SchemaMethod(data.initializationConstructor) : undefined;
        this.serializationConstructor = data.serializationConstructor ? new SchemaMethod(data.serializationConstructor) : undefined;
        this.staticCreateMethod = data.staticCreateMethod ? new SchemaMethod(data.staticCreateMethod) : undefined;
        this.description = data.description ?? "";
        this.isStruct = data.isStruct!;
        this.isEnum = data.isEnum!;
        this.enumValues = data.enumValues?.map(d => new SchemaEnumValue(d)) ?? [];
        this.discriminatorKey = data.discriminatorKey;
        this.isDiscriminatorBase = data.isDiscriminatorBase ?? false;
        this.discriminatorProperty = data.discriminatorProperty ? new SchemaProperty(data.discriminatorProperty) : undefined;
    }

    public get defaultCreateMethod(): SchemaMethod | undefined {
        return this.serializationConstructor ?? this.initializationConstructor ?? this.staticCreateMethod ?? undefined;
    }
    public get isDefaultCreateMethodStatic(): boolean {
        return this.staticCreateMethod !== undefined && this.defaultCreateMethod === this.staticCreateMethod;
    }
    public get extraProperties(): SchemaProperty[] | undefined {
        let ctorParamNames = this.defaultCreateMethod?.methodParameters?.map(mp => mp.name?.toLowerCase()) ?? [];
        let r: SchemaProperty[] = this.properties ?? [];
        if (this.inheritFrom) {
            r = r.concat(this.inheritFrom.schemaObject?.properties ?? []);
        }
        return r.filter(p => !ctorParamNames.includes(p.name?.toLowerCase()));
    }
}
