// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoRest.CSharp.Input;
using AutoRest.CSharp.MgmtExplorer.Autorest;
using AutoRest.SdkExplorer.Model.Code;
using AutoRest.SdkExplorer.Model.Example;
using AutoRest.SdkExplorer.Model.Schema;

namespace AutoRest.CSharp.MgmtExplorer.Extensions
{
    internal static class SeModelExampleExtension
    {
        internal static ExampleDesc CreateExampleDesc(this ExampleModel em, ApiDesc operationDesc)
        {
            ExampleDesc r = new ExampleDesc()
            {
                SdkPackageName = operationDesc.SdkPackageName,
                SdkPackageVersion = operationDesc.SdkPackageVersion,
                ExplorerCodeGenVersion = operationDesc.ExplorerCodeGenVersion,
                GeneratedTimestamp = operationDesc.GeneratedTimestamp,
                Language = operationDesc.Language,
                ServiceName = operationDesc.ServiceName,
                ResourceName = operationDesc.ResourceName,
                OperationName = operationDesc.OperationName,
                SwaggerOperationId = operationDesc.SwaggerOperationId,
                SdkOperationId = operationDesc.SdkOperationId,
                SdkFullUniqueName = operationDesc.FullUniqueName,
                ExampleName = em.Name,
                OriginalFilePath = em.OriginalFile,
                OriginalFileNameWithoutExtension = Path.GetFileNameWithoutExtension(em.OriginalFile),
            };

            var allMethodParameters = operationDesc.CodeSegments.SelectMany(cs => cs.Parameters);
            string[] IGNORED_PARAM_LIST =
            {
                "api-version",
            };
            foreach (var exampleParam in em.AllParameters)
            {
                var sName = exampleParam.Parameter.Language.GetSerializerNameOrName();
                if (IGNORED_PARAM_LIST.Contains(sName))
                    continue;

                var methodParameter = allMethodParameters.FirstOrDefault(p => p.SerializerName == sName);
                if (methodParameter != null)
                {
                    r.ExampleValues[sName] = exampleParam.ExampleValue.CreateSeExampleValueDesc();
                }
                else if (exampleParam.Parameter.Schema.Type == AllSchemaTypes.Constant)
                {
                    // ignore it if the example is for a constant parameter
                    continue;
                }
                else
                {
                    throw new InvalidOperationException("unable to find parameter for example, name = " + sName);
                }
            }
            return r;
        }

        internal static ExampleValueDesc CreateSeExampleValueDesc(this ExampleValue ev)
        {
            ExampleValueDesc r = new ExampleValueDesc();
            r.SchemaType = ev.Schema.Type.ToString();
            if (ev.Language != null)
            {
                r.ModelName = ev.Language.GetName();
                r.SerializerName = ev.Language.GetSerializerNameOrName();
                r.CSharpName = ev.CSharpName();
            }

            if (r.SchemaType == AllSchemaTypes.AnyObject.ToString() || r.SchemaType == AllSchemaTypes.Any.ToString())
            {
                // we know nothing about it. just ignore these parameters for now because we dont know how to re-create these value later anyway
                // TODO: add some handling if we found some "Any" can be guessed...
                r.RawValue = null;
            }
            else
            {
                r.RawValue = ev.RawValue?.ToString();
            }
            r.PropertyValues = ev.Properties?.ToDictionary(
                v => v.Key,
                v => v.Value.CreateSeExampleValueDesc());
            r.ArrayValues = ev.Elements?.Select(v => v.CreateSeExampleValueDesc()).ToList() ?? new List<ExampleValueDesc>();
            return r;
        }
    }
}
