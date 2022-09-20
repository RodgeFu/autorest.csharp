// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Generation.Writers;
using AutoRest.CSharp.Mgmt.Decorator;
using AutoRest.CSharp.Mgmt.Models;
using AutoRest.CSharp.Mgmt.Output;
using AutoRest.CSharp.MgmtExplorer.Models;
using AutoRest.CSharp.Output.Models.Shared;
using AutoRest.CSharp.Utilities;
using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

namespace AutoRest.CSharp.MgmtExplorer.Generation
{
    internal class MgmtExplorerWriterUtility
    {
        public static MgmtExplorerVariable WriteGetArmClient(CodeWriter writer)
        {
            writer.UseNamespace("Azure.Identity");
            return WriteDefineVariableEqualsExpression(writer, typeof(ArmClient), "client", $"new {typeof(ArmClient)}(new DefaultAzureCredential())");
        }

        public static MgmtExplorerVariable WriteGetTenantResource(CodeWriter writer, MgmtExtensions tenantExtension, MgmtExplorerVariable armClientVar)
        {
            return WriteDefineVariableEqualsExpression(writer, tenantExtension.ArmCoreType, "tenant", $"{armClientVar.Declaration}.GetTenants().GetAllAsync().GetAsyncEnumerator().Current");
        }

        public static MgmtExplorerVariable WriteGetExtensionResource(CodeWriter writer, MgmtExtensions extension, MgmtExplorerVariable armClientVar)
        {
            if (extension.ArmCoreType == typeof(TenantResource))
                return WriteGetTenantResource(writer, extension, armClientVar);
            else
            {
                var idVar = WriteGetResourceIdentifier(writer, extension);
                return WriteDefineVariableEqualsExpression(writer, extension.ArmCoreType, extension.ResourceName.ToVariableName(), $"{armClientVar.Declaration}.Get{extension.ArmCoreType.Name}({idVar})");
            }
        }

        public static MgmtExplorerVariable WriteGetResourceCollection(CodeWriter writer, ResourceCollection collection, MgmtExplorerVariable collectionHost)
        {
            FormattableString getCollectionMethodName = $"Get{collection.ResourceName.ResourceNameToPlural()}";
            CSharpType providerType = collection.Resource.Type;
            CodeWriterDeclaration providerVar = new CodeWriterDeclaration("collectionVar");

            return WriteDefineVariableEqualsFuncWithVarDefined(writer,
                collection.Type, "collectionVar", $"{collectionHost.Declaration}.Get{collection.ResourceName.ResourceNameToPlural()}", collection.ExtraConstructorParameters);
        }

        public static MgmtExplorerVariable WriteGetResource(CodeWriter writer, Resource resource, MgmtExplorerVariable armClient)
        {
            var idVar = WriteGetResourceIdentifier(writer, resource);
            return WriteDefineVariableEqualsExpression(writer, resource.Type, $"{resource.Type.Name}Var".ToVariableName(), $"{armClient.Declaration}.Get{resource.Type}({idVar})");
        }

        public static MgmtExplorerVariable WriteGetResourceIdentifier(CodeWriter writer, MgmtExtensions extension)
        {
            return WriteGetResourceIdentifier(writer, extension.ArmCoreType, extension.ContextualPath);
        }

        public static MgmtExplorerVariable WriteGetResourceIdentifier(CodeWriter writer, Resource resource)
        {
            return WriteGetResourceIdentifier(writer, resource.Type, resource.RequestPath);
        }

        public static MgmtExplorerVariable WriteGetResourceIdentifier(CodeWriter writer, CSharpType type, RequestPath requestPath)
        {
            return WriteDefineVariableEqualsFunc(writer,
                typeof(ResourceIdentifier), $"{type.Name}Id".ToVariableName(), $"{type}.CreateResourceIdentifier", requestPath.Where(s => s.IsReference).Select(s => FormattableStringFactory.Create("{{{0}_ParamName}}", s.ReferenceName)));
        }

