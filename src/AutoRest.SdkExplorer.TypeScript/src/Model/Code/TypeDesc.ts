import { logError } from "../../Utils/logger";
import { SchemaEnum } from "../Schema/SchemaEnum";
import { SchemaNone } from "../Schema/SchemaNone";
import { SchemaObject } from "../Schema/SchemaObject";
import { SchemaStore } from "../Schema/SchemaStore";

export class TypeDesc implements AutoRest.SdkExplorer.Interface.TypeDesc {
    name: string;
    namespace: string;
    isValueType: boolean;
    isEnum: boolean;
    isNullable: boolean;
    isGenericType: boolean;
    isFrameworkType: boolean;
    isList: boolean;
    isDictionary: boolean;
    isBinaryData: boolean;
    arguments: TypeDesc[];
    fullNameWithNamespace: string;
    fullNameWithoutNamespace: string;
    schemaKey: string;
    schemaType: string;

    private _schemaLoaded: boolean = false;
    private _schemaObject?: SchemaObject;
    get schemaObject(): SchemaObject | undefined {
        if (!this._schemaLoaded)
            throw Error(`schemaObject not loaded yet for ${this.schemaKey}`);
        return this._schemaObject;
    }
    private _schemaEnum?: SchemaEnum;
    get schemaEnum(): SchemaEnum | undefined {
        if (!this._schemaLoaded)
            throw Error(`schemaEnum not loaded yet for ${this.schemaKey}`);
        return this._schemaEnum;
    }
    private _schemaNone?: SchemaNone;
    get schemaNone(): SchemaNone | undefined {
        if (!this._schemaLoaded)
            throw Error(`schemaNone not loaded yet for ${this.schemaKey}`);
        return this._schemaNone;
    }

    public getFullNameWithNamespace(includeGlobal: boolean, includeEndingQuestionMark: boolean): string | undefined {
        if (!this.fullNameWithNamespace)
            return undefined;

        let r = this.fullNameWithNamespace;
        if (includeGlobal)
            r = 'global::' + r;
        if (!includeEndingQuestionMark) {
            if (r[r.length - 1] === '?')
                r = r.substring(0, r.length - 1);
        }
        return r;
    }

    constructor(data: AutoRest.SdkExplorer.Interface.TypeDesc) {
        this.name = data.name!;
        this.namespace = data.namespace!;
        this.isValueType = data.isValueType!;
        this.isEnum = data.isEnum!;
        this.isNullable = data.isNullable!;
        this.isGenericType = data.isGenericType!;
        this.isFrameworkType = data.isFrameworkType!;
        this.isList = data.isList!;
        this.isDictionary = data.isDictionary!;
        this.isBinaryData = data.isBinaryData!;
        this.arguments = data.arguments ? data.arguments.map(d => new TypeDesc(d)) : [];
        this.fullNameWithNamespace = data.fullNameWithNamespace!;
        this.fullNameWithoutNamespace = data.fullNameWithoutNamespace!;
        this.schemaKey = data.schemaKey!;
        this.schemaType = data.schemaType!;
    }

    public loadSchema(schemaStore: SchemaStore): void {
        // schema already loaded.
        // TODO: support reset schema when needed
        if (this._schemaLoaded)
            return;

        if (this.schemaType === "OBJECT_SCHEMA") {
            this._schemaLoaded = true;
            this._schemaObject = schemaStore.objectSchemasMap.get(this.schemaKey!);
            this._schemaEnum = undefined;
            this._schemaNone = undefined;
            if (this._schemaObject) {
                this._schemaObject?.inheritFrom?.loadSchema(schemaStore);
                this._schemaObject?.inheritBy?.forEach(v => v.loadSchema(schemaStore));
                this._schemaObject.properties?.forEach(p => p.type?.loadSchema(schemaStore));
                this._schemaObject.initializationConstructor?.methodParameters?.forEach(p => p.type?.loadSchema(schemaStore));
                this._schemaObject.serializationConstructor?.methodParameters?.forEach(p => p.type?.loadSchema(schemaStore));
                this._schemaObject.discriminatorProperty?.type?.loadSchema(schemaStore);
            }
        }
        else if (this.schemaType === "ENUM_SCHEMA") {
            this._schemaLoaded = true;
            this._schemaObject = undefined;
            this._schemaEnum = schemaStore.enumSchemasMap.get(this.schemaKey!);
            this._schemaNone = undefined;
        }
        else if (this.schemaType === "NONE_SCHEMA") {
            this._schemaLoaded = true;
            this._schemaObject = undefined;
            this._schemaEnum = undefined;
            this._schemaNone = schemaStore.noneSchemasMap.get(this.schemaKey!);
        }
        else {
            logError("unknown SchemaType: " + this.fullNameWithNamespace);
        }

        if (this._schemaObject === undefined && this._schemaEnum === undefined && this._schemaNone === undefined) {
            logError("schema not found for type: " + this.fullNameWithNamespace);
        }

        this.arguments?.forEach(a => a.loadSchema(schemaStore));
    }

    public static get AnyType() {
        return new TypeDesc({
            fullNameWithNamespace: "Any",
            fullNameWithoutNamespace: "Any",
            isBinaryData: false,
            arguments: [],
            isDictionary: false,
            isEnum: false,
            isFrameworkType: false,
            isGenericType: false,
            isList: false,
            isNullable: true,
            isValueType: false,
            name: "Any",
            namespace: "",
            schemaKey: "Any",
            schemaType: "none",
        })
    }

    public static getDictionaryItemType(dictionaryType: TypeDesc) {
        if (dictionaryType.isDictionary !== true || !dictionaryType.arguments || dictionaryType.arguments.length !== 2) {
            throw Error("unexpected arguments for dict item. type: " + dictionaryType.fullNameWithNamespace);
        }
        let keyType = dictionaryType.arguments[0];
        let valueType = dictionaryType.arguments[1];

        let r = new TypeDesc({
            fullNameWithNamespace: `${dictionaryType.fullNameWithNamespace}.DictoinaryItem`,
            fullNameWithoutNamespace: `${dictionaryType.fullNameWithoutNamespace}.DictionaryItem`,
            isBinaryData: false,
            arguments: [],
            isDictionary: false,
            isEnum: false,
            isFrameworkType: false,
            isGenericType: true,
            isList: false,
            isNullable: false,
            isValueType: false,
            name: "dictionaryitem",
            namespace: "",
            schemaKey: `${dictionaryType.fullNameWithNamespace}.DictoinaryItem`,
            schemaType: "none",
        });
        r.arguments?.push(keyType);
        r.arguments?.push(valueType);
        return r;
    }
}
