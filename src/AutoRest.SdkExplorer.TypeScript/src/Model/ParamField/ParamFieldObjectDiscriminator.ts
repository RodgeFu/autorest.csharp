import { logError } from "../../Utils/logger";
import { containsString, isStringEqualCaseInsensitive } from "../../Utils/utils";
import { TypeDesc } from "../Code/TypeDesc";
import { ExampleValueDesc } from "../Example/ExampleValueDesc";
import { ParamFieldBase, ParamFieldExtraConstructorParameters } from "./ParamFieldBase";
import { ParamFieldEnum } from "./ParamFieldEnum";
import { ParamFieldFactory, ParamFieldType } from "./ParamFieldFactory";
import { ParamFieldObject } from "./ParamFieldObject";


export class ParamFieldObjectDiscriminator extends ParamFieldObject {

    public override setExampleValue(value: ExampleValueDesc): void {
        if (value.propertyValues === undefined)
            this.valueAsProperties = undefined;
        else if (value.propertyValuesMap === undefined || value.propertyValuesMap.size === 0)
            this.valueAsProperties = this.getDefaultValueWhenNotNull();
        else {
            let keyExampleValue = this.findDiscriminatorKeyExampleValue(value);
            if (!keyExampleValue) {
                logError("Ignore the example for discriminator object because no key found")
                return;
            }
            else {
                this.discriminatorEnum.setExampleValue(keyExampleValue);
            }
            super.setExampleValue(value);
        }
    }

    private findDiscriminatorKeyExampleValue(example: ExampleValueDesc): ExampleValueDesc | undefined {
        for (let propValueInExample of example.propertyValuesMap?.values() ?? []) {
            let foundExampleValue = this.discriminatorEnum.findMatchExample(propValueInExample);
            if (foundExampleValue) {
                return foundExampleValue;
            }
        }
        return undefined;
    }

    public override linkRelatedExamplesToObjectProperties(examples: ExampleValueDesc[]): void {
        if (this.valueAsProperties) {
            let matchExamples = examples.filter(ex => {
                let keyEx = this.findDiscriminatorKeyExampleValue(ex);
                return keyEx?.rawValue === this.curDiscriminatorKey;
            });
            this.linkRelatedExamplesToPropertyList(this.valueAsProperties, matchExamples);
        }
    }

    private _implTypePropertiesCache: Map<string, ParamFieldBase[]> = new Map<string, ParamFieldBase[]>();
    private getImplTypePropertiesFromCache() {
        let key = this.implType.schemaObject?.discriminatorKey ?? "";

        let list = this._implTypePropertiesCache.get(key);
        if (!list) {
            list = this.generatePropertyParamFieldList(this.implType);
            this._implTypePropertiesCache.set(key, list);
        }
        return list;
    }
    private setImplTypeProperties(list: ParamFieldBase[] | undefined) {
        let key = this.implType.schemaObject?.discriminatorKey ?? "";
        if (list)
            this._implTypePropertiesCache.set(key, list);
        else
            this._implTypePropertiesCache.delete(key);
    }

    public override set valueAsProperties(list: ParamFieldBase[] | undefined) {
        if (list)
            this.setImplTypeProperties(list);
        super.valueAsProperties = list;
    }
    public override get valueAsProperties(): ParamFieldBase[] | undefined {
        return super.valueAsProperties;
    }

    private _discriminatorEnum?: ParamFieldEnum;
    public get discriminatorEnum(): ParamFieldEnum {
        if (!this._discriminatorEnum) {
            let prop = this.type.schemaObject?.discriminatorProperty;
            this._discriminatorEnum = ParamFieldFactory.createParamField(prop?.name!, prop?.type!, this,
                { isReadonly: false, isRequired: true, serializerPath: prop?.serializerPath ?? prop?.name!, parameterOwner: this.parameterOwner, idPrefix: this.idPrefix }) as ParamFieldEnum;
            this._discriminatorEnum.visibleInCode = false;
            this._discriminatorEnum.description = prop?.description ?? "";
        }
        return this._discriminatorEnum;
    }

    public get curDiscriminatorKey(): string {
        return this.discriminatorEnum.valueAsString ?? "";
    }

    public override get implType(): TypeDesc {
        let key = this.curDiscriminatorKey;
        let found = this.type.schemaObject?.inheritBy?.find(v => isStringEqualCaseInsensitive(v.schemaObject?.discriminatorKey, key));
        if (!found) {
            found = this.type.schemaObject?.inheritBy.find(v => containsString(v.schemaObject?.discriminatorKey ?? "", key, true));
            if (found) {
                console.warn(`failed to find implementation with exact key '${key}' but found '${found.schemaObject?.discriminatorKey}' contains it which will be used`);
            }
            else {
                if (this.type.schemaObject && this.type.schemaObject.inheritBy.length > 0) {
                    console.warn(`failed to find implementation with or contains key '${key}'. All available discriminator is '${this.type.schemaObject.inheritBy.map(t => t.schemaObject?.discriminatorKey).join(",")}'. Try to use the first one as discriminator key`);
                    found = this.type.schemaObject.inheritBy[0];
                }
                else {
                    console.error(`failed to find implementation with or contains key '${key}' for discriminator key`);
                }
            }
        }
        return found ?? this.type;
    }

    public override getDefaultValueWhenNotNull(): any {
        let list = this.generatePropertyParamFieldList(this.implType);
        return list;
    }

    constructor(fieldName: string, fieldType: ParamFieldType, type: TypeDesc, parent: ParamFieldBase | undefined, params: ParamFieldExtraConstructorParameters) {
        super(fieldName, fieldType, type, parent, params);
        this.discriminatorEnum.onValueSet.registerHandler("discriminator-base", (sender) => {
            if (this.discriminatorEnum.isNull || this.discriminatorEnum.isIgnorable)
                return;
            let list = this.getImplTypePropertiesFromCache();
            this.valueAsProperties = list;
        });
    }
}
