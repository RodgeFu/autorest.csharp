// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using AutoRest.CSharp.Input;
using AutoRest.CSharp.Mgmt.AutoRest;
using AutoRest.CSharp.Mgmt.Decorator;
using AutoRest.CSharp.Mgmt.Models;
using AutoRest.CSharp.Mgmt.Output;
using AutoRest.CSharp.MgmtExplorer.Autorest;
using AutoRest.CSharp.Output.Models;
using AutoRest.CSharp.Utilities;
using SECodeGen.CSharp.Model.Azure;
using static SECodeGen.CSharp.Model.Code.ApiDesc;

namespace AutoRest.CSharp.MgmtExplorer.Models
{
    internal class MgmtExplorerApiDesc
    {
        public MgmtTypeProvider Provider { get; }
        public MgmtClientOperation Operation { get; }
        public ExampleGroup? ExampleGroup { get; }
        private int _mgmtRestOperationIndex { get; set; }
        public MgmtRestOperation RestOperation => this.Operation[_mgmtRestOperationIndex];

        public string Description => $"Method {Operation.Name}() on {Provider.Type.Name}";

        public MgmtExplorerCodeGenInfo Info { get; set; }

        public string FullUniqueName => $"{ServiceName}_{ResourceName}_{SdkOperationId}_{this.Info.SdkPackageVersion}";

        public string ServiceName
        {
            get
            {
                string name = MgmtContext.Context.DefaultLibraryName;
                const string POST_MANAGEMENTCLIENT = "ManagementClient";
                if (name.EndsWith(POST_MANAGEMENTCLIENT))
                    name = name.Substring(0, name.Length - POST_MANAGEMENTCLIENT.Length);
                return name;
            }
        }
        public string ResourceName => this.Provider.Type.Name;
        public string OperationName => this.Operation.Name;
        public string SwaggerOperationId => this.RestOperation.OperationId;
        public string SdkOperationId => $"{this.OperationName}_For_OperationId_{this.SwaggerOperationId}";
        // seems no need to include namespace to make it more readable
        public string OperationNameWithParameters => this.GetOperationNameWithParameters(false /*includeNamespace*/);
        public string OperationNameWithScopeAndParameters => this.GetOperationNameWithScopeAndParameters(false /*includeNamespace*/);
        public AzureResourceType? OperationProviderAzureResourceType { get; set; }
        public string OperationProviderType { get; set; }

        public string RequestPath { get; set; }
        public string ApiVersion { get; set; }

        // RequestParameter (type) contains more metadata we need to reference objects
        private List<RequestParameter> AllRequestParameters { get; set; }
        // contains all the parameters for lookup purpose including propertyBagParmater
        private List<MgmtExplorerParameter> AllParameters { get; set; }
        public List<MgmtExplorerParameter> MethodParameters { get; set; }
        public MgmtExplorerParameter? PropertyBagParameter { get; set; }

        public MgmtExplorerApiDesc(MgmtExplorerCodeGenInfo info, MgmtTypeProvider provider, MgmtClientOperation operation, ExampleGroup? exampleGroup, int mgmtRestOperationIndex)
        {
            Info = info;
            Provider = provider;
            Operation = operation;
            ExampleGroup = exampleGroup;
            this._mgmtRestOperationIndex = mgmtRestOperationIndex;

            var restOp = this.RestOperation;

            this.RequestPath = restOp.RequestPath.ToString();
            System.Diagnostics.Debug.Assert(restOp.Operation.ApiVersions.Count == 1);
            this.ApiVersion = restOp.Operation.ApiVersions.Last().Version;

            switch (provider)
            {
                case ResourceCollection rc:
                    this.OperationProviderType = ProviderType.ResourceCollection;
                    this.OperationProviderAzureResourceType = new AzureResourceType(
                        rc.ResourceType.Namespace.ConstantValue,
                        string.Join("/", rc.ResourceType.Types.Select(t => t.ConstantValue)));
                    break;
                case Resource res:
                    this.OperationProviderType = ProviderType.Resource;
                    this.OperationProviderAzureResourceType = new AzureResourceType(
                        res.ResourceType.Namespace.ConstantValue,
                        string.Join("/", res.ResourceType.Types.Select(t => t.ConstantValue)));
                    break;
                case MgmtExtension ex:
                    this.OperationProviderType = ProviderType.Extension;
                    this.OperationProviderAzureResourceType = null;
                    break;
                default:
                    throw new InvalidOperationException("Unexpected apiDesc.Provider type: " + provider.GetType().ToString());
            }

            this.AllRequestParameters = InitAllRequestParameters();
            this.AllParameters = InitAllParameters();
            this.MethodParameters = InitMethodParameters();
        }

