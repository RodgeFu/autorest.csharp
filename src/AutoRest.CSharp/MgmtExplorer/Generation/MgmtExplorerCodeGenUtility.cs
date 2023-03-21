// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Mgmt.Decorator;
using AutoRest.CSharp.Mgmt.Models;
using AutoRest.CSharp.Mgmt.Output;
using AutoRest.CSharp.MgmtExplorer.Models;
using AutoRest.CSharp.Utilities;
using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Humanizer;

namespace AutoRest.CSharp.MgmtExplorer.Generation
{
    internal class MgmtExplorerCodeGenUtility
    {
        public const string TAB_STRING = "    ";

        public static MgmtExplorerVariable WriteGetArmClient(MgmtExplorerCodeSegmentWriter writer)
        {
            writer.UseNamespace("Azure.Identity");
            return WriteDefineVariableEqualsExpression(writer, typeof(ArmClient), "armClient", $"new {typeof(ArmClient)}(new DefaultAzureCredential())");
        }

        #region Write Get something
        public static MgmtExplorerVariable WriteGetTenantResource(MgmtExplorerCodeSegmentWriter writer, MgmtExtensions tenantExtension, MgmtExplorerVariable armClientVar)
        {
            return WriteDefineVariableEqualsExpression(writer, tenantExtension.ArmCoreType, "tenantResource", $"{armClientVar.KeyDeclaration}.GetTenants().GetAllAsync().GetAsyncEnumerator().Current");
        }

        //public static MgmtExplorerVariable WriteGetScopeResource(MgmtExplorerCodeSegmentWriter writer, RequestPath scopeRequestPath, List<MgmtExplorerParameter> scopeParameters, MgmtExplorerVariable armClientVar)
        //{
        //    List<FormattableString> list = new List<FormattableString>();

        //    scopeRequestPath

        //    foreach (MgmtExplorerParameter mep in scopeParameters)
        //    {
        //        writer.AddCodeSegmentParameter(mep.ToCodeSegmentParameter(true /*includeSchema*/));
        //        list.Add(FormattableStringFactory.Create(mep.Key));
        //    }

        //    return WriteDefineVariableEqualsExpression(writer, new MgmtExplorerVariable("scope", new CSharpType(typeof(string))),
        //        )
        //    return WriteDefineVariableEqualsFunc(writer,
        //        typeof(string), $"{type.Name}Id".ToVariableName(), $"{type}.CreateResourceIdentifier", list);
        //}

        public static MgmtExplorerVariable WriteGetExtensionResource(MgmtExplorerCodeSegmentWriter writer, MgmtExtensions extension, List<MgmtExplorerParameter> hostParameters, MgmtExplorerVariable armClientVar)
        {
            writer.UseNamespace(extension.Namespace);
            if (extension.ArmCoreType == typeof(TenantResource))
                return WriteGetTenantResource(writer, extension, armClientVar);
            else
            {
                var idVar = WriteGetResourceIdentifier(writer, extension, hostParameters);
                return WriteDefineVariableEqualsExpression(writer, extension.ArmCoreType, extension.ResourceName.ToVariableName(), $"{armClientVar.KeyDeclaration}.Get{extension.ArmCoreType.Name}({idVar.KeyDeclaration})");
            }
        }

        public static MgmtExplorerVariable WriteGetResourceCollection(MgmtExplorerCodeSegmentWriter writer, ResourceCollection collection, MgmtExplorerVariable collectionHost)
        {
            // TODO: checked the code and found nothing is added to ExtraConstructorParameters (or any special case?), so just use the name as serailizerName should be safe for now which can be verified when compiling generated code
            //       if any special case found, try to retrieve the constructor method -> methodParameter -> inputParameter -> seralizerName  (refer to GetOperationParameters)
            string requestPath = collection.RequestPath.SerializedPath ?? "";
            return WriteDefineVariableEqualsFuncWithVarDefined(writer,
                collection.Type, $"{collection.Type.Name.Camelize()}Var", $"{collectionHost.KeyDeclaration}.Get{collection.ResourceName.ResourceNameToPlural()}",
                collection.ExtraConstructorParameters.Select(p => new MgmtExplorerParameter(p, p.Name, p.Name, requestPath)));
        }

