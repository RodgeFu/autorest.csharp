import { isStringEqualCaseInsensitive, isStringNullOrEmpty } from "../../Utils/utils";
import { AzureResource } from "./AzureResource";
import { AzureResourceIdentifierSegment } from "./AzureResourceIdentifierSegment";
import { AzureResourceType } from "./AzureResourceType";


export class AzureResourceIdentifier implements AutoRest.SdkExplorer.Interface.AzureResourceIdentifier {
    /**
     * only for readability and troubleshooting purpose, may be empty. Use getId() to get the real id
     */
    public rawId: string;
    public resourceSegments: AzureResourceIdentifierSegment[] = [];
    public action: string = "";

    public getId(): string {
        return this.generateId(this.resourceSegments.length, true);
    }

    public get isEmpty() { return isStringNullOrEmpty(this.getId()); }
    public get hasParameter() { return this.resourceSegments.findIndex(s => s.isParameterized) >= 0; }

    public getSubscriptionId(): string | undefined {
        return this.GetResourceTypeValue("subscriptions");
    }

    public getResourceGroupName(): string | undefined {
        return this.GetResourceTypeValue("resourcegroups");
    }

    public getResourceType(): AzureResourceType | undefined {
        const providerIndex = this.GetResourceTypeIndex("providers");
        if (providerIndex >= 0) {
            const ns = this.resourceSegments[providerIndex].value;
            const value = this.resourceSegments.slice(providerIndex + 1).map(v => v.type).join("/");
            return new AzureResourceType({ namespace: ns, type: value });
        }
        return undefined;
    }

    public getLastResourceName(): string | undefined {
        return this.resourceSegments.length > 0 ? this.resourceSegments[this.resourceSegments.length - 1].value : undefined;
    }

    private generateId(takeSegmentCount: number, includeAction: boolean): string {
        const resourcePart = this.resourceSegments.slice(0, takeSegmentCount).map(s => `${s.type}/${s.value}`).join("/");
        const actionPart = (!includeAction || this.action == null || this.action.length === 0) ? "" : `${this.action}`;

        if (resourcePart.length === 0)
            return "";
        if (actionPart.length === 0)
            return `/${resourcePart}`;
        else
            return `/${resourcePart}/${actionPart}`;
    }

    public getParentIdentifierAt(lastSegmentIndex: number): AzureResourceIdentifier | undefined {
        const id = this.generateId(lastSegmentIndex + 1, false);
        return AzureResourceIdentifier.Parse(id);
    }

    constructor(data: AutoRest.SdkExplorer.Interface.AzureResourceIdentifier) {
        this.action = data.action;
        this.rawId = data.rawId;
        this.resourceSegments = data.resourceSegments.map(s => new AzureResourceIdentifierSegment(s));
    }

    private GetResourceTypeValue(resourceTypeName: string): string | undefined {
        return this.resourceSegments.find(s => isStringEqualCaseInsensitive(s.type, resourceTypeName))?.value;
    }

    private GetResourceTypeIndex(resourceName: string): number {
        return this.resourceSegments.findIndex(s => isStringEqualCaseInsensitive(s.type, resourceName));
    }

    public static IsPatternMatch(idA: string | undefined, idB: string | undefined): boolean {
        if (isStringNullOrEmpty(idA) && isStringNullOrEmpty(idB))
            return true;
        if (isStringNullOrEmpty(idA) || isStringNullOrEmpty(idB))
            return false;
        const a = AzureResourceIdentifier.Parse(idA!);
        const b = AzureResourceIdentifier.Parse(idB!);

        if (a.resourceSegments.length !== b.resourceSegments.length)
            return false;
        if (!AzureResourceType.isEquals(a.getResourceType(), b.getResourceType()))
            return false;
        if (!isStringEqualCaseInsensitive(a.action, b.action))
            return false;
        return true;
    }

    // from id like  /subscriptions/{subscription-id}/resourceGroups/myResourceGroup/providers/Microsoft.Network/networkInterfaces/{existing-nic-name}
    public static Parse(idStrng: string): AzureResourceIdentifier {

        const arr = idStrng.split(/[/\\]/).map(v => v.trim()).filter(v => v.length > 0);

        let i = 1;
        var segs = [];
        var action = "";
        while (i < arr.length) {
            segs.push(new AzureResourceIdentifierSegment({ type: arr[i - 1], value: arr[i] }));
            i += 2;
        }
        if (i == arr.length)
            action = arr[i - 1];
        const r = new AzureResourceIdentifier({ rawId: idStrng, resourceSegments: segs, action: action });
        return r;
    }

    /**
     * 
     * @param idString - an resource id with parameterized resource name/value wrapped with {}, i.e. request path like: 
     *              "/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Network/privateDnsZones/{privateZoneName}"
     * @param resource - replace parameters with the value from the resource if possible
     * @returns
     */
    public static ParseWithResource(idString: string, resource: AzureResource): AzureResourceIdentifier {

        const pAri = AzureResourceIdentifier.Parse(idString)
        const resAri = AzureResourceIdentifier.Parse(resource.id);

        pAri.resourceSegments.forEach(seg => {
            if (seg.isParameterized) {
                const found = resAri.GetResourceTypeValue(seg.type);
                if (found)
                    seg.value = found;
            }
        });

        return pAri;
    }
}

