import { convertStringIndexdArray, convertStringIndexdArrayToMap, getStringIndexArrayFromMap } from "../../Utils/utils";
import { SchemaEnum } from "./SchemaEnum";
import { SchemaNone } from "./SchemaNone";
import { SchemaObject } from "./SchemaObject";

export class SchemaStore implements AutoRest.SdkExplorer.Interface.SchemaStore {

    // fields below is for ser/der, please use ...Map property instead 
    get objectSchemas(): { [index: string]: SchemaObject } | undefined {
        return getStringIndexArrayFromMap(this.objectSchemasMap);
    }
    get enumSchemas(): { [index: string]: SchemaEnum } | undefined {
        return getStringIndexArrayFromMap(this.enumSchemasMap);
    }
    get noneSchemas(): { [index: string]: SchemaNone } | undefined {
        return getStringIndexArrayFromMap(this.noneSchemasMap);
    }

    objectSchemasMap: Map<string, SchemaObject>;
    enumSchemasMap: Map<string, SchemaEnum>;
    noneSchemasMap: Map<string, SchemaNone>;

    public constructor(data: AutoRest.SdkExplorer.Interface.SchemaStore) {
        this.objectSchemasMap = convertStringIndexdArrayToMap(data.objectSchemas, d => new SchemaObject(d)) ?? new Map<string, SchemaObject>();
        this.enumSchemasMap = convertStringIndexdArrayToMap(data.enumSchemas, d => new SchemaEnum(d)) ?? new Map<string, SchemaEnum>();
        this.noneSchemasMap = convertStringIndexdArrayToMap(data.noneSchemas, d => new SchemaNone(d)) ?? new Map<string, SchemaNone>();
    }

    public get schemaCount(): number {
        return this.objectSchemasMap.size + this.enumSchemasMap.size + this.noneSchemasMap.size;
    }
}