        public static MgmtExplorerVariable WriteGetResource(MgmtExplorerCodeSegmentWriter writer, Resource resource, List<MgmtExplorerParameter> hostParameters, MgmtExplorerVariable armClient)
        {
            var idVar = WriteGetResourceIdentifier(writer, resource, hostParameters);
            return WriteDefineVariableEqualsExpression(writer, resource.Type, $"{resource.Type.Name}Var".ToVariableName(), $"{armClient.KeyDeclaration}.Get{resource.Type.Name}({idVar.KeyDeclaration})");
        }

        public static MgmtExplorerVariable WriteGetResourceIdentifier(MgmtExplorerCodeSegmentWriter writer, MgmtExtensions extension, List<MgmtExplorerParameter> hostParameters)
        {
            return WriteGetResourceIdentifier(writer, extension.ArmCoreType, hostParameters);
        }

        public static MgmtExplorerVariable WriteGetResourceIdentifier(MgmtExplorerCodeSegmentWriter writer, Resource resource, List<MgmtExplorerParameter> hostParameters)
        {
            return WriteGetResourceIdentifier(writer, resource.Type, hostParameters);
        }

        public static MgmtExplorerVariable WriteGetResourceIdentifier(MgmtExplorerCodeSegmentWriter writer, CSharpType type, List<MgmtExplorerParameter> hostParameters)
        {
            List<FormattableString> list = new List<FormattableString>();
            foreach (MgmtExplorerParameter mep in hostParameters)
            {
                writer.AddCodeSegmentParameter(mep.ToCodeSegmentParameter(true /*includeSchema*/));
                list.Add(FormattableStringFactory.Create(mep.Key));
            }

            return WriteDefineVariableEqualsFunc(writer,
                typeof(ResourceIdentifier), $"{type.Name}Id".ToVariableName(), $"{type}.CreateResourceIdentifier", list);
        }
        #endregion

        #region Write Invoke some operation

        public static MgmtExplorerVariable WriteInvokeLongRunningOperation(MgmtExplorerCodeSegmentWriter writer, MgmtClientOperation operation, List<MgmtExplorerParameter> parameters, MgmtExplorerVariable providerVar)
        {
            // LongRunningOperation should be returning something like ArmOperation<...> or ArmOperation
            // TODO: use non-async SDK, any problem?
            //var lro = WriteDefineVariableEqualsFuncWithVarDefined(writer, operation.ReturnType, "lro", $"await {providerVar.KeyDeclaration}.{operation.Name}Async", parameters);
            var lro = WriteDefineVariableEqualsFuncWithVarDefined(writer, operation.ReturnType, "lro", $"{providerVar.KeyDeclaration}.{operation.Name}", parameters);
            if (operation.ReturnType.IsGenericType)
            {
                return WriteDefineVariableEqualsExpression(writer, operation.ReturnType.Arguments.First(), "result", $"{lro.KeyDeclaration}.Value");
            }
            else
            {
                return WriteDefineVariableEqualsExpression(writer, typeof(Azure.Response), "result", $"{lro.KeyDeclaration}.WaitForCompletionResponse()");
            }
        }

        public static MgmtExplorerVariable WriteInvokePagedOperation(MgmtExplorerCodeSegmentWriter writer, MgmtClientOperation operation, List<MgmtExplorerParameter> parameters, MgmtExplorerVariable providerVar)
        {
            var po = WriteDefineVariableEqualsFuncWithVarDefined(writer, new CSharpType(typeof(Pageable<>), operation.ReturnType), $"{operation.Resource?.Type.Name.ToVariableName() ?? "paged"}List", $"{providerVar.KeyDeclaration}.{operation.Name}", parameters);
            CSharpType listType = new CSharpType(typeof(List<>), operation.ReturnType);
            // TODO: does tolist work?
            return WriteDefineVariableEqualsExpression(writer, listType, "result", $"{po.KeyDeclaration}.ToList()");
        }

