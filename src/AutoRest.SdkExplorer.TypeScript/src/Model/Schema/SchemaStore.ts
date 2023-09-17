import { convertStringIndexdArray, convertStringIndexdArrayToMap } from "../../Utils/utils";
import { SchemaEnum } from "./SchemaEnum";
import { SchemaNone } from "./SchemaNone";
import { SchemaObject } from "./SchemaObject";

export class SchemaStore implements AutoRest.SdkExplorer.Interface.SchemaStore {

    // fields below is for ser/der, please use ...Map property instead 
    objectSchemas?: { [index: string]: SchemaObject };
    enumSchemas?: { [index: string]: SchemaEnum };
    noneSchemas?: { [index: string]: SchemaNone };

    objectSchemasMap: Map<string, SchemaObject>;
    enumSchemasMap: Map<string, SchemaEnum>;
    noneSchemasMap: Map<string, SchemaNone>;

    public constructor(data: AutoRest.SdkExplorer.Interface.SchemaStore) {
        this.objectSchemas = convertStringIndexdArray(data.objectSchemas, d => new SchemaObject(d));
        this.enumSchemas = convertStringIndexdArray(data.enumSchemas, d => new SchemaEnum(d));
        this.noneSchemas = convertStringIndexdArray(data.noneSchemas, d => new SchemaNone(d));

        this.objectSchemasMap = convertStringIndexdArrayToMap(data.objectSchemas, d => new SchemaObject(d));
        this.enumSchemasMap = convertStringIndexdArrayToMap(data.enumSchemas, d => new SchemaEnum(d));
        this.noneSchemasMap = convertStringIndexdArrayToMap(data.noneSchemas, d => new SchemaNone(d));

    }

    public get schemaCount(): number {
        return this.objectSchemasMap.size + this.enumSchemasMap.size + this.noneSchemasMap.size;
    }
}
