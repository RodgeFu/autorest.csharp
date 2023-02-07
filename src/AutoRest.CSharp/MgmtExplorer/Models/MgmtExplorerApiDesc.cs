﻿// Copyright (c) Microsoft Corporation. All rights reserved.
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
using AutoRest.CSharp.MgmtExplorer.Contract;
using AutoRest.CSharp.Output.Models;
using AutoRest.CSharp.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoRest.CSharp.MgmtExplorer.Models
{
    internal class MgmtExplorerApiDesc
    {
        public MgmtTypeProvider Provider { get; }
        public MgmtClientOperation Operation { get; }
        public ExampleGroup? ExampleGroup { get; }

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
        public string SwaggerOperationId => this.Operation.First().OperationId;
        public string SdkOperationId => $"{this.OperationName}_For_OperationId_{this.SwaggerOperationId}";
        // seems no need to include namespace to make it more readable
        public string OperationNameWithParameters => this.GetOperationNameWithParameters(false /*includeNamespace*/);
        public string OperationNameWithScopeAndParameters => this.GetOperationNameWithScopeAndParameters(false /*includeNamespace*/);
        public MgmtExplorerProviderAzureResourceType? OperationProviderAzureResourceType { get; set; }
        public MgmtExplorerProviderType OperationProviderType { get; set; }

        public string RequestPath { get; set; }
        public string ApiVersion { get; set; }

        // RequestParameter (type) contains more metadata we need to reference objects
        private List<RequestParameter> AllRequestParameters { get; set; }
        // contains all the parameters for lookup purpose
        private List<MgmtExplorerParameter> AllParameters { get; set; }
        public List<MgmtExplorerParameter> MethodParameters { get; set; }

        public MgmtExplorerApiDesc(MgmtExplorerCodeGenInfo info, MgmtTypeProvider provider, MgmtClientOperation operation, ExampleGroup? exampleGroup)
        {
            Info = info;
            Provider = provider;
            Operation = operation;
            ExampleGroup = exampleGroup;

            System.Diagnostics.Debug.Assert(operation.Count == 1);
            var firstOp = operation.First();

            this.RequestPath = firstOp.RequestPath.ToString();
            System.Diagnostics.Debug.Assert(firstOp.Operation.ApiVersions.Count == 1);
            this.ApiVersion = firstOp.Operation.ApiVersions.Last().Version;

            switch (provider)
            {
                case ResourceCollection rc:
                    this.OperationProviderType = MgmtExplorerProviderType.ResourceCollection;
                    this.OperationProviderAzureResourceType = new MgmtExplorerProviderAzureResourceType(
                        rc.ResourceType.Namespace.ConstantValue,
                        string.Join("/", rc.ResourceType.Types.Select(t => t.ConstantValue)));
                    break;
                case Resource res:
                    this.OperationProviderType = MgmtExplorerProviderType.Resource;
                    this.OperationProviderAzureResourceType = new MgmtExplorerProviderAzureResourceType(
                        res.ResourceType.Namespace.ConstantValue,
                        string.Join("/", res.ResourceType.Types.Select(t => t.ConstantValue)));
                    break;
                case MgmtExtensions ex:
                    this.OperationProviderType = MgmtExplorerProviderType.Extension;
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
            RequestPath opRequestPath = this.Operation.FirstOrDefault().RequestPath;
            RequestPath opRequestPathForHost = new RequestPath(opRequestPath.Take(hostRequestPath.Count));

            // for troubleshooting
            if (hostRequestPath.Where(s => s.IsReference).Count() != opRequestPathForHost.Where(s => s.IsReference).Count())
            {
                throw new InvalidOperationException($"hostRequestPath/opRequestPath reference count mismatch: " +
                    $"hostRequestPath: {string.Join(',', hostRequestPath.Where(s => s.IsReference).Select(s => s.ReferenceName))}" +
                    $"opRequestPath: {string.Join(',', opRequestPath.Where(s => s.IsReference).Select(s => s.ReferenceName))}");
            }

            var r = new List<MgmtExplorerParameter>();
            string hintPath = "";
            foreach (var s in opRequestPathForHost)
            {
                if (s.IsReference)
                {
                    hintPath += $"/{{{s.ReferenceName}}}";

                    var p = this.AllParameters.FirstOrDefault(pp => pp.CSharpName == s.ReferenceName);
                    p.Source = "RequestPath";
                    p.SourceArg = hintPath;
                    if (p == null)
                        throw new InvalidOperationException("Can't find host parameter in all parameter list: " + s.ReferenceName);
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
            var op = this.Operation.FirstOrDefault();
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
            var allParameters = this.Operation.Parameters.ToList();
            var methodParameters = this.Operation.MethodSignature.Modifiers.HasFlag(MethodSignatureModifiers.Extension) ?
                this.Operation.MethodParameters.Skip(1) : this.Operation.MethodParameters;
            foreach (var p in methodParameters)
            {
                if (allParameters.FirstOrDefault(pp => pp.Name == p.Name) == null)
                {
                    allParameters.Add(p);
                }
            }

            var requestPath = this.Operation.FirstOrDefault().RequestPath.SerializedPath;
            List<MgmtExplorerParameter> r = new List<MgmtExplorerParameter>();
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