        public static MgmtExplorerVariable WriteInvokeLongRunningOperation(CodeWriter writer, MgmtRestOperation operation, MgmtExplorerVariable providerVar)
        {
            // LongRunningOperation should be returning something like ArmOperation<...> or ArmOperation
            var lro = WriteDefineVariableEqualsFuncWithVarDefined(writer, operation.ReturnType, "lro", $"await {providerVar.Declaration}.{operation.Name}Async", operation.Parameters);
            if (operation.ReturnType.IsGenericType)
            {
                return WriteDefineVariableEqualsExpression(writer, operation.ReturnType.Arguments.First(), "result", $"{lro}.Value");
            }
            else
            {
                // TODO: more check needed
                Debugger.Break();
                return WriteDefineVariableEqualsExpression(writer, typeof(Azure.Response), "result", $"{lro}.WaitForCompletionResponse");
            }
        }

        public static MgmtExplorerVariable WriteInvokePagedOperation(CodeWriter writer, MgmtRestOperation op, MgmtExplorerVariable providerVar)
        {
            var po = WriteDefineVariableEqualsFuncWithVarDefined(writer, new CSharpType(typeof(AsyncPageable<>), op.ReturnType), $"{op.Resource!.Type.Name}List", $"await {providerVar.Declaration}.{op.Name}Async", op.Parameters);
            CSharpType listType = new CSharpType(typeof(List<>), op.ReturnType);
            // TODO: does tolist work?
            return WriteDefineVariableEqualsExpression(writer, listType, "result", $"{po}.ToList()");
        }

        internal static MgmtExplorerVariable? WriteInvokeNormalOperation(CodeWriter writer, MgmtRestOperation op, MgmtExplorerVariable providerVar)
        {
            // Expect to get Azure.Response or Azure.Response<T> ?
            CSharpType returnType = op.ReturnType.IsGenericType ? op.ReturnType.Arguments.First() : op.ReturnType;
            // TODO: DOUBLE check needed
            if (!op.ReturnType.IsGenericType)
            {
                Debugger.Break();
            }
            return WriteDefineVariableEqualsFuncWithVarDefined(writer, returnType, "result", $"await {providerVar.Declaration}.{op.Name}Async", op.Parameters);
        }

        private static MgmtExplorerVariable WriteDefineVariableEqualsFunc(CodeWriter writer, CSharpType type, string varName, FormattableString methodName, IEnumerable<FormattableString> parameterVariableStrings, bool postNewLine = true, bool postSemiColon = true)
        {
            var r = WriteDefineVariableEqualsExpression(writer, type, varName, $"{methodName}(", false /*newline*/, false /*postSemicolon*/);
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

        private static MgmtExplorerVariable WriteDefineVariableEqualsFuncWithVarDefined(CodeWriter writer, CSharpType type, string varName, FormattableString methodName, IEnumerable<Parameter> parameters, bool postNewLine = true, bool postSemiColon = true)
        {
            List<MgmtExplorerParameterVariable> pVarList = new List<MgmtExplorerParameterVariable>();
            foreach (var param in parameters)
            {
                MgmtExplorerParameterVariable pVar = new MgmtExplorerParameterVariable(param);
                WriteDefineVariableEqualsExpression(writer,
                    param.Type, pVar.Variable, $"{pVar.Value_Default}");
                pVarList.Add(pVar);
            }
            return WriteDefineVariableEqualsFunc(writer, type, varName, methodName, pVarList.Select(p => FormattableStringFactory.Create(p.NameInMethodInvoke())), postNewLine, postSemiColon);
        }

        private static MgmtExplorerVariable WriteDefineVariableEqualsExpression(CodeWriter writer, CSharpType type, string varName, FormattableString expression, bool postNewLine = true, bool postSemiColon = true)
        {
            return WriteDefineVariableEqualsExpression(writer, type, new CodeWriterDeclaration(varName), expression, postNewLine, postSemiColon);
        }

        private static MgmtExplorerVariable WriteDefineVariableEqualsExpression(CodeWriter writer, CSharpType type, CodeWriterDeclaration var, FormattableString expression, bool postNewLine = true, bool postSemiColon = true)
        {
            string semiColon = postSemiColon ? ";" : "";
            writer.Append($"{type} {var:D} = {expression}{semiColon}");
            if (postNewLine)
                Line(writer);
            return new MgmtExplorerVariable(var, type);
        }

        public static void Line(CodeWriter writer)
        {
            writer.Line();
        }

        public static void Tab(CodeWriter writer)
        {
            const string tab = "    ";
            writer.AppendRaw(tab);
        }
    }
}
