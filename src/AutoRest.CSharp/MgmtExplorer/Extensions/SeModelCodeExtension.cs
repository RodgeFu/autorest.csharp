// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.MgmtExplorer.Models;
using AutoRest.CSharp.Output.Models.Types;
using AutoRest.SdkExplorer.Model.Code;

namespace AutoRest.CSharp.MgmtExplorer.Extensions
{
    internal static class SeModelCodeExtension
    {
        internal static ApiDesc CreateOperationDesc(this MgmtExplorerApiDesc apiDesc)
        {
            ApiDesc r = new ApiDesc()
            {
                Language = "DotNet",
                SdkPackageName = apiDesc.Info.SdkPackageName,
                SdkPackageVersion = apiDesc.Info.SdkPackageVersion,
                Dependencies = apiDesc.Info.Dependencies,
                ExplorerCodeGenVersion = apiDesc.Info.ExplorerCodeGenVersion,
                GeneratedTimestamp = apiDesc.Info.GeneratedTimestamp,
                ServiceName = apiDesc.ServiceName,
                ResourceName = apiDesc.ResourceName,
                OperationName = apiDesc.OperationName,
                SwaggerOperationId = apiDesc.SwaggerOperationId,
                SdkOperationId = apiDesc.SdkOperationId,
                FullUniqueName = apiDesc.FullUniqueName,
                OperationNameWithScopeAndParameters = apiDesc.OperationNameWithScopeAndParameters,
                OperationNameWithParameters = apiDesc.OperationNameWithParameters,
                OperationMethodParameters = apiDesc.MethodParameters.Select(p => p.ToCodeSegmentParameter()).ToList(),
                PropertyBagParameter = apiDesc.PropertyBagParameter?.ToCodeSegmentParameter(),
                OperationProviderAzureResourceType = apiDesc.OperationProviderAzureResourceType,
                OperationProviderType = apiDesc.OperationProviderType,
                RequestPath = apiDesc.RequestPath,
                ApiVersion = apiDesc.ApiVersion,
                EncodedFunctionName = apiDesc.EncodedFunctionName,
            };
            return r;
        }

        internal static TypeDesc CreateSeTypeDesc(this Type type)
        {
            return new CSharpType(type).CreateSeTypeDesc();
        }

        internal static TypeDesc CreateSeTypeDesc(this CSharpType csharpType)
        {
            TypeDesc r = new TypeDesc();
            if (csharpType.IsFrameworkType && csharpType.FrameworkType == typeof(Nullable<>))
            {
                // use the real type and mark it as nullable
                csharpType = csharpType.Arguments[0];
                r.IsNullable = true;
            }
            else if (!csharpType.IsFrameworkType && csharpType.Implementation is SystemObjectType && ((SystemObjectType)csharpType.Implementation).SystemType.IsClass)
            {
                r.IsNullable = true;
            }
            else
            {
                r.IsNullable = csharpType.IsNullable;
            }

            r.Name = csharpType.Name;
            r.Namespace = csharpType.Namespace;
            r.IsValueType = csharpType.IsValueType;
            r.IsEnum = csharpType.IsEnum;
            r.IsGenericType = csharpType.IsGenericType;
            r.IsFrameworkType = csharpType.IsFrameworkType;
            r.Arguments = new List<TypeDesc>();
            if (r.IsGenericType)
            {
                foreach (var t in csharpType.Arguments)
                {
                    r.Arguments.Add(t.CreateSeTypeDesc());
                }
            }
            r.FullNameWithNamespace = r.GetFullName(true);
            r.FullNameWithoutNamespace = r.GetFullName(false);

            if (csharpType.IsFrameworkType)
            {
                if (TypeFactory.IsList(csharpType) || csharpType.FrameworkType == typeof(List<>))
                    r.IsList = true;

                if (TypeFactory.IsDictionary(csharpType))
                    r.IsDictionary = true;

                if (csharpType.FrameworkType == typeof(BinaryData))
                    r.IsBinaryData = true;
            }

            var schema = csharpType.GetOrCreateSeSchema();
            r.SetSchema(schema);
            return r;
        }
    }
}