        public List<MgmtExplorerParameter> GetHostParameters(RequestPath hostRequestPath)
        {
            RequestPath opRequestPath = this.RestOperation.RequestPath;
            RequestPath opRequestPathForHost = Mgmt.Models.RequestPath.FromSegments(opRequestPath.Take(hostRequestPath.Count));

            // for troubleshooting
            if (hostRequestPath.Where(s => s.IsReference).Count() != opRequestPathForHost.Where(s => s.IsReference).Count())
            {
                throw new InvalidOperationException($"hostRequestPath/opRequestPath reference count mismatch: " +
                    $"hostRequestPath: {string.Join(',', hostRequestPath.Where(s => s.IsReference).Select(s => s.ReferenceName))}" +
                    $" <> opRequestPath: {string.Join(',', opRequestPath.Where(s => s.IsReference).Select(s => s.ReferenceName))}");
            }

            var r = new List<MgmtExplorerParameter>();
            string hintPath = "";
            foreach (var s in opRequestPathForHost)
            {
                if (s.IsReference)
                {
                    hintPath += $"/{{{s.ReferenceName}}}";

                    var p = this.AllParameters.FirstOrDefault(pp => pp.CSharpName == s.ReferenceName);
                    if (p == null)
                        throw new InvalidOperationException("Can't find host parameter in all parameter list: " + s.ReferenceName);
                    p.Source = MgmtExplorerParameter.SOURCE_REQUEST_PATH;
                    p.SourceArg = hintPath;
                    r.Add(p);
                }
                else
                {
                    hintPath += $"/{s.ConstantValue}";
                }
            }
            return r;
        }

        private List<RequestParameter> InitAllRequestParameters()
        {
            // TODO: we only consider the first operation for now. When will there be more operations?
            var op = this.RestOperation;
            var sr = op.Operation.GetServiceRequest();
            var allRequestParameters = sr == null ?
                op.Operation.Parameters : op.Operation.Parameters.Concat(sr.Parameters);

            return allRequestParameters.ToList();
        }

        private List<MgmtExplorerParameter> InitAllParameters()
        {
            string[] CLIENT_PARAMETER_NAMES = {
                "cancellationToken",
                "waitUntil" };
            var requestPath = this.RestOperation.RequestPath.SerializedPath;
            var allParameters = this.Operation.Parameters.Select(p =>
                this.Operation.PropertyBagUnderlyingParameters.FirstOrDefault(pp => pp.Name == p.Name) ?? p).ToList();

            var methodParameters = this.Operation.MethodSignature.Modifiers.HasFlag(MethodSignatureModifiers.Extension) ?
                this.Operation.MethodParameters.Skip(1) : this.Operation.MethodParameters;
            foreach (var p in methodParameters)
            {
                if (p.IsPropertyBag)
                {
                    this.PropertyBagParameter = new MgmtExplorerParameter(p, p.Name, p.Name, requestPath);
                }
                else
                {
                    if (allParameters.FirstOrDefault(pp => pp.Name == p.Name) == null)
                    {
                        allParameters.Add(p);
                    }
                }
            }

            List<MgmtExplorerParameter> r = new List<MgmtExplorerParameter>();
            if (this.PropertyBagParameter != null)
                r.Add(this.PropertyBagParameter);
            foreach (var mp in allParameters)
            {
                if (CLIENT_PARAMETER_NAMES.Contains(mp.Name))
                {
                    // use the serializerName = Name for client parameters
                    r.Add(new MgmtExplorerParameter(mp, mp.Name, mp.Name, requestPath));
                }
                else
                {
                    var rp = this.AllRequestParameters.Find(p => p.Language.GetName().ToVariableName() == mp.Name);
                    if (rp == null)
                        throw new InvalidOperationException("Can't find Input Parameter for Method Parameter: " + mp.Name);

                    r.Add(new MgmtExplorerParameter(mp, rp, requestPath));
                }
            }
            return r;
        }

        private List<MgmtExplorerParameter> InitMethodParameters()
        {
            var methodParameters = this.Operation.MethodSignature.Modifiers.HasFlag(MethodSignatureModifiers.Extension) ?
                this.Operation.MethodParameters.Skip(1) : this.Operation.MethodParameters;

            var r = new List<MgmtExplorerParameter>();
            foreach (var mp in methodParameters)
            {
                if (mp.IsPropertyBag)
                    continue;
                var p = this.AllParameters.FirstOrDefault(pp => pp.CSharpName == mp.Name);
                if (p == null)
                    throw new InvalidOperationException("Can't find method parameter in all parameter list: " + mp.Name);
                r.Add(p);
            }
            foreach (var mp in this.Operation.PropertyBagUnderlyingParameters)
            {
                var p = this.AllParameters.FirstOrDefault(pp => pp.CSharpName == mp.Name);
                if (p == null)
                    throw new InvalidOperationException("Can't find method parameter in all parameter list: " + mp.Name);
                r.Add(p);
            }
            return r;
        }

        public string GetOperationNameWithParameters(bool includeNamespace)
        {
            return this.OperationName + "(" + string.Join(", ", this.MethodParameters.Select(p => $"{p.Type.GetFullName(includeNamespace)} {p.CSharpName}")) + ")";
        }

        public string GetOperationNameWithScopeAndParameters(bool includeNamespace)
        {
            return $"{this.ServiceName}.{this.ResourceName}.{this.GetOperationNameWithParameters(includeNamespace)}";
        }
    }

}
