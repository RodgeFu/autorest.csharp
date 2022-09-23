// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Generation.Writers;
using AutoRest.CSharp.Mgmt.Decorator;
using AutoRest.CSharp.Mgmt.Models;
using AutoRest.CSharp.Mgmt.Output;
using AutoRest.CSharp.MgmtExplorer.Contract;
using AutoRest.CSharp.MgmtExplorer.Models;
using AutoRest.CSharp.Output.Models;
using AutoRest.CSharp.Output.Models.Shared;
using AutoRest.CSharp.Utilities;
using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

namespace AutoRest.CSharp.MgmtExplorer.Generation
{
    internal class MgmtExplorerCodeGenUtility
    {
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

        public static MgmtExplorerVariable WriteGetExtensionResource(MgmtExplorerCodeSegmentWriter writer, MgmtExtensions extension, MgmtExplorerVariable armClientVar)
        {
            if (extension.ArmCoreType == typeof(TenantResource))
                return WriteGetTenantResource(writer, extension, armClientVar);
            else
            {
                var idVar = WriteGetResourceIdentifier(writer, extension);
                return WriteDefineVariableEqualsExpression(writer, extension.ArmCoreType, extension.ResourceName.ToVariableName(), $"{armClientVar.KeyDeclaration}.Get{extension.ArmCoreType.Name}({idVar.KeyDeclaration})");
            }
        }

        public static MgmtExplorerVariable WriteGetResourceCollection(MgmtExplorerCodeSegmentWriter writer, ResourceCollection collection, MgmtExplorerVariable collectionHost)
        {
            return WriteDefineVariableEqualsFuncWithVarDefined(writer,
                collection.Type, $"{collection.Type.Name}Var", $"{collectionHost.KeyDeclaration}.Get{collection.ResourceName.ResourceNameToPlural()}", collection.ExtraConstructorParameters);
        }

        public static MgmtExplorerVariable WriteGetResource(MgmtExplorerCodeSegmentWriter writer, Resource resource, MgmtExplorerVariable armClient)
        {
            var idVar = WriteGetResourceIdentifier(writer, resource);
            return WriteDefineVariableEqualsExpression(writer, resource.Type, $"{resource.Type.Name}Var".ToVariableName(), $"{armClient.KeyDeclaration}.Get{resource.Type.Name}({idVar.KeyDeclaration})");
        }

        public static MgmtExplorerVariable WriteGetResourceIdentifier(MgmtExplorerCodeSegmentWriter writer, MgmtExtensions extension)
        {
            return WriteGetResourceIdentifier(writer, extension.ArmCoreType, extension.ContextualPath);
        }

        public static MgmtExplorerVariable WriteGetResourceIdentifier(MgmtExplorerCodeSegmentWriter writer, Resource resource)
        {
            return WriteGetResourceIdentifier(writer, resource.Type, resource.RequestPath);
        }

        public static MgmtExplorerVariable WriteGetResourceIdentifier(MgmtExplorerCodeSegmentWriter writer, CSharpType type, RequestPath requestPath)
        {
            List<FormattableString> list = new List<FormattableString>();
            foreach (var s in requestPath.Where(s => s.IsReference))
            {
                MgmtExplorerParameter mep = new MgmtExplorerParameter(s.ReferenceName, typeof(string), null, null);
                writer.AddCodeSegmentParameter(mep.AsCodeSegmentParameter());
                list.Add(FormattableStringFactory.Create(mep.Key));
            }

            return WriteDefineVariableEqualsFunc(writer,
                typeof(ResourceIdentifier), $"{type.Name}Id".ToVariableName(), $"{type}.CreateResourceIdentifier", list);
        }
        #endregion

        #region Write Invoke some operation
        private static IEnumerable<Parameter> GetOperationParameters(MgmtClientOperation operation)
        {
            return operation.MethodSignature.Modifiers.HasFlag(MethodSignatureModifiers.Extension) ?
                operation.MethodParameters.Skip(1) : operation.MethodParameters;
        }

        public static MgmtExplorerVariable WriteInvokeLongRunningOperation(MgmtExplorerCodeSegmentWriter writer, MgmtClientOperation operation, MgmtExplorerVariable providerVar)
        {
            // LongRunningOperation should be returning something like ArmOperation<...> or ArmOperation
            var lro = WriteDefineVariableEqualsFuncWithVarDefined(writer, operation.ReturnType, "lro", $"await {providerVar.KeyDeclaration}.{operation.Name}Async", GetOperationParameters(operation));
            if (operation.ReturnType.IsGenericType)
            {
                return WriteDefineVariableEqualsExpression(writer, operation.ReturnType.Arguments.First(), "result", $"{lro.KeyDeclaration}.Value");
            }
            else
            {
                return WriteDefineVariableEqualsExpression(writer, typeof(Azure.Response), "result", $"{lro.KeyDeclaration}.WaitForCompletionResponse()");
            }
        }

        public static MgmtExplorerVariable WriteInvokePagedOperation(MgmtExplorerCodeSegmentWriter writer, MgmtClientOperation operation, MgmtExplorerVariable providerVar)
        {
            var po = WriteDefineVariableEqualsFuncWithVarDefined(writer, new CSharpType(typeof(AsyncPageable<>), operation.ReturnType), $"{operation.Resource?.Type.Name.ToVariableName() ?? "paged"}List", $"await {providerVar.KeyDeclaration}.{operation.Name}Async", GetOperationParameters(operation));
            CSharpType listType = new CSharpType(typeof(List<>), operation.ReturnType);
            // TODO: does tolist work?
            return WriteDefineVariableEqualsExpression(writer, listType, "result", $"{po.KeyDeclaration}.ToList()");
        }

        internal static MgmtExplorerVariable WriteInvokeNormalOperation(MgmtExplorerCodeSegmentWriter writer, MgmtClientOperation operation, MgmtExplorerVariable providerVar)
        {
            // Expect to get Azure.Response or Azure.Response<T> ?
            CSharpType returnType = operation.ReturnType.IsGenericType ? operation.ReturnType.Arguments.First() : operation.ReturnType;
            return WriteDefineVariableEqualsFuncWithVarDefined(writer, returnType, "result", $"await {providerVar.KeyDeclaration}.{operation.Name}Async", GetOperationParameters(operation));
        }
        #endregion

        private static MgmtExplorerVariable WriteDefineVariableEqualsFuncWithVarDefined(MgmtExplorerCodeSegmentWriter writer, CSharpType type, string varSuggestedName, FormattableString methodName, IEnumerable<Parameter> parameters, bool postNewLine = true, bool postSemiColon = true)
        {
            List<FormattableString> list = new List<FormattableString>();
            foreach (var param in parameters)
            {
                MgmtExplorerParameter mep = new MgmtExplorerParameter(param);
                MgmtExplorerVariable mev = new MgmtExplorerVariable($"{param.Name}Var".ToVariableName(), param.Type);
                WriteDefineVariableEqualsExpression(writer, mev, FormattableStringFactory.Create(mep.Key));
                writer.AddCodeSegmentParameter(mep.AsCodeSegmentParameter());
                writer.AddCodeSegmentVariable(mev.AsCodeSegmentVariable());

                string named = param.DefaultValue != null ? $"{mep.ParameterName}: " : $"";
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
            const string tab = "    ";
            writer.AppendRaw(tab);
        }
    }
}