        internal static MgmtExplorerVariable WriteInvokeNormalOperation(MgmtExplorerCodeSegmentWriter writer, MgmtClientOperation operation, List<MgmtExplorerParameter> parameters, MgmtExplorerVariable providerVar)
        {
            // Expect to get Azure.Response or Azure.Response<T> ?
            CSharpType returnType = operation.ReturnType.IsGenericType ? operation.ReturnType.Arguments.First() : operation.ReturnType;
            // TODO: use non-async SDK instead, any problem?
            //return WriteDefineVariableEqualsFuncWithVarDefined(writer, returnType, "result", $"await {providerVar.KeyDeclaration}.{operation.Name}Async", parameters);
            return WriteDefineVariableEqualsFuncWithVarDefined(writer, returnType, "result", $"{providerVar.KeyDeclaration}.{operation.Name}", parameters);
        }
        #endregion

        private static MgmtExplorerVariable WriteDefineVariableEqualsFuncWithVarDefined(MgmtExplorerCodeSegmentWriter writer, CSharpType type, string varSuggestedName, FormattableString methodName, IEnumerable<MgmtExplorerParameter> parameters, bool postNewLine = true, bool postSemiColon = true)
        {
            List<FormattableString> list = new List<FormattableString>();
            foreach (var mep in parameters)
            {
                MgmtExplorerVariable mev = new MgmtExplorerVariable($"{mep.CSharpName}Var".ToVariableName(), mep.Type);
                WriteDefineVariableEqualsExpression(writer, mev, FormattableStringFactory.Create(mep.Key));
                writer.AddCodeSegmentParameter(mep.ToCodeSegmentParameter(true /*includeSchema*/));

                string named = mep.DefaultValue != null ? $"{mep.CSharpName}: " : $"";
                list.Add($"{named}{mev.KeyDeclaration}");
            }
            return WriteDefineVariableEqualsFunc(writer, type, varSuggestedName, methodName, list, postNewLine, postSemiColon);
        }

        private static MgmtExplorerVariable WriteDefineVariableEqualsFunc(MgmtExplorerCodeSegmentWriter writer, CSharpType type, string varSuggestedName, FormattableString methodName, IEnumerable<FormattableString> parameterVariableStrings, bool postNewLine = true, bool postSemiColon = true)
        {
            var r = WriteDefineVariableEqualsExpression(writer, type, varSuggestedName, $"{methodName}(", false /*newline*/, false /*postSemicolon*/);
            foreach (var str in parameterVariableStrings)
            {
                Line(writer);
                Tab(writer);
                writer.Append($"{str},");
            }
            writer.RemoveTrailingComma();
            writer.AppendRaw(");");
            if (postNewLine)
                Line(writer);
            return r;
        }

        private static MgmtExplorerVariable WriteDefineVariableEqualsExpression(MgmtExplorerCodeSegmentWriter writer, CSharpType type, string varSuggestedName, FormattableString expression, bool postNewLine = true, bool postSemiColon = true)
        {
            return WriteDefineVariableEqualsExpression(writer, new MgmtExplorerVariable(varSuggestedName, type), expression, postNewLine, postSemiColon);
        }

        private static MgmtExplorerVariable WriteDefineVariableEqualsExpression(MgmtExplorerCodeSegmentWriter writer, MgmtExplorerVariable variable, FormattableString expression, bool postNewLine = true, bool postSemiColon = true)
        {
            string semiColon = postSemiColon ? ";" : "";
            writer.Append($"{variable.Type} {variable.KeyDeclaration:D} = {expression}{semiColon}");
            if (postNewLine)
                Line(writer);
            writer.AddCodeSegmentVariable(variable.AsCodeSegmentVariable());
            return variable;
        }

        public static void Line(MgmtExplorerCodeSegmentWriter writer)
        {
            writer.Line();
        }

        public static void Tab(MgmtExplorerCodeSegmentWriter writer)
        {
            writer.AppendRaw(TAB_STRING);
        }
    }
}
